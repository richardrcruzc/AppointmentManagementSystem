using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Domain
{
    public interface IEntity
    {
        /// <summary>
        /// The unique id and primary key for entity
        /// </summary>
        public int Id { get; set; }

        //Auditable
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
    }
}
