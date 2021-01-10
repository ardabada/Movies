using Microsoft.Extensions.Options;
using Movies.Models.Configuration;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Movies.Repositories.Impl
{
    public class LocalFileMovieRepository : IMovieRepository
    {
        FavoritesOptions options;

        public LocalFileMovieRepository(IOptionsMonitor<FavoritesOptions> favoritesOptions)
        {
            options = favoritesOptions.CurrentValue;
        }

        public void AddFavorite(int movieId)
        {
            var favs = GetFavorites();
            favs.Add(movieId.ToString());
            save(favs);
        }
        public List<string> GetFavorites()
        {
            if (File.Exists(options.DataPath))
            {
                string content = File.ReadAllText(options.DataPath);
                return JsonConvert.DeserializeObject<string[]>(content).ToList();
            }
            return new List<string>();
        }
        public void RemoveFavorite(int movieId)
        {
            var favs = GetFavorites();
            favs.Remove(movieId.ToString());
            save(favs);
        }

        private void save(List<string> ids)
        {
            File.WriteAllText(options.DataPath, JsonConvert.SerializeObject(ids.Distinct()));
        }
    }
}
