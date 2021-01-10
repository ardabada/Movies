using Movies.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Models.Pages
{
    public class MoviesListPage : WebPage
    {
        public string DiscoverText { get; set; }
        public bool IsPaginationMode { get; set; }
        public LoadableList<MovieQuickInfo> Movies { get; set; }
    }
}
