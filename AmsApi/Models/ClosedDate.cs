using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    public class ClosedDateModel 
    {
       
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Close Date and Time from")]
        public DateTime From { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Close Date and Time To")]
        public DateTime To { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Description cannot be longer than 50 characters or less than 2 characters.")]        
        public string Description { get; set; }
        public LocationModel Location { get; set; }
    }
}
