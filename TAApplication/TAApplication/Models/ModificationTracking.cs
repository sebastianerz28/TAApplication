using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TAApplication.Models
{
    public class ModificationTracking : Application
    {
        [Required]
        [ScaffoldColumn(false)]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}")]
        public DateTime CreationDate { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}")]
        public DateTime ModificationDate { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "Created By", ShortName = "Created By", Prompt = "U0000000", Description = "This is the creator of this application.")]
        public string CreatedBy { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "Modfified By", ShortName = "Modfified By", Prompt = "U000000", Description = "This is who last modified this application.")]
        public string ModifiedBy { get; set; }
    }
}
