using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kushak.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        [RegularExpression("^[a-zA-Zء-ي ]*$")]
        public string Name { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 200)]
        public string Description { get; set; }

        public string SellerId { get; set; }
        public UserDto SellerDto { get; set; }

        public IEnumerable<ProductDto> ProductsDto { get; set; }
    }
}