namespace Movies.Models.Configuration
{
    public class TheMovieDbApiOptions
    {
        public string BasePath { get; set; }
        public string ApiKey { get; set; }
        public string PosterPath { get; set; }
        public string NoPosterPath { get; set; }
        public string YoutubeEmbed { get; set; }
    }
}
