using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Controllers
{
    public class WebControllerBase : Controller
    {
        protected T CreatePage<T>(string title) where T : WebPage, new()
        {
            T page = new T();
            page.IsAjax = isAjaxRequest();
            page.Title = title;
            page.AbsoluteUrl = Request.Path.Value;
            return page;
        }

        bool isAjaxRequest()
        {
            const string ajaxHeaderName = "X-Requested-With";
            const string ajaxHeaderValue = "XMLHttpRequest";
            return Request.Headers.TryGetValue(ajaxHeaderName, out var value) && value.Contains(ajaxHeaderValue);
        }
    }
}
