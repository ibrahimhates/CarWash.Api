using CarWash.Core.Pagination;

namespace CarWash.Service.ServiceExtensions
{
    public static class PaginationExtension
    {
        public static IQueryable<T> ApplyPagination<T>(
            this IQueryable<T> query,
            PaginationFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter), "PaginationFilter cannot be null.");
            }

            var pagedQuery = query;

            // PageNumber ve PageSize değerlerini null kontrolü yaparak atıyoruz.
            var pageNumber = filter.PageNumber;
            var pageSize = filter.PageSize;

            pagedQuery = pagedQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return pagedQuery;
        }
    }
}
