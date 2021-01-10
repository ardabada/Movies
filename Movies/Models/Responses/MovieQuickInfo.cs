namespace Movies.Models.Responses
{
    public class MovieQuickInfo
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int? ReleaseYear { get; set; }
        public double Rank { get; set; }
        public string Poster { get; set; }
        public bool IsFavorite { get; set; }
    }
}
