using Newtonsoft.Json;

namespace Movies.Models.Vendor.TheMovieDb
{
    public class VideoInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("iso_639_1")]
        public string ISO639 { get; set; }
        [JsonProperty("iso_3166_1")]
        public string ISO3166 { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("site")]
        public string Site { get; set; }
        [JsonProperty("size")]
        public int Size { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
