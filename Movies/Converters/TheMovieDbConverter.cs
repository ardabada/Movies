using Microsoft.Extensions.Options;
using Movies.Models.Configuration;
using Movies.Models.Responses;
using Movies.Models.Vendor.TheMovieDb;
using System;
using System.Linq;

namespace Movies.Converters
{
    public class TheMovieDbConverter
    {
        readonly TheMovieDbApiOptions movieDbOptions;

        public TheMovieDbConverter(IOptionsMonitor<TheMovieDbApiOptions> movieDbOptions)
        {
            this.movieDbOptions = movieDbOptions.CurrentValue;
        }

        public MovieQuickInfo ConvertMovieListItemToQuickInfo(MovieGeneralInfo movie)
        {
            if (movie == null)
                return null;

            var result = new MovieQuickInfo()
            {
                MovieId = movie.Id,
                Title = movie.Title,
                Rank = movie.VoteAverage,
                ReleaseYear = movie.ReleaseDate?.Year,
                Poster = string.IsNullOrEmpty(movie.PosterPath) ? movieDbOptions.NoPosterPath : movieDbOptions.PosterPath + movie.PosterPath
            };
            return result;
        }
        public MovieDetails ConvertMovieWithVideosToMovieDetails(MovieWithVideosInfo movie)
        {
            if (movie == null)
                return null;

            var result = new MovieDetails();
            result.General = ConvertMovieListItemToQuickInfo(movie);
            result.Budget = movie.Budget;
            result.Revenue = movie.Revenue;
            result.Website = movie.Homepage;
            result.ImdbLink = "https://www.imdb.com/title/" + movie.ImdbId;
            var duration = TimeSpan.FromMinutes(movie.Runtime);
            result.Duration = duration.ToString();
            string teaserEmbed = null;
            if (movie.Videos != null && movie.Videos.Result != null)
            {
                var video = movie.Videos.Result.Where(x => x.Type.ToLower() == "teaser").FirstOrDefault();
                if (video != null && video.Site.ToLower() == "youtube")
                    teaserEmbed = movieDbOptions.YoutubeEmbed.Replace("{video-id}", video.Key);
            }
            result.Teaser = teaserEmbed;
            return result;
        }
    }
}
