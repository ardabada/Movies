using Newtonsoft.Json;

namespace Movies.Models.Vendor.TheMovieDb
{
    public class Genre
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
