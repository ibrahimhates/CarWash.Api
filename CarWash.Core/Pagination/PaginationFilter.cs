namespace CarWash.Core.Pagination
{
    public class PaginationFilter
    {
        private int DefaultPageSize = 10;
        private int MaximumPageSize = 100;

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = DefaultPageSize;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > MaximumPageSize ? MaximumPageSize : pageSize <= 0 ? DefaultPageSize : pageSize;
        }
    }
}
