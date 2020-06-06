using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Domain
{
    public class NotificationMessage : BaseEntity
    {
        public int? LocationId { get; set; }
    }
}
