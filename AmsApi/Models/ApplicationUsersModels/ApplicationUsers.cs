

using AmsApi.Domain;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AmsApi.Models.ApplicationUsersModels
{
    public class ApplicationUser : IdentityUser
    {
         
        #region Lazy-Load Properties
        /// <summary>
        /// A list of all the refresh tokens issued for this users.
        /// </summary>
        public virtual List<Token> Tokens { get; set; }
        #endregion

    }
}
