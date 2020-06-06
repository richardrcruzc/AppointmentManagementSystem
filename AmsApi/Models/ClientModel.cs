using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name cannot be longer than 50 characters or less than 2 characters.")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name cannot be longer than 50 characters or less than 2 characters.")]
        public string FirstName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public bool SendNotificationBy { get; set; }
        public bool AcceptsMarketingNotifications { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }


        public ICollection<AppointmentModel> Appointment { get; set; }
    }
}
