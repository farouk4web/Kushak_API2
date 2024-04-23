using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kushak.DTOs
{
    public class SiteSettingsDto
    {
        [Required]
        public string CurrencySign { get; set; }

        [Required]
        [Range(0, 500)]
        public decimal ShippingFee { get; set; }

        [Required]
        [Range(0, 500)]
        public decimal CashOnDelivaryFee { get; set; }
    }
}