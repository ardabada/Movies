using Microsoft.AspNetCore.Mvc;
using Movies.Models.Requests;
using Movies.Services;

namespace Movies.Controllers
{
    [ApiController]
    [ValidateAntiForgeryToken]
    public class ApiController : ControllerBase
    {
        readonly IMovieService movieService;

        public ApiController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [HttpPost("favorites")]
        public IActionResult AddToFavorites([FromBody] FavoritesRequest request)
        {
            movieService.AddToFavorites(request);
            return Ok();
        }
        [HttpDelete("favorites")]
        public IActionResult RemoveFromFavorites([FromBody] FavoritesRequest request)
        {
            movieService.RemoveFromFavorites(request);
            return Ok();
        }
    }
}
