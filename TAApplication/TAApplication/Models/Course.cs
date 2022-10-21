/**
 * Author:    Sebastian Ramirez
 * Partner:   Noah Carlson
 * Date:      10/8/2022
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and [Your Name(s)] - This work may not be copied for use in Academic Coursework.
 *
 * I, Sebastian Ramirez and Noah Carlson, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents
 *
 *   TODO
 */

using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;


namespace TAApplication.Models
{
    public class Course
    {

        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Semester Offered:", ShortName = "Semester Offered", Prompt = "e.g. Spring", Description = "This is the semester within the year that this course if offered.")]
        public string? SemesterOffered { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Year Offered:", ShortName = "Year Offered", Prompt = "e.g. 2023", Description = "This is the year that this course will be offered.")]
        public string? YearOffered { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Title of Course:", ShortName = "Title", Prompt = "e.g. Web Software", Description = "This is the name of the course.")]
        public string? Title { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Course Department:", ShortName = "Department", Prompt = "e.g. CS", Description = "This is the department that offers the course.")]
        public string? Department { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Course Number:", ShortName = "Number", Prompt = "e.g. 2420", Description = "This is the identifying number of the course.")]
        public string? Number { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Course Section:", ShortName = "Section", Prompt = "e.g. 001", Description = "This is the courses section where there may be multple sections offered.")]
        public string? Section { get; set; }

        [Required]
        [Display(Name = "Course Description:", ShortName = "Description", Prompt = "e.g. Course catalog description", Description = "This is the description of the course.")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Professor Unid: ", ShortName = "Prof Unid", Prompt = "e.g u000000", Description = "The professors unid")]
        [RegularExpression("^u[0-9]{7}")]
        public string? ProfessorUnid { get; set; }

        [Required]
        [Display(Name = "Professor's Name: ", ShortName = "Prof's Name", Prompt = "e.g Jim Cook", Description = "The professors name")]
        public string? ProfessorName { get; set; }

        [Required]
        [Display(Name = "Start Time:", ShortName = "Start Time", Prompt = "e.g 8:00", Description = "Course Start Time")]
        public TimeSpan Start { get; set; }

        [Required]
        [Display(Name = "End Time:", ShortName = "End Time", Prompt = "e.g 9:00", Description = "Course End Time")]
        public TimeSpan End { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Days offered:", ShortName = "Days", Prompt = "M/W", Description = "Days this class meets")]
        public string? DaysOffered { get; set; }

        [Required]
        [Display(Name = "Location:", ShortName = "Loc", Prompt = "e.g. WEB L104", Description = "Location of this course")]
        public string? Location { get; set; }

        [Required]
        [Display(Name = "Credit Hours:", ShortName = "Units", Prompt = "e.g. 3", Description = "How many credit hours this course is worth")]
        public int CreditHours { get; set; }

        [Required]
        [Display(Name = "Enrollment Count:", ShortName = "Enrollment", Prompt = "e.g. 150", Description = "How many students are enrolled in the class")]
        public int Enrollment { get; set; }

        [Display(Name = "Note:", ShortName = "Note", Prompt = "e.g. Needs Extra TAs", Description = "A place for the site admin to make notes about this course")]
        public string? Note { get; set; }

    }
}
