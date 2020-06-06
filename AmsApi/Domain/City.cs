

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmsApi.Domain
{
    public class City : BaseEntity
    {
        #region Constructor        
        public  City()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// City name (in UTF8 format)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// City name (in ASCII format)
        /// </summary>
        public string Name_ASCII { get; set; }
        /// <summary>
        /// City latitude
        /// </summary>
        [Column(TypeName = "decimal(7,4)")]        
        public decimal Lat { get; set; }
        /// <summary>
        /// City longitude
        /// </summary>
        [Column(TypeName = "decimal(7,4)")] 
        public decimal Lon { get; set; }
        #endregion
        /// <summary>
        /// Country Id (foreign key)
        /// </summary>
       public int CountryId { get; set; }

        #region Navigation Properties
        /// <summary>
        /// The country related to this city.
        /// </summary>
        public virtual Country Country { get; set; }
        #endregion



    }
}
