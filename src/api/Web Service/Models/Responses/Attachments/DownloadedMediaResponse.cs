using TemplateWebService.Models.Entities;

namespace TemplateWebService.Models.Responses.Attachments
{
    public class DownloadedMediaResponse
    {
        public Attachment Attachment { get; init; } = default!;

        public string FullPath { get; init; } = string.Empty;
    }
}