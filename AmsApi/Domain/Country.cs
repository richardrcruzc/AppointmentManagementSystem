using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Domain
{
    public class Country:BaseEntity    
    {
        #region Constructor
        public Country()
        {
        }
        #endregion
        #region Properties
        /// <summary>
        /// Country name (in UTF8 format)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Country code (in ISO 3166-1 ALPHA-2 format)
        /// </summary>
        [JsonPropertyName("iso2")]
        public string ISO2 { get; set; }
        /// <summary>
        /// Country code (in ISO 3166-1 ALPHA-3 format)
        /// </summary>
        [JsonPropertyName("iso3")]
        public string ISO3 { get; set; }
        #endregion

        #region Navigation Properties
        /// <summary>
        /// A list containing all the cities related to this country.
        /// </summary>
        public virtual List<City> Cities { get; set; }
        #endregion

        
    }
}

