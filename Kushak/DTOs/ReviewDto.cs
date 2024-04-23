using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kushak.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }

        [Range(1, 5)]
        [Required]
        public byte StarsCount { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Comment { get; set; }

        public DateTime? DateOfCreate { get; set; }

        public int ProductId { get; set; }
        public ProductDto ProductDto { get; set; }

        public string BuyerId { get; set; }
        public UserDto BuyerDto { get; set; }
    }
}