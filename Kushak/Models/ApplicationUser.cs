using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Kushak.Models
{
    // You can add profile data for the user by
    // adding more properties to your ApplicationUser class,
    // please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationUser : IdentityUser
    {
        //custom props for user
        [Required]
        [StringLength(60, MinimumLength = 4)]
        [RegularExpression("^[a-zA-Zء-ي ]*$")]
        public string FullName { get; set; }

        public string ProfileImageSrc { get; set; }

        public DateTime? JoinDate { get; set; }



        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public ICollection<Order> Orders { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match 
            // the one defined in CookieAuthenticationOptions.AuthenticationType

            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}