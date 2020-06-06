using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Domain
{
    public class StaffWorkingHour : BaseEntity
    {
       
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime From { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime To { get; set; }
        public Repeat Repeat { get; set; }
        //Day, weekly, montly
        public EndRepeat EndRepeat { get; set; }
        //ongoin, expecific date
        public DateTime? EndDate { get; set; }
        public Location Location { get; set; }
        public Staff Staff { get; set; }

    }
    public enum Repeat
    {
        Day, Weekly, Montly
    }
    public enum EndRepeat
    {
        Ongoing, EspecificDate
    }
}
