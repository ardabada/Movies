using Newtonsoft.Json;
using System.Collections.Generic;

namespace Movies.Models.Vendor.TheMovieDb
{
    public class Pagination<T> : ListData<T>
    {
        [JsonProperty("page")]
        public int CurrentPage { get; set; }
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
    }
}
