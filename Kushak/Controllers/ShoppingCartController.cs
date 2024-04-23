using Kushak.DTOs;
using Kushak.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace Kushak.Controllers
{
    [Authorize]
    public class ShoppingCartController : ApiController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: ShoppingCart
        public IHttpActionResult GetAll()
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUserShppingCart = _context.ShoppingCarts
                                                .Include(s => s.CartItems.Select(i => i.Product))
                                                .SingleOrDefault(s => s.BuyerId == currentUserId && s.OrderId == null);

            if (currentUserShppingCart == null)
            {
                currentUserShppingCart = new ShoppingCart
                {
                    BuyerId = currentUserId
                };

                _context.ShoppingCarts.Add(currentUserShppingCart);
                _context.SaveChanges();
            }


            var dto = new ShoppingCartDto
            {
                Id = currentUserShppingCart.Id,
                OrderId = null,
                BuyerId = currentUserShppingCart.BuyerId,

                CartItemsDto = currentUserShppingCart.CartItems.Select(i => new CartItemDto
                {
                    Id = i.Id,
                    Quantity = i.Quantity,
                    ShoppingCartId = i.ShoppingCartId,
                    ProductId = i.ProductId,
                    ProductDto = new ProductDto
                    {
                        Id = i.Product.Id,
                        Name = i.Product.Name,
                        Description = i.Product.Description,
                        ImageSrc = i.Product.ImageSrc,
                        Price = i.Product.Price,
                        AvailableToSale = i.Product.AvailableToSale,
                        UnitsInStore = i.Product.UnitsInStore,
                        StarsCount = i.Product.StarsCount,
                        CountOfSale = i.Product.CountOfSale,
                        ShowInSlider = i.Product.ShowInSlider,
                        CategoryId = i.Product.CategoryId
                    }
                })
            };

            return Ok(dto);
        }


        // ADD new item to the shopping cart
        [HttpPost]
        public IHttpActionResult AddNewItem(CartItemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = User.Identity.GetUserId();
            var productInDb = _context.Products.Find(dto.ProductId);

            if (productInDb != null)
            {
                // check if this product are available to sale or not
                if (productInDb.AvailableToSale == false || productInDb.UnitsInStore == 0)
                    return BadRequest();

                // get shopping cart with null order id and Create one if there is no cart available
                var currentUserShppingCart = _context.ShoppingCarts
                                                        .SingleOrDefault(s => s.BuyerId == currentUserId && s.OrderId == null);
                if (currentUserShppingCart == null)
                {
                    currentUserShppingCart = new ShoppingCart
                    {
                        BuyerId = currentUserId
                    };

                    _context.ShoppingCarts.Add(currentUserShppingCart);
                    _context.SaveChanges();
                }

                // check if this product added before, or not
                var itemInDb = _context.CartItems
                                    .SingleOrDefault(c => c.ProductId == productInDb.Id && c.ShoppingCartId == currentUserShppingCart.Id);

                if (itemInDb != null)
                {
                    itemInDb.Quantity = dto.Quantity;
                }
                else
                {
                    // create cart item to add it to user shopping cart
                    itemInDb = new CartItem
                    {
                        ProductId = dto.ProductId,
                        Quantity = dto.Quantity,
                        ShoppingCartId = currentUserShppingCart.Id
                    };

                    _context.CartItems.Add(itemInDb);
                }
                _context.SaveChanges();

                // populate dto by item data then send it
                dto.Id = itemInDb.Id;
                dto.Quantity = itemInDb.Quantity;
                dto.ShoppingCartId = itemInDb.ShoppingCartId;
                dto.ProductDto = new ProductDto
                {
                    Id = productInDb.Id,
                    Name = productInDb.Name,
                    Description = productInDb.Description,
                    ImageSrc = productInDb.ImageSrc,
                    Price = productInDb.Price,
                    CountOfSale = productInDb.CountOfSale,
                    AvailableToSale = productInDb.AvailableToSale,
                    UnitsInStore = productInDb.UnitsInStore,
                    StarsCount = productInDb.StarsCount,
                    ShowInSlider = productInDb.ShowInSlider,
                    CategoryId = productInDb.CategoryId,
                    SellerId = productInDb.SellerId
                };

                return Ok(dto);
            }

            return BadRequest();
        }


        // remove an item from shopping Cart
        [HttpDelete]
        public IHttpActionResult Remove(int id)
        {
            var item = _context.CartItems
                                    .Include(i => i.ShoppingCart)
                                    .SingleOrDefault(i => i.Id == id);

            if (item == null || item.ShoppingCart.BuyerId != User.Identity.GetUserId())
                return NotFound();

            _context.CartItems.Remove(item);
            _context.SaveChanges();

            return Ok();
        }
    }
}
