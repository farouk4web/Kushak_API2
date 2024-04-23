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
    public class ReviewsController : ApiController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // id of product not review
        public IHttpActionResult Getall4Product(int id)
        {
            var allReviewsinDb = _context.Reviews
                                                .Include(r => r.Buyer)
                                                .Where(r => r.ProductId == id)
                                                .ToList();

            var reviewsDto = allReviewsinDb.Select(r => new ReviewDto
            {
                Id = r.Id,
                Comment = r.Comment,
                DateOfCreate = r.DateOfCreate,
                StarsCount = r.StarsCount,
                ProductId = r.ProductId,
                BuyerId = r.BuyerId,

                BuyerDto = new UserDto
                {
                    Id = r.Buyer.Id,
                    FullName = r.Buyer.Email,
                    Email = r.Buyer.Email,
                    JoinDate = r.Buyer.JoinDate,
                    ProfileImageSrc = r.Buyer.ProfileImageSrc
                }
            });

            return Ok(reviewsDto);
        }


        [HttpPost]
        public IHttpActionResult AddNewReview(ReviewDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // check if thir is a product with this id or NOT
            var productInDb = _context.Products
                                        .Include(p => p.Reviews)
                                        .SingleOrDefault(p => p.Id == dto.ProductId);

            if (productInDb == null)
                return BadRequest();

            var currentUserId = User.Identity.GetUserId();

            //// check if user pay this product or not
            var userCartItems = _context.CartItems
                                            .Where(i => i.ProductId == dto.ProductId
                                                            && i.ShoppingCart.BuyerId == currentUserId
                                                            && i.ShoppingCart.OrderId != null)
                                            .ToList();

            if (userCartItems.Count() == 0)
                return BadRequest();

            // check if user add review to this product before ------> just one review
            var reviewInDb = _context.Reviews.SingleOrDefault(m =>
                                m.BuyerId == currentUserId && m.ProductId == dto.ProductId);

            if (reviewInDb != null)
                return BadRequest();

            //add new review
            Review review = new Review()
            {
                ProductId = dto.ProductId,
                Comment = dto.Comment,
                StarsCount = dto.StarsCount,

                DateOfCreate = DateTime.UtcNow,
                BuyerId = currentUserId
            };
            _context.Reviews.Add(review);
            _context.SaveChanges();

            // sign average of product stars Count 
            if (productInDb.Reviews.Count() != 0)
                productInDb.StarsCount = productInDb.Reviews.Average(r => r.StarsCount);

            _context.SaveChanges();


            // populate data to send it Again To client
            dto.Id = review.Id;
            dto.DateOfCreate = review.DateOfCreate;
            dto.BuyerId = review.BuyerId;

            var currentUser = _context.Users.Find(currentUserId);
            dto.BuyerDto = new UserDto
            {
                Id = currentUser.Id,
                Email = currentUser.Email,
                FullName = currentUser.FullName,
                ProfileImageSrc = currentUser.ProfileImageSrc,
                JoinDate = currentUser.JoinDate
            };

            return Ok(dto);
        }
    }
}
