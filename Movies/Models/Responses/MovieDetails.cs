namespace Movies.Models.Responses
{
    public class MovieDetails
    {
        public MovieQuickInfo General { get; set; }
        public long Budget { get; set; }
        public long Revenue { get; set; }
        public string Teaser { get; set; }
        public string ImdbLink { get; set; }
        public string Duration { get; set; }
        public string Website { get; set; }
    }
}
