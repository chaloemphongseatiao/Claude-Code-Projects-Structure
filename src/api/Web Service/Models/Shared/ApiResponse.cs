namespace TemplateWebService.Models.Shared
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = false;

        public string? Message { get; set; }

        public T? Data { get; set; }
    }
}