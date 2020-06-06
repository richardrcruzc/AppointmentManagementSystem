using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Domain
{
    public class CategoryModel : BaseEntity
    {
         
        public string Description { get; set; }

        public ICollection<Service> Services { get; set; }
    }
}
