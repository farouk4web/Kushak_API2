using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kushak.DTOs
{
    public class CheckoutDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }
    }
}