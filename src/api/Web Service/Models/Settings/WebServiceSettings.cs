using System.ComponentModel.DataAnnotations;

namespace TemplateWebService.Models.Settings
{
    public sealed class WebServiceSettings
    {
        [Required]
        public required DatabaseSettings Database { get; set; }

        [Required]
        public required FileServerSettings FileServer { get; set; }

        [Required]
        public required EmailSettings Smtp { get; set; }

        public string? PublicBaseUrl { get; set; }
    }
}