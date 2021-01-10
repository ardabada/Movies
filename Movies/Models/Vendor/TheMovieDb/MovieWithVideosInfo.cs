using Newtonsoft.Json;
using System.Collections.Generic;

namespace Movies.Models.Vendor.TheMovieDb
{
    public class MovieWithVideosInfo : MovieGeneralInfo
    {
        [JsonProperty("imdb_id")]
        public string ImdbId { get; set; }
        [JsonProperty("runtime")]
        public int Runtime { get; set; }
        [JsonProperty("budget")]
        public long Budget { get; set; }
        [JsonProperty("revenue")]
        public long Revenue { get; set; }
        [JsonProperty("genres")]
        public List<Genre> Genres { get; set; }
        [JsonProperty("homepage")]
        public string Homepage { get; set; }

        [JsonProperty("videos")]
        public ListData<VideoInfo> Videos { get; set; }
    }
}
