using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Movies.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ViewResult()
            {
                StatusCode = (int)(context.HttpContext.Request.Query.ContainsKey("suppress_status_code") ? HttpStatusCode.OK : HttpStatusCode.InternalServerError),
                ViewName = "Views/error.cshtml"
            };
        }
    }
}
