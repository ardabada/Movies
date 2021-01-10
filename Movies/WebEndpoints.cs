namespace Movies
{
    public class WebEndpoints
    {
        public const string _MOVIE_ID_PLACEHOLDER = "{movie_id}";

        public const string POPULAR_MOVIES = "/popular";
        public const string TOP_RATED_MOVIES = "/best";
        public const string MOVIE_DETAILS = "/movie/" + _MOVIE_ID_PLACEHOLDER;

        public static string GetRouteValueKey(string placeholder)
        {
            return placeholder.Replace("{", string.Empty).Replace("}", string.Empty);
        }
    }
}
