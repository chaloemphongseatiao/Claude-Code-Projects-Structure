namespace TemplateWebService.Models.Responses.Attachments
{
    public class AttachmentResponse
    {
        public long FileId { get; set; }

        public string OriginalName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public string FileExtension { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public bool IsActive { get; set; }

        public string PublicUrl { get; set; } = string.Empty;
    }
}