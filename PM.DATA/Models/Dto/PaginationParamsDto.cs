namespace PM.DATA.Models.Dto
{
    public class PaginationParamsDto
    {
        public int PageNumber { get; set; } = 1;  // Default to the first page
        public int PageSize { get; set; } = 10;   // Default page size

        public PaginationParamsDto()
        {
            // Ensure PageNumber is always at least 1
            if (PageNumber < 1) PageNumber = 1;
            // Ensure PageSize is always at least 1
            if (PageSize < 1) PageSize = 10;
        }
    }

}
