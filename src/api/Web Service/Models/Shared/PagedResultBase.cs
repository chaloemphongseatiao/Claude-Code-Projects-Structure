namespace TemplateWebService.Models.Shared
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int RowCount { get; set; }

        public int PageCount =>
            PageSize <= 0
                ? 0
                : (int)Math.Ceiling((double)RowCount / Math.Max(1, PageSize));

        public int FirstRowOnPage =>
            RowCount == 0
                ? 0
                : (CurrentPage - 1) * PageSize + 1;

        public int LastRowOnPage =>
            Math.Min(CurrentPage * PageSize, RowCount);
    }
}