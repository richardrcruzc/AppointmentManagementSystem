using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Domain
{
    public class Client : BaseEntity
    {
      
        [Required]
        [StringLength(50, MinimumLength = 2,ErrorMessage = "Last name cannot be longer than 50 characters or less than 2 characters.")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name cannot be longer than 50 characters or less than 2 characters.")]
        public string FirstName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool Active { get; set; }
        [StringLength(50, MinimumLength = 2)]
        public string SendNotificationBy { get; set; }
        public bool AcceptsMarketingNotifications { get; set; }

        public string Photo { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }
                
         

        public ICollection<Appointment> Appointment { get; set; }

        
    }
}
