namespace TemplateWebService.Models.Shared
{
    public class PagedApiResponse<T> : ApiResponse<T>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }

        public int TotalPages =>
            PageSize <= 0
                ? 0
                : (int)Math.Ceiling((double)Total / Math.Max(1, PageSize));
    }
}