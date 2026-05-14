using System.ComponentModel.DataAnnotations;

namespace TemplateWebService.Models.Settings
{
    public sealed class DatabaseSettings
    {
        [Required]
        [ConfigurationKeyName("LOMA_LOTTO")]
        public required ConnectionInfo LOMA_LOTTO { get; set; }
    }

    public sealed class ConnectionInfo
    {
        [Required]
        public required string ConnectionString { get; set; }
    }
}