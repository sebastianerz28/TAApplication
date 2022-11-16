using System.ComponentModel.DataAnnotations;
using TAApplication.Areas.Data;

namespace TAApplication.Models
{
    public class Availability
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Monday { get; set; }
        [Required]
        public string Tuesday { get; set; }
        [Required]
        public string Wednesday { get; set; }
        [Required]
        public string Thursday { get; set; }
        [Required]
        public string Friday { get; set; }
        [Required]
        [Key]
        public TAUser TAUser { get; set; }
    }
}
