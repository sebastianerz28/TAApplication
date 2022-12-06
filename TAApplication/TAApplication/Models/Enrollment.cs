using TAApplication.Areas.Data;
using System.ComponentModel.DataAnnotations;
namespace TAApplication.Models
{
    public class Enrollment
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Course { get; set; }
        [Required]
        public DateTime LastUpdated { get; set; }
        [Required]
        public int TotalEnrollment { get; set; }
    }
}
