using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kushak.DTOs
{
    public class CartItemDto
    {
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int ProductId { get; set; }
        public ProductDto ProductDto { get; set; }

        public int ShoppingCartId { get; set; }
        public ShoppingCartDto ShoppingCartDto { get; set; }
    }
}