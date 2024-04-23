using Kushak.DTOs;
using Kushak.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;

namespace Kushak.Controllers
{
    [Authorize]
    public class OrdersController : ApiController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();


        /// <summary>
        ///  if you want to get all Orders left it as default 
        ///  and write it if you want to get orders bby level
        ///  1 ==> not Paid
        ///  2 ==> paid
        ///  3 ==> confirmed
        ///  4 ==> shipping
        ///  5 ==> delivered
        /// </summary>
        /// <param name="level"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public IHttpActionResult Index(int level = 0, int pageNumber = 1)
        {
            int elementInPage = 10;
            var groupOfOrders = new List<Order>();
            switch (level)
            {
                case 1:
                    groupOfOrders = _context.Orders
                                    .Where(o => o.PaymentMethodId == null)
                                    .OrderByDescending(o => o.Id)
                                    .Skip((pageNumber - 1) * elementInPage)
                                    .Take(elementInPage)
                                    .ToList();
                    break;

                case 2:
                    groupOfOrders = _context.Orders
                                    .Where(o => o.PaymentMethodId != null && o.IsConfirmed == false)
                                    .OrderByDescending(o => o.Id)
                                    .Skip((pageNumber - 1) * elementInPage)
                                    .Take(elementInPage)
                                    .ToList();
                    break;

                case 3:
                    groupOfOrders = _context.Orders
                                    .Where(o => o.IsConfirmed && o.IsShipping == false)
                                    .OrderByDescending(o => o.Id)
                                    .Skip((pageNumber - 1) * elementInPage)
                                    .Take(elementInPage)
                                    .ToList();
                    break;

                case 4:
                    groupOfOrders = _context.Orders
                                    .Where(o => o.IsShipping && o.IsDelivered == false)
                                    .OrderByDescending(o => o.Id)
                                    .Skip((pageNumber - 1) * elementInPage)
                                    .Take(elementInPage)
                                    .ToList();
                    break;

                case 5:
                    groupOfOrders = _context.Orders
                                    .Where(o => o.IsDelivered)
                                    .OrderByDescending(o => o.Id)
                                    .Skip((pageNumber - 1) * elementInPage)
                                    .Take(elementInPage)
                                    .ToList();
                    break;

                default:
                    groupOfOrders = _context.Orders
                                            .OrderByDescending(o => o.Id)
                                            .Skip((pageNumber - 1) * elementInPage)
                                            .Take(elementInPage)
                                            .ToList();
                    break;
            }

            var ordersDto = groupOfOrders.Select(o => new OrderDto
            {
                Id = o.Id,
                BuyerId = o.BuyerId,
                ShoppingCartId = o.ShoppingCartId,

                FullName = o.FullName,
                Phone = o.Phone,

                City = o.City,
                Country = o.Country,
                Region = o.Region,
                Street = o.Street,
                MoreAboutAddress = o.MoreAboutAddress,

                PaymentMethodFee = o.PaymentMethodFee,
                PaymentMethodId = o.PaymentMethodId,

                Total = o.Total,
                ShippingFee = o.ShippingFee,
                GrandTotal = o.GrandTotal,

                DateOfCreate = o.DateOfCreate,
                DateOfConfirmation = o.DateOfConfirmation,
                DateOfDelivery = o.DateOfDelivery,
                DateOfShipping = o.DateOfShipping,

                IsConfirmed = o.IsConfirmed,
                IsDelivered = o.IsDelivered,
                IsShipping = o.IsShipping
            });

            return Ok(ordersDto);
        }

        [HttpGet]
        [Route("api/UserOrders")]
        public IHttpActionResult UserOrders()
        {
            var currentUserId = User.Identity.GetUserId();
            var userOrders = _context.Orders
                                    .Where(o => o.BuyerId == currentUserId)
                                    .OrderByDescending(o => o.Id)
                                    .ToList();

            var ordersDto = userOrders.Select(o => new OrderDto
            {
                Id = o.Id,
                BuyerId = o.BuyerId,
                ShoppingCartId = o.ShoppingCartId,

                FullName = o.FullName,
                Phone = o.Phone,

                City = o.City,
                Country = o.Country,
                Region = o.Region,
                Street = o.Street,
                MoreAboutAddress = o.MoreAboutAddress,

                PaymentMethodFee = o.PaymentMethodFee,
                PaymentMethodId = o.PaymentMethodId,

                Total = o.Total,
                ShippingFee = o.ShippingFee,
                GrandTotal = o.GrandTotal,

                DateOfCreate = o.DateOfCreate,
                DateOfConfirmation = o.DateOfConfirmation,
                DateOfDelivery = o.DateOfDelivery,
                DateOfShipping = o.DateOfShipping,

                IsConfirmed = o.IsConfirmed,
                IsDelivered = o.IsDelivered,
                IsShipping = o.IsShipping
            });

            return Ok(ordersDto);
        }

        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            var orderInDb = _context.Orders
                                    .Include(o => o.ShoppingCart.CartItems.Select(i => i.Product))
                                    .SingleOrDefault(o => o.Id == id);

            if (orderInDb == null || orderInDb.BuyerId != User.Identity.GetUserId())
                return NotFound();

