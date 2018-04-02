using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolChallenge.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

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


        public int? NumberOfStudents { get; set; }
    }
}
