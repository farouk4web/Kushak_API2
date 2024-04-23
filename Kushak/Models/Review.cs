using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kushak.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Range(1, 5)]
        [Required]
        public byte StarsCount { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Comment { get; set; }

        public DateTime? DateOfCreate { get; set; }



        // Relations 
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string BuyerId { get; set; }
        public ApplicationUser Buyer { get; set; }
    }
}