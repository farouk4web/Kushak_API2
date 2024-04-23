using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kushak.DTOs
{
    public class ShoppingCartDto
    {
        public int Id { get; set; }

        public int? OrderId { get; set; }
        //public Order Order { get; set; }

        public string BuyerId { get; set; }
        public UserDto BuyerDto { get; set; }

        public IEnumerable<CartItemDto> CartItemsDto { get; set; }
    }
}