            var dto = new OrderDto
            {
                Id = orderInDb.Id,

                FullName = orderInDb.FullName,
                Phone = orderInDb.Phone,

                City = orderInDb.City,
                Country = orderInDb.Country,
                Region = orderInDb.Region,
                Street = orderInDb.Street,
                MoreAboutAddress = orderInDb.MoreAboutAddress,

                PaymentMethodFee = orderInDb.PaymentMethodFee,
                PaymentMethodId = orderInDb.PaymentMethodId,

                Total = orderInDb.Total,
                ShippingFee = orderInDb.ShippingFee,
                GrandTotal = orderInDb.GrandTotal,

                DateOfCreate = orderInDb.DateOfCreate,
                DateOfConfirmation = orderInDb.DateOfConfirmation,
                DateOfDelivery = orderInDb.DateOfDelivery,
                DateOfShipping = orderInDb.DateOfShipping,

                IsConfirmed = orderInDb.IsConfirmed,
                IsDelivered = orderInDb.IsDelivered,
                IsShipping = orderInDb.IsShipping,

                BuyerId = orderInDb.BuyerId,
                ShoppingCartId = orderInDb.ShoppingCartId,

                ShoppingCart = new ShoppingCartDto
                {
                    Id = orderInDb.ShoppingCart.Id,
                    OrderId = orderInDb.ShoppingCart.OrderId,
                    BuyerId = orderInDb.ShoppingCart.BuyerId,

                    CartItemsDto = orderInDb.ShoppingCart.CartItems.Select(i => new CartItemDto
                    {
                        Id = i.Id,
                        Quantity = i.Quantity,
                        ProductDto = new ProductDto
                        {
                            Id = i.Product.Id,
                            Name = i.Product.Name,
                            ImageSrc = i.Product.ImageSrc,
                            Price = i.Product.Price,
                            Description = i.Product.Description,
                            AvailableToSale = i.Product.AvailableToSale,
                            UnitsInStore = i.Product.UnitsInStore,
                            CountOfSale = i.Product.CountOfSale,
                            StarsCount = i.Product.StarsCount,
                            ShowInSlider = i.Product.ShowInSlider,
                            CategoryId = i.Product.CategoryId,
                            SellerId = i.Product.SellerId
                        }
                    })
                }
            };

            return Ok(dto);
        }

        [HttpPost]
        public IHttpActionResult Create(OrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var shippingFee = Convert.ToDecimal(ConfigurationManager.AppSettings["shippingFee"].ToString());
            var currentUserId = User.Identity.GetUserId();

            var currentUserShoppingCart = _context.ShoppingCarts
                                                    .Include(m => m.CartItems.Select(s => s.Product))
                                                    .SingleOrDefault(i => i.BuyerId == currentUserId && i.OrderId == null);

            if (currentUserShoppingCart == null || currentUserShoppingCart.CartItems.Count() == 0)
                return BadRequest();

            foreach (var item in currentUserShoppingCart.CartItems)
            {
                if (item.Product.AvailableToSale == false || item.Product.UnitsInStore == 0)
                {
                    _context.CartItems.Remove(item);
                    _context.SaveChanges();
                }
            }

            if (currentUserShoppingCart.CartItems.Count() == 0)
                return BadRequest();

            // add new order to database
            decimal total = 0;
            foreach (var item in currentUserShoppingCart.CartItems)
            {
                total += item.Quantity * item.Product.Price;
            }

            Order newOrder = new Order
            {
                // main data 
                FullName = dto.FullName,
                Phone = dto.Phone,

                // address
                Country = dto.Country,
                Region = dto.Region,
                City = dto.City,
                Street = dto.Street,
                MoreAboutAddress = dto.MoreAboutAddress,

                // update for security
                Total = total,
                ShippingFee = shippingFee,
                GrandTotal = total + shippingFee, // without payment method fee                    

                // relate with current user
                BuyerId = currentUserId,
                ShoppingCartId = currentUserShoppingCart.Id,

                // add utc date Time 
                DateOfCreate = DateTime.UtcNow
            };

            // insert the order to database
            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            // relate shopingCart with this order
            currentUserShoppingCart.OrderId = newOrder.Id;
            _context.SaveChanges();

            // populate data and send it again
            dto.Id = newOrder.Id;
            dto.Total = newOrder.Total;
            dto.ShippingFee = newOrder.ShippingFee;
            dto.GrandTotal = newOrder.GrandTotal;
            dto.DateOfCreate = newOrder.DateOfCreate;
            dto.ShoppingCartId = newOrder.ShoppingCartId;
            dto.BuyerId = newOrder.BuyerId;

            return Ok(dto);
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var orderInDB = _context.Orders
                                .Include(o => o.ShoppingCart.CartItems)
                                .SingleOrDefault(o => o.Id == id);

            if (orderInDB == null)
                return NotFound();

            else if (orderInDB.IsConfirmed == true)
                return BadRequest();

            if (orderInDB.BuyerId == User.Identity.GetUserId() || User.IsInRole(RoleName.Owners) || User.IsInRole(RoleName.Admins))
            {
                _context.CartItems.RemoveRange(orderInDB.ShoppingCart.CartItems);
                _context.ShoppingCarts.Remove(orderInDB.ShoppingCart);
                _context.Orders.Remove(orderInDB);
                _context.SaveChanges();

                return Ok();
            }

            return BadRequest();
        }
    }
}
