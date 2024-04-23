using Kushak.DTOs;
using Kushak.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kushak.Controllers
{
    [Authorize]
    public class CheckoutController : ApiController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();


        [HttpPost]
        public IHttpActionResult Checkout(CheckoutDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var orderInDb = _context.Orders
                                        .Include(o => o.ShoppingCart.CartItems.Select(c => c.Product))
                                        .SingleOrDefault(o => o.Id == dto.OrderId);

            if (orderInDb == null || orderInDb.PaymentMethodId != null)
                return NotFound();

            // now pay using choosed payment method
            var operationResult = false;

            if (dto.PaymentMethodId == PaymentMethodsIDs.CashOnDelivaryId)
            {
                operationResult = true;
            }

            else if (dto.PaymentMethodId == PaymentMethodsIDs.PaypalId)
            {
                // Paypal Code

                operationResult = true;
            }

            else if (dto.PaymentMethodId == PaymentMethodsIDs.VisaCardId)
            {
                // visacard Code

                operationResult = true;
            }

            else if (dto.PaymentMethodId == PaymentMethodsIDs.VodafoneCashId)
            {
                // vodafone cash api Code here;

                operationResult = true;
            }

            else
                return BadRequest();


            if (operationResult == true)
            {
                // update order and add payment method fee
                //orderInDb.PaymentMethodId = dto.PaymentMethodId;
                orderInDb.PaymentMethodId = PaymentMethodsIDs.CashOnDelivaryId;

                orderInDb.PaymentMethodFee = orderInDb.PaymentMethodId == PaymentMethodsIDs.CashOnDelivaryId ? decimal.Parse(ConfigurationManager.AppSettings["cashOnDelivaryFee"].ToString()) : 0;
                orderInDb.GrandTotal += orderInDb.PaymentMethodFee;

                // now remove the quantity from every product in unit in store field
                orderInDb.ShoppingCart.CartItems.ForEach(item => item.Product.UnitsInStore = item.Product.UnitsInStore - item.Quantity);
                orderInDb.ShoppingCart.CartItems.ForEach(item => item.Product.CountOfSale = item.Product.CountOfSale + item.Quantity);
                _context.SaveChanges();
            }

            return Ok();
        }
    }
}
