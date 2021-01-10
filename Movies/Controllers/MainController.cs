using Microsoft.AspNetCore.Mvc;
using Movies.Models.Pages;
using Movies.Services;
using System.Threading.Tasks;

namespace Movies.Controllers
{
    public class MainController : WebControllerBase
    {
        readonly IMovieService movieService;

        public MainController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return LocalRedirect(WebEndpoints.POPULAR_MOVIES);
        }

        [HttpGet(WebEndpoints.POPULAR_MOVIES)]
        public async Task<IActionResult> Popular([FromQuery(Name = "page")] int pageNumber)
        {
            var page = createMoviesListPage("Popular", ref pageNumber);
            page.DiscoverText = "Discover popular movies";
            page.Movies = await movieService.GetPopularMovies(pageNumber);
            return View("Views/movies_list.cshtml", page);
        }
        [HttpGet(WebEndpoints.TOP_RATED_MOVIES)]
        public async Task<IActionResult> TopRated([FromQuery(Name = "page")] int pageNumber)
        {
            var page = createMoviesListPage("Top rated", ref pageNumber);
            page.DiscoverText = "Discover best ranked movies";
            page.Movies = await movieService.GetTopRatedMovies(pageNumber);
            return View("Views/movies_list.cshtml", page);
        }

        [HttpGet(WebEndpoints.MOVIE_DETAILS)]
        public async Task<IActionResult> MovieDetails()
        {
            string key = WebEndpoints.GetRouteValueKey(WebEndpoints._MOVIE_ID_PLACEHOLDER);
            var movieId = Request.RouteValues[key].ToString();
            var details = await movieService.GetMovieDetails(movieId);
            var page = CreatePage<MovieDetailsPage>(details.General.Title);
            page.Movie = details;
            return View("Views/movie_details.cshtml", page);
        }

        MoviesListPage createMoviesListPage(string title, ref int pageNumber)
        {
            var page = CreatePage<MoviesListPage>(title);
            page.IsPaginationMode = Request.Headers.ContainsKey("pagination");
            if (!page.IsPaginationMode)
                pageNumber = 1;
            return page;
        }
    }
}
