/**
 * Author:    Sebastian Ramirez
 * Partner:   Noah Carlson
 * Date:      09/22/2022
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and [Noah Carlson/Sebastian Ramirez] - This work may not be copied for use in Academic Coursework.
 *
 * I, Sebastian Ramirez, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents
 *      This File contains the properties for the TAUser along with specifying column properties for the actual table in the database
 */

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
