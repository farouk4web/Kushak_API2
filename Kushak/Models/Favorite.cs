using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kushak.Models
{
    public class Favorite
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}