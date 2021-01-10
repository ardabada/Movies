using Microsoft.Extensions.Options;
using Movies.Converters;
using Movies.Models.Configuration;
using Movies.Models.Requests;
using Movies.Models.Responses;
using Movies.Models.Vendor.TheMovieDb;
using Movies.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Movies.Services.Impl
{
    public class MovieService : IMovieService
    {
        readonly TheMovieDbApiOptions theMovieDbApiOptions;
        readonly IHttpClientFactory httpClientFactory;
        readonly TheMovieDbConverter converter;
        readonly IMovieRepository movieRepository;

        public MovieService(
            IOptionsMonitor<TheMovieDbApiOptions> theMovieDbApiOptions,
            IHttpClientFactory httpClientFactory,
            TheMovieDbConverter converter,
            IMovieRepository movieRepository)
        {
            this.theMovieDbApiOptions = theMovieDbApiOptions.CurrentValue;
            this.httpClientFactory = httpClientFactory;
            this.converter = converter;
            this.movieRepository = movieRepository;
        }


        public void AddToFavorites(FavoritesRequest request)
        {
            movieRepository.AddFavorite(request.MovieId);
        }
        public void RemoveFromFavorites(FavoritesRequest request)
        {
            movieRepository.RemoveFavorite(request.MovieId);
        }

        public async Task<MovieDetails> GetMovieDetails(string movieId)
        {
            var data = await fetchMoviesDb<MovieWithVideosInfo>("/movie/" + movieId, new Dictionary<string, string>()
            {
                { "append_to_response", "videos" }
            });
            var result = converter.ConvertMovieWithVideosToMovieDetails(data);
            var favorites = movieRepository.GetFavorites();
            result.General.IsFavorite = favorites.Contains(result.General.MovieId.ToString());
            return result;
        }

        public async Task<LoadableList<MovieQuickInfo>> GetPopularMovies(int page)
        {
            return await fetchMoviesList("/discover/movie", page, "popularity.desc");
        }
        public async Task<LoadableList<MovieQuickInfo>> GetTopRatedMovies(int page)
        {
            return await fetchMoviesList("/discover/movie", page, "vote_average.desc");
        }

        async Task<LoadableList<MovieQuickInfo>> fetchMoviesList(string path, int page, string order)
        {
            if (page < 1)
                page = 1;
            var fetchedData = await fetchMoviesDb<Pagination<MovieGeneralInfo>>(path, new Dictionary<string, string>()
            {
                { "sort_by", order },
                { "page", page.ToString() },
                { "language", "en-US" },
                { "include_adult", "false" },
                { "include_video", "true" }
            });
            var list = fetchedData.Result.Select(x => converter.ConvertMovieListItemToQuickInfo(x)).ToList();
            var favorites = movieRepository.GetFavorites();
            foreach (var fav in favorites)
            {
                list.Where(x => x.MovieId.ToString() == fav).ToList().ForEach(x => x.IsFavorite = true);
            }
            bool canLoadMore = fetchedData.CurrentPage < fetchedData.TotalPages;
            return new LoadableList<MovieQuickInfo>(list, canLoadMore, page);
        }

        private async Task<T> fetchMoviesDb<T>(string path, Dictionary<string, string> query)
        {
            var client = httpClientFactory.CreateClient();
            if (query == null)
                query = new Dictionary<string, string>();
            query.Add("api_key", theMovieDbApiOptions.ApiKey);
            string url = theMovieDbApiOptions.BasePath + path + "?" + string.Join("&", query.Select(x => x.Key + "=" + Uri.EscapeDataString(x.Value ?? string.Empty)));
            var response = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<T>(response);
        }
    }
}
