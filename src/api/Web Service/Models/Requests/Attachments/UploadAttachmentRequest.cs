using System.ComponentModel.DataAnnotations;

namespace TemplateWebService.Models.Requests.Attachments
{
    public class UploadAttachmentRequest
    {
        [Required(ErrorMessage = "เธเธฃเธธเธ“เธฒเธฃเธฐเธเธธเนเธเธฅเน")]
        public IFormFile File { get; set; } = default!;

        public string? Folder { get; set; }

        // Default or Public
        public string? SourceName { get; set; }

        [Required(ErrorMessage = "เธเธฃเธธเธ“เธฒเธฃเธฐเธเธธเธเธนเนเธชเธฃเนเธฒเธ")]
        [MaxLength(100, ErrorMessage = "เธเธนเนเธชเธฃเนเธฒเธเธ•เนเธญเธเนเธกเนเน€เธเธดเธ 100 เธ•เธฑเธงเธญเธฑเธเธฉเธฃ")]
        public required string CreatedBy { get; set; }
    }
}