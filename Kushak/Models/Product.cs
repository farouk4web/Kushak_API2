using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kushak.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [MinLength(250)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int UnitsInStore { get; set; }

        [Required]
        public bool AvailableToSale { get; set; }

        [Required]
        public bool ShowInSlider { get; set; }



        public string ImageSrc { get; set; }

        public int? CountOfSale { get; set; }

        public double? StarsCount { get; set; }


        // relatinships
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }


        public string SellerId { get; set; }
        public ApplicationUser Seller { get; set; }


        public ICollection<Review> Reviews { get; set; }

        public ICollection<Favorite> Favorites { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    }
}