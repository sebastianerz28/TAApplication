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
 *   This file serves as the model relating to an application contains all relevant fields.
 */


using System.ComponentModel.DataAnnotations;
using System.Windows.Markup;
using TAApplication.Areas.Data;

namespace TAApplication.Models
{
    public enum DegreePursuing { BS, Masters, BSMS, PhD }
    public class Application
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Your degree being pursued:", ShortName = "Degree Pursuing",Prompt ="BS", Description ="This data will help us assign you to the appropriate level of courses e.g(2xxx) level")]
        public DegreePursuing DegreePursuing { get; set;}

        [Required]
        [Display(Name = "Department of Major:", ShortName = "Department", Prompt = "CS, CE", Description = "This data will help us assign you to the correct type of classes")]
        [StringLength(150)]
        public string Department { get; set; }

        [Required]
        [Display(Name = "UofU GPA:", ShortName = "GPA", Prompt = "4.0", Description = "This is your current cumulative GPA.")]
        [Range(0,5)]
        public double GPA { get; set; }

        [Required]
        [Display(Name = "Desired Weekly Hours:", ShortName = "Desired Hours", Prompt = "e.g. 6", Description = "This helps determine scheduling.")]
        [Range(5,20)]
        public int DesiredHours { get; set; }

        [Required]
        [Display(Name = "Avalability before semester?:", ShortName = "Available", Prompt = "True", Description = "This is whether you will be able to start the week before school starts.")]
        public bool AvailableBeforeSemester { get; set; }

        [Required]
        [Display(Name = "Semesters Completed at UofU:", ShortName = "SemestersCompleted", Prompt = "e.g. 3", Description = "This is used to determine how many years the student has been at the university")]
        [Range(0,int.MaxValue)]
        public int SemestersCompleted { get; set; }

        [StringLength(50000)]
        [Display(Name = "Personal Statement:", ShortName = "PersonalStatement", Prompt = "Some information about your self.", Description = "This is a place where you can share who you are outside of the gpa. Let us know anything you deem important.")]
        [DataType(DataType.MultilineText)]
        [DisplayFormat(NullDisplayText ="Not Provided")]
        public string? PersonalStatement { get; set; }

        [Display(Name = "School Transferred From:", ShortName = "Transfer School", Prompt = "University of California Berkeley", Description = "Please let us know the school that you have transferred from if applicable")]
        [StringLength(150)]
        [DisplayFormat(NullDisplayText = "Not Provided")]
        public string? TransferSchool { get; set; }

        [Display(Name = "LinkedIn Profile:", ShortName = "LinkedIn", Prompt = "https://www.linkedin.com/", Description = "This links to the applicants LinkedIn profile")]
        [DisplayFormat(NullDisplayText = "Not Provided")]
        [Url]
        public string? LinkedIn { get; set; }

        [Display(Name = "Resume Upload:", ShortName = "Resume", Prompt = "Insert a personal resume", Description = "This is where you can input your personal resume to give more background on your accomplishments.")]
        [DisplayFormat(NullDisplayText = "Not Provided")]
        [RegularExpression("(.*\\.)(pdf)$", ErrorMessage ="File must be a pdf")]
        public string? ResumeName { get; set; }

        [Display(Name = "Profile Picture Upload:", ShortName = "Profile Upload", Prompt = "Insert a profile picture", Description = "Please input your profile picture")]
        [DisplayFormat(NullDisplayText = "Not Provided")]
        [RegularExpression("(.*\\.)(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$", ErrorMessage = "File must be a jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF")]
        public string? ProfilePicName { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}")]
        public DateTime CreationDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}")]
        public DateTime ModificationDate { get; set; }

        [Required]
        [Key]
        public TAUser TAUser { get; set; }
        
    }
}
