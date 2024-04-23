using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kushak.DTOs
{
    public class FavoriteDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public ProductDto ProductDto { get; set; }

        public string UserId { get; set; }
        public UserDto UserDto { get; set; }
    }
}