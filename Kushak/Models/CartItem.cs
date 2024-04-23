using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kushak.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}