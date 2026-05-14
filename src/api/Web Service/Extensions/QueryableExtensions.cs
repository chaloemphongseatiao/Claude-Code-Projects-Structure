using Microsoft.EntityFrameworkCore;

using TemplateWebService.Models.Shared;

namespace TemplateWebService.Extensions
{
    public static class QueryableExtensions
    {
        public static PagedResult<T> GetPaged<T>(
            this IQueryable<T> query,
            int page,
            int pageSize)
            where T : class
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            var skip = (page - 1) * pageSize;

            result.Results = query
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            return result;
        }

        public static async Task<PagedResult<T>> GetPagedAsync<T>(
            this IQueryable<T> query,
            int page,
            int pageSize,
            CancellationToken ct = default)
            where T : class
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = await query.CountAsync(ct)
            };

            var skip = (page - 1) * pageSize;

            result.Results = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);

            return result;
        }
    }
}