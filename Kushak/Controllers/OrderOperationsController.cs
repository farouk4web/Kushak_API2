using Kushak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kushak.Controllers
{
    [Authorize(Roles = RoleName.OwnersAndAdminsAndSellersAndShippingStaff)]
    public class OrderOperationsController : ApiController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        [HttpPost]
        [Route("Api/ConfirmOrder/{id}")]
        [Authorize(Roles = RoleName.OwnersAndAdminsAndSellers)]
        public IHttpActionResult ConfirmOrder(int id)
        {
            var orderInDb = _context.Orders.Find(id);
            if (orderInDb == null)
                return NotFound();

            if (orderInDb.PaymentMethodId != null)
            {
                orderInDb.IsConfirmed = true;
                orderInDb.DateOfConfirmation = DateTime.UtcNow;

                _context.SaveChanges();

                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("Api/ShippingOrder/{id}")]
        [Authorize(Roles = RoleName.OwnersAndAdminsAndShippingStaff)]
        public IHttpActionResult ShippingOrder(int id)
        {
            var orderInDb = _context.Orders.Find(id);
            if (orderInDb == null)
                return NotFound();

            if (orderInDb.IsConfirmed == true)
            {
                orderInDb.IsShipping = true;
                orderInDb.DateOfShipping = DateTime.UtcNow;

                _context.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("Api/DeliveredOrder/{id}")]
        [Authorize(Roles = RoleName.OwnersAndAdminsAndShippingStaff)]
        public IHttpActionResult DeliveredOrder(int id)
        {
            var orderInDb = _context.Orders.Find(id);
            if (orderInDb == null)
                return NotFound();

            if (orderInDb.IsShipping == true)
            {
                orderInDb.IsDelivered = true;
                orderInDb.DateOfDelivery = DateTime.UtcNow;

                _context.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }
    }
}
