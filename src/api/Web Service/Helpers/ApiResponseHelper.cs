using TemplateWebService.Models.Shared;

namespace TemplateWebService.Helpers
{
    public class ApiResponseHelper
    {
        public static ApiResponse<T> Ok<T>(
            T data,
            string message = "เธ”เธณเน€เธเธดเธเธเธฒเธฃเธชเธณเน€เธฃเนเธ") =>
            new()
            {
                Success = true,
                Message = message,
                Data = data
            };

        public static ApiResponse<T> Fail<T>(
            string message) =>
            new()
            {
                Success = false,
                Message = message,
                Data = default
            };

        // เธ•เธฑเนเธเธเนเธฒ revealDetail = true เน€เธเธเธฒเธฐเธ•เธญเธ Dev/Debug เน€เธเธทเนเธญเธเนเธญเธเธเธฑเธเธเธฒเธฃเน€เธเธขเธฃเธฒเธขเธฅเธฐเน€เธญเธตเธขเธ”เธ เธฒเธขเนเธเนเธ Production
        public static ApiResponse<object> ApiError(
            Exception ex,
            bool revealDetail = false) =>
            new()
            {
                Success = false,
                Message = revealDetail
                   ? "เน€เธเธดเธ”เธเนเธญเธเธดเธ”เธเธฅเธฒเธ”: " + (ex.InnerException?.Message ?? ex.Message)
                   : "เน€เธเธดเธ”เธเนเธญเธเธดเธ”เธเธฅเธฒเธ”",
                Data = default
            };
    }
}