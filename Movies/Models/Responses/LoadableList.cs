using System.Collections.Generic;

namespace Movies.Models.Responses
{
    public class LoadableList<T>
    {
        public LoadableList() { }
        public LoadableList(List<T> result, bool canLoadMore, int currentPage)
        {
            CanLoadMore = canLoadMore;
            Result = result;
            CurrentPage = currentPage;
        }

        public int CurrentPage { get; set; }
        public bool CanLoadMore { get; set; }
        public List<T> Result { get; set; }
    }
}
