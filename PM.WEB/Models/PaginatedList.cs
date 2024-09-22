namespace PM.WEB.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; } // List of items on the current page
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

}
