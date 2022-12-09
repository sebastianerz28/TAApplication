/**
 * Author:    Sebastian Ramirez
 * Partner:   Noah Carlson
 * Date:      12/9/2022
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Noah Carlson and Sebastian Ramirez - This work may not be copied for use in Academic Coursework.
 *
 * I, Sebastian Ramirez and Noah Carlson, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents:
 * 
 * This file represents the Enrollments Table
 *   
 */
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
        //CS 2420
        //CS2420
    }
}
