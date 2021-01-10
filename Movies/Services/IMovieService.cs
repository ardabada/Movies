using Movies.Models.Requests;
using Movies.Models.Responses;
using System.Threading.Tasks;

namespace Movies.Services
{
    public interface IMovieService
    {
        Task<LoadableList<MovieQuickInfo>> GetPopularMovies(int page);
        Task<LoadableList<MovieQuickInfo>> GetTopRatedMovies(int page);
        Task<MovieDetails> GetMovieDetails(string movieId);

        void AddToFavorites(FavoritesRequest request);
        void RemoveFromFavorites(FavoritesRequest request);
    }
}
