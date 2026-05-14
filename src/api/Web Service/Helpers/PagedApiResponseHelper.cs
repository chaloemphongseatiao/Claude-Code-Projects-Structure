using TemplateWebService.Models.Shared;

namespace TemplateWebService.Helpers
{
    public class PagedApiResponseHelper
    {
        public static PagedApiResponse<T> Ok<T>(
            T data,
            int page,
            int pageSize,
            int total,
            string message = "เธ”เธณเน€เธเธดเธเธเธฒเธฃเธชเธณเน€เธฃเนเธ") =>
            new()
            {
                Success = true,
                Message = message,
                Page = page,
                PageSize = pageSize,
                Total = total,
                Data = data
            };

        public static PagedApiResponse<T> Fail<T>(
            string message,
            int page = 0,
            int pageSize = 0,
            int total = 0) =>
            new()
            {
                Success = false,
                Message = message,
                Page = page,
                PageSize = pageSize,
                Total = total,
                Data = default
            };

        // เธ•เธฑเนเธเธเนเธฒ revealDetail = true เน€เธเธเธฒเธฐเธ•เธญเธ Dev/Debug เน€เธเธทเนเธญเธเนเธญเธเธเธฑเธเธเธฒเธฃเน€เธเธขเธฃเธฒเธขเธฅเธฐเน€เธญเธตเธขเธ”เธ เธฒเธขเนเธเนเธ Production
        public static PagedApiResponse<object> ApiError(
            Exception ex,
            bool revealDetail = false) =>
            new()
            {
                Success = false,
                Message = revealDetail
                    ? "เน€เธเธดเธ”เธเนเธญเธเธดเธ”เธเธฅเธฒเธ”: " + (ex.InnerException?.Message ?? ex.Message)
                    : "เน€เธเธดเธ”เธเนเธญเธเธดเธ”เธเธฅเธฒเธ”",
                Page = 0,
                PageSize = 0,
                Total = 0,
                Data = default
            };
    }
}