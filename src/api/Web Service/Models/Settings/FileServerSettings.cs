using System.ComponentModel.DataAnnotations;

namespace TemplateWebService.Models.Settings
{
    public sealed class FileServerSettings
    {
        [Required]
        public Dictionary<string, FileSourceDetail> FileSource { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }

    public sealed class FileSourceDetail
    {
        [Required]
        public required string RemotePath { get; set; }

        // URL prefix (เธ—เธฒเธเน€เธฅเธทเธญเธ) เธ–เนเธฒเนเธกเนเนเธชเน extension เธเธฐเธ•เธฑเนเธเน€เธเนเธ /files/{name}
        [RegularExpression(@"^\/[^\s\\*?#]*$", ErrorMessage = "RequestPath เธ•เนเธญเธเธเธถเนเธเธ•เนเธเธ”เนเธงเธข / เนเธฅเธฐเธซเนเธฒเธกเธกเธตเธเนเธญเธเธงเนเธฒเธ, \\, ?, #, *")]
        public string? RequestPath { get; set; }
    }
}