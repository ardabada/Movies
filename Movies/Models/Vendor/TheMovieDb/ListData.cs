using Newtonsoft.Json;
using System.Collections.Generic;

namespace Movies.Models.Vendor.TheMovieDb
{
    public class ListData<T>
    {
        [JsonProperty("results")]
        public List<T> Result { get; set; }
    }
}
