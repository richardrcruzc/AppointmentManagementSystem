using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Domain
{    
    public class Message : BaseEntity
    {
        
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Text cannot be longer than 150 characters or less than 2 characters.")]
        public string Text { get; set; }
        public Staff Recipient { get; set; }
        public Method? Method { get; set; }
        public TypeMessage? TypeMessage { get; set; }

        public Location Location { get; set; }
    }

    public enum TypeMessage
    {
        Confirmation, Reminder, Rescheduling, ThankYou, Cancelation, NoShowUp
    }

    public enum Method
    { 
     Mail, SMS
    }
}
