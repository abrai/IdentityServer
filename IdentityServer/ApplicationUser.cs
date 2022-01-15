using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        /// <summary>
        /// Gets or sets the DisplayName 
        /// </summary>
        [StringLength(512)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the ISPWDUser 
        /// </summary>
        public bool? ISPWDUser { get; set; }

        /// <summary>
        /// Gets or sets the gender 
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the ISPWDUser
        /// </summary>
        public int? SAPId { get; set; }

        /// <summary>
        /// Gets or sets the ISPWDUser
        /// </summary>
        public int? ManagerId { get; set; }

        /// <summary>
        /// Gets or sets the user create date 
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the successful log on count
        /// </summary>
        public int LogOnCount { get; set; }

        /// <summary>
        /// Gets or sets the LastLogin date
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

    }
}
