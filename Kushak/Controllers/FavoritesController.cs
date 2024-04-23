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

namespace Kushak.Controllers
{
    [Authorize]
    public class FavoritesController : ApiController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public IHttpActionResult GetAll()
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUserFavoriesItems = _context.Favorites
                                                        .Include(f => f.Product)
                                                        .Where(f => f.UserId == currentUserId)
                                                        .ToList();

            var favoritesDto = currentUserFavoriesItems.Select(c => new FavoriteDto
            {
                Id = c.Id,
                UserId = c.UserId,
                ProductId = c.ProductId,
                ProductDto = new ProductDto
                {
                    Id = c.Product.Id,
                    Name = c.Product.Name,
                    ImageSrc = c.Product.ImageSrc,
                    Price = c.Product.Price,
                    StarsCount = c.Product.StarsCount
                }
            });

            return Ok(favoritesDto);
        }

        [HttpPost]
        public IHttpActionResult AddToMyFavorite(FavoriteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = User.Identity.GetUserId();

            // check if user add this product to his favorites before or not
            var itemInDb = _context.Favorites.SingleOrDefault(i => i.ProductId == dto.ProductId && i.UserId == currentUserId);
            var productInDB = _context.Products.Find(dto.ProductId);

            if (itemInDb != null || productInDB == null)
                return BadRequest();

            Favorite item = new Favorite()
            {
                ProductId = dto.ProductId,
                UserId = currentUserId
            };

            _context.Favorites.Add(item);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteFromFavorite(int id)
        {
            var item = _context.Favorites.Find(id);
            if (item == null)
                return NotFound();

            if (item.UserId == User.Identity.GetUserId())
            {
                _context.Favorites.Remove(item);
                _context.SaveChanges();
            }

            return Ok();
        }
    }
}
