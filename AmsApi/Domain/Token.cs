using AmsApi.Models.ApplicationUsersModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Domain
{
    public class Token: BaseEntity
    {
        #region Constructor
        public Token()
        {
        }
        #endregion
        #region Properties
       
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string Value { get; set; }
        public int Type { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        #endregion
        #region Lazy-Load Properties
        /// <summary>
        /// The user related to this token
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        #endregion
    }
     
  }
