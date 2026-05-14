namespace TemplateWebService.Models.Entities
{
    public class Attachment
    {
        public long FileId { get; set; }

        public required string OriginalName { get; set; }

        public required string FileName { get; set; }

        public required string FilePath { get; set; }

        public required string FileExtension { get; set; }

        public required string ContentType { get; set; }

        public long FileSize { get; set; }

        public bool IsActive { get; set; }

        public required string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}