using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Domain
{
    public class Staff : BaseEntity/*, IValidatableObject*/
    {

        [Required(ErrorMessage = "LastName is required")]
        [StringLength(25, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(25, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(15, MinimumLength = 10)]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 5)]
        public string Email { get; set; }
        public bool Active { get; set; }
        public bool CanBook { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(500)]
        public string BriefCv{ get; set; }
         
       
        //[Required]
        //[DataType(DataType.Password)]
        //[Compare("Password")]
        //[NotMapped]
        //public string ConfirmPassword { get; set; }



        public ICollection<StaffWorkingHour> StaffWorkingHours { get; set; }


        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    var results = new List<ValidationResult>();
        //    if (  string.IsNullOrWhiteSpace(Email))
        //    {
        //        results.Add(new ValidationResult("Enter Email Address."));
        //    }

        //    return results;
        //}

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }


    }
    public enum Role
    {
        Admin, Employee
    }
}
