using Kushak.DTOs;
using Kushak.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kushak.Controllers
{
    [Authorize(Roles = RoleName.OwnersAndAdminsAndSellers)]
    public class CategoriesController : ApiController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Index(int pageNumber = 1)
        {
            int elementInPage = 10;
            var groupOfCategories = _context.Categories
                                                .OrderBy(p => p.Id)
                                                .Skip((pageNumber - 1) * elementInPage)
                                                .Take(elementInPage)
                                                .ToList();

            var categoriesDto = groupOfCategories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                SellerId = c.SellerId
            });

            return Ok(categoriesDto);
        }

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Details(int id)
        {
            var categoryInDb = _context.Categories
                                            .Include(c => c.Seller)
                                            .SingleOrDefault(p => p.Id == id);
            if (categoryInDb == null)
                return NotFound();

            var categoryDto = new CategoryDto
            {
                Id = categoryInDb.Id,
                Name = categoryInDb.Name,
                Description = categoryInDb.Description,
                SellerId = categoryInDb.SellerId,
                SellerDto = new UserDto
                {
                    Id = categoryInDb.Seller.FullName,
                    FullName = categoryInDb.Seller.FullName,
                    Email = categoryInDb.Seller.Email,
                    JoinDate = categoryInDb.Seller.JoinDate,
                    ProfileImageSrc = categoryInDb.Seller.ProfileImageSrc
                }
            };

            return Ok(categoryDto);
        }

        [HttpPost]
        public IHttpActionResult New(CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Category newCategory = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                SellerId = User.Identity.GetUserId()
            };
            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            categoryDto.Id = newCategory.Id;
            categoryDto.SellerId = newCategory.SellerId;

            return Ok(categoryDto);
        }

        [HttpPut]
        public IHttpActionResult Update(int id, CategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryInDb = _context.Categories.SingleOrDefault(p => p.Id == id);
            if (categoryInDb == null)
                return NotFound();

            // new data
            categoryInDb.Name = dto.Name;
            categoryInDb.Description = dto.Description;
            categoryInDb.SellerId = User.Identity.GetUserId();
            _context.SaveChanges();

            dto.Id = categoryInDb.Id;
            dto.SellerId = categoryInDb.SellerId;

            return Ok(dto);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var categoryInDb = _context.Categories
                                            .Include(c => c.Products)
                                            .SingleOrDefault(p => p.Id == id);

            if (categoryInDb == null)
                return NotFound();

            else if (categoryInDb.Products.Count() > 0)
                return BadRequest();

            _context.Categories.Remove(categoryInDb);
            _context.SaveChanges();
            return Ok();
        }
    }
}
