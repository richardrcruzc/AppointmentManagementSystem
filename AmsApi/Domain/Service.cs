using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Domain
{
    public class Service : BaseEntity
    {
       
        [StringLength(50, MinimumLength = 3)]
        public string ServiceName { get; set; }        
        public int DurationHour { get; set; }
        public int DurationMinute { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        public string Photo { get; set; }
        [StringLength(100, MinimumLength = 3)]
        public string ServiceDescription { get; set; }
        public bool ActiveStatus { get; set; }
        public int ServiceCategoryId { get; set; }
        public ServiceCategory ServiceCategory { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
