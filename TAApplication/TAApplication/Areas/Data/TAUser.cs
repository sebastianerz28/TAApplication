using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TAApplication.Areas.Data
{
    [Index(nameof(Unid), IsUnique = true)]
    public class TAUser : IdentityUser
    {
        [Required]
        public string? Unid { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string? ReferredTo { get; set; }
    }
}
