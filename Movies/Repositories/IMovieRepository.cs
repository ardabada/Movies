using System.Collections.Generic;

namespace Movies.Repositories
{
    public interface IMovieRepository
    {
        List<string> GetFavorites();
        void AddFavorite(int movieId);
        void RemoveFavorite(int movieId);
    }
}
