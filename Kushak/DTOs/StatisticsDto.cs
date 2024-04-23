using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kushak.DTOs
{
    public class StatisticsDto
    {
        public int ProductsCount { get; set; }
        public int OrdersCount { get; set; }
        public int ReviewsCount { get; set; }
        public int UsersCount { get; set; }

        public IEnumerable<ProductDto> TopSellingProducts { get; set; }

        public IEnumerable<UserDto> LastUsers { get; set; }
    }
}