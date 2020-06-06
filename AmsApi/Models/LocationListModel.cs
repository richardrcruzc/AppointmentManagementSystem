using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    public class LocationListModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public int TotalMsgs { get; set; }
        public int TotalAppt { get; set; }
        public int TotalClosed { get; set; }
    }
}
