using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Domain
{
    public class Appointment: BaseEntity
    {
        
        public Client Client { get; set; }
        public int? LocationId { get; set; }
        public Location Location { get; set; }
        public int? ServiceId { get; set; }
        public Service Service { get; set; }
        public int? StaffId { get; set; }
        public Staff Staff { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Appointment Date and Time")]
        public DateTime StartTime { get; set; }
       
        
    }
}
