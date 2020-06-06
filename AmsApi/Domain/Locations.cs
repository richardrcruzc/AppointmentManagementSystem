using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Domain
{
    public class Location : BaseEntity
    {
        
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Description cannot be longer than 150 characters or less than 2 characters.")]
        public string Description { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Contact Name cannot be longer than 50 characters or less than 2 characters.")]
        public string ContactName { get; set; }
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Contact Email cannot be longer than 50 characters or less than 5 characters.")]
        public string ContactEmail { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Address cannot be longer than 50 characters or less than 2 characters.")]
        public string Address { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Address1 cannot be longer than 50 characters or less than 2 characters.")]
        public string Address1 { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "City cannot be longer than 50 characters or less than 2 characters.")]
        public string State { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "State cannot be longer than 50 characters or less than 2 characters.")]
        public string City { get; set; }
        [Required]
        [DataType(DataType.PostalCode)]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "ZipCode cannot be longer than 50 characters or less than 2 characters.")]
        public string ZipCode { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Country cannot be longer than 50 characters or less than 2 characters.")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(15,MinimumLength =10)]
        public string Phone { get; set; }

        public bool Active { get; set; }

        public bool Confirmation { get; set; }
        public bool Reminder { get; set; }
        public bool Rescheduling { get; set; }
        public bool ThankYou { get; set; }
        public bool Cancelation { get; set; }
        public bool NoShowUp { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        public ICollection<NotificationMessage> NotificationMessages { get; set; }
        public ICollection<ClosedDate> ClosedDates { get; set; }

    }
}
