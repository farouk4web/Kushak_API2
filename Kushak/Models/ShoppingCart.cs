using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kushak.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public int? OrderId { get; set; }
        //public Order Order { get; set; }

        public string BuyerId { get; set; }
        public ApplicationUser Buyer { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    }
}