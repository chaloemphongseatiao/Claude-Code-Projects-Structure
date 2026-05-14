using System.ComponentModel.DataAnnotations;

namespace TemplateWebService.Models.Requests.Lookups
{
    public class LookupUpsertRequest
    {
        [Required(ErrorMessage = "เธเธฃเธธเธ“เธฒเธฃเธฐเธเธธเธฃเธซเธฑเธช")]
        [MaxLength(10, ErrorMessage = "เธฃเธซเธฑเธชเธ•เนเธญเธเนเธกเนเน€เธเธดเธ 10 เธ•เธฑเธงเธญเธฑเธเธฉเธฃ")]
        public required string Code { get; set; }

        [Required(ErrorMessage = "เธเธฃเธธเธ“เธฒเธฃเธฐเธเธธเธเธทเนเธญ")]
        [MaxLength(255, ErrorMessage = "เธเธทเนเธญเธ•เนเธญเธเนเธกเนเน€เธเธดเธ 255 เธ•เธฑเธงเธญเธฑเธเธฉเธฃ")]
        public required string Name { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
    }
}