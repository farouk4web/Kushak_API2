using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kushak.DTOs
{
    public class ProductDto
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

        [Required]
        public int CategoryId { get; set; }
        public CategoryDto CategoryDto { get; set; }

        public string SellerId { get; set; }
        public UserDto SellerDto { get; set; }


        public IEnumerable<ReviewDto> ReviewsDto { get; set; }

        public IEnumerable<FavoriteDto> FavoritesDto { get; set; }

        public IEnumerable<CartItemDto> CartItemsDto { get; set; }
    }
}