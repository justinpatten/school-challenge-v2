using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolChallenge.Models
{
    public class Student
    {
        [Key] 
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a student number!")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]*$", ErrorMessage = "Use only alphanumeric characters please!")]
        [StringLength(100)]
        [Display(Name = "Student Number")]
        public string StudentNumber { get; set; }

        [Required(ErrorMessage = "Please enter a first name!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use only alpha characters please!")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a last name!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use only alpha characters please!")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Does this Student have a Scholarship?")]
        public bool HasScholarship { get; set; }
    }
}
