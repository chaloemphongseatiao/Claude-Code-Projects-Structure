using System.ComponentModel.DataAnnotations;

namespace TemplateWebService.Models.Settings
{
    public sealed class EmailSettings
    {
        [Required]
        public required string Host { get; set; }

        [Required]
        public int Port { get; set; }

        [Required]
        public required string Security { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string FromEmail { get; set; }

        [Required]
        public required string FromName { get; set; }
    }
}