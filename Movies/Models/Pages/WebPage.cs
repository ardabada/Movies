using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Models.Pages
{
    public class WebPage
    {
        public string Title { get; set; }
        public string AbsoluteUrl { get; set; }
        public bool IsAjax { get; set; }
    }
}
