using Kushak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity;
using System.IO;
using Kushak.DTOs;

namespace Kushak.Controllers
{
    [Authorize(Roles = RoleName.OwnersAndAdminsAndSellers)]
    public class ProductsController : ApiController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Index(int categoryId = 0, int pageNumber = 1)
        {
            var groupOfProducts = new List<Product>();
            int elementInPage = 10;

            if (categoryId == 0)
            {
                groupOfProducts = _context.Products
                                                    .Include(p => p.Category)
                                                    .OrderBy(p => p.Id)
                                                    .Skip((pageNumber - 1) * elementInPage)
                                                    .Take(elementInPage)
                                                    .ToList();
            }
            else
            {
                groupOfProducts = _context.Products
                                                    .Include(p => p.Category)
                                                    .Where(p => p.CategoryId == categoryId)
                                                    .OrderBy(p => p.Id)
                                                    .Skip((pageNumber - 1) * elementInPage)
                                                    .Take(elementInPage)
                                                    .ToList();
            }

            var productsDto = groupOfProducts.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                ImageSrc = p.ImageSrc,
                Price = p.Price,
                Description = p.Description,
                AvailableToSale = p.AvailableToSale,
                CountOfSale = p.CountOfSale,
                UnitsInStore = p.UnitsInStore,
                ShowInSlider = p.ShowInSlider,
                StarsCount = p.StarsCount,
                SellerId = p.SellerId,
                CategoryId = p.CategoryId,
                CategoryDto = new CategoryDto
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name,
                    Description = p.Category.Description,
                    SellerId = p.Category.SellerId
                }
            });

            return Ok(productsDto);
        }

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Details(int id)
        {
            var productInDb = _context.Products
                                            .Include(p => p.Category)
                                            .Include(p => p.Seller)
                                            .Include(p => p.Reviews.Select(r => r.Buyer))
                                            .SingleOrDefault(p => p.Id == id);
            if (productInDb == null)
                return NotFound();

            var productDto = new ProductDto
            {
                Id = productInDb.Id,
                Name = productInDb.Name,
                ImageSrc = productInDb.ImageSrc,
                Price = productInDb.Price,
                Description = productInDb.Description,
                AvailableToSale = productInDb.AvailableToSale,
                CountOfSale = productInDb.CountOfSale,
                UnitsInStore = productInDb.UnitsInStore,
                ShowInSlider = productInDb.ShowInSlider,

                StarsCount = productInDb.StarsCount,
                CategoryId = productInDb.CategoryId,
                SellerId = productInDb.SellerId,

                SellerDto = new UserDto
                {
                    Id = productInDb.Seller.Id,
                    Email = productInDb.Seller.Email,
                    FullName = productInDb.Seller.FullName,
                    ProfileImageSrc = productInDb.Seller.ProfileImageSrc,
                    JoinDate = productInDb.Seller.JoinDate
                },
                CategoryDto = new CategoryDto
                {
                    Id = productInDb.Category.Id,
                    Name = productInDb.Category.Name,
                    Description = productInDb.Category.Description
                },
                ReviewsDto = productInDb.Reviews.Select(r => new ReviewDto
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
                        Email = r.Buyer.Email,
                        FullName = r.Buyer.FullName,
                        ProfileImageSrc = r.Buyer.ProfileImageSrc,
                        JoinDate = r.Buyer.JoinDate
                    }
                })
            };

            return Ok(productDto);
        }

        [HttpPost]
        public IHttpActionResult New(ProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Product newProduct = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                AvailableToSale = dto.AvailableToSale,
                CategoryId = dto.CategoryId,
                Price = dto.Price,
                ShowInSlider = dto.ShowInSlider,
                UnitsInStore = dto.UnitsInStore,


                StarsCount = 0,
                CountOfSale = 0,
                SellerId = User.Identity.GetUserId(),
                ImageSrc = "/Uploads/Products/product.png",
            };
            _context.Products.Add(newProduct);
            _context.SaveChanges();

            // populate compelete data in DTO
            dto.Id = newProduct.Id;
            dto.StarsCount = newProduct.StarsCount;
            dto.CountOfSale = newProduct.CountOfSale;
            dto.ImageSrc = newProduct.ImageSrc;
            dto.SellerId = newProduct.SellerId;

            var seller = _context.Users.Find(newProduct.SellerId);
            dto.SellerDto = new UserDto
            {
                Id = seller.Id,
                Email = seller.Email,
                FullName = seller.FullName,
                ProfileImageSrc = seller.ProfileImageSrc,
                JoinDate = seller.JoinDate
            };

            var category = _context.Categories.Find(newProduct.CategoryId);
            dto.CategoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return Ok(dto);
        }

        [HttpPost]
        [Route("Api/UploadProductPicture")]
        public IHttpActionResult UploadProductPicture(int productId)
        {
            var picture = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;

            if (picture != null || picture.ContentLength > 0)
            {
                // check if we support this extention  or not
                string[] supportedTypes = { ".png", ".jpg", ".jpeg" };
                FileInfo info = new FileInfo(picture.FileName);
                var isSupported = supportedTypes.Contains(info.Extension);

                if (isSupported == true)
                {
                    var productInDB = _context.Products.Find(productId);
                    if (productInDB == null)
                        return NotFound();

                    var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads/Products/"), productId + ".png");
                    picture.SaveAs(path);

                    productInDB.ImageSrc = "/Uploads/Products/" + productId + ".png";
                    _context.SaveChanges();

                    return Ok();
                }
            }

            return BadRequest();
        }

        [HttpPut]
        public IHttpActionResult Update(int id, ProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productInDb = _context.Products
                                        .Include(p => p.Category)
                                        .Include(p => p.Seller)
                                        .SingleOrDefault(p => p.Id == id);
            if (productInDb == null)
                return NotFound();

            if (productInDb.SellerId == User.Identity.GetUserId() || User.IsInRole(RoleName.Admins) || User.IsInRole(RoleName.Owners))
            {
                productInDb.Name = dto.Name;
                productInDb.Description = dto.Description;
                productInDb.Price = dto.Price;
                productInDb.UnitsInStore = dto.UnitsInStore;
                productInDb.AvailableToSale = dto.AvailableToSale;
                productInDb.ShowInSlider = dto.ShowInSlider;
                productInDb.CategoryId = dto.CategoryId;

                _context.SaveChanges();

                //populate data to send it to clint again
                dto.Id = productInDb.Id;
                dto.ImageSrc = productInDb.ImageSrc;
                dto.CountOfSale = productInDb.CountOfSale;
                dto.StarsCount = productInDb.StarsCount;

                dto.SellerDto = new UserDto
                {
                    Id = productInDb.Seller.Id,
                    Email = productInDb.Seller.Email,
                    FullName = productInDb.Seller.FullName,
                    ProfileImageSrc = productInDb.Seller.ProfileImageSrc,
                    JoinDate = productInDb.Seller.JoinDate
                };

                dto.CategoryDto = new CategoryDto
                {
                    Id = productInDb.Category.Id,
                    Name = productInDb.Category.Name,
                    Description = productInDb.Category.Description,
                    SellerId = productInDb.Category.SellerId
                };

                return Ok(dto);
            }

            return BadRequest();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var productInDb = _context.Products.SingleOrDefault(p => p.Id == id);
            if (productInDb == null)
                return NotFound();

            if (productInDb.SellerId == User.Identity.GetUserId() || User.IsInRole(RoleName.Admins) || User.IsInRole(RoleName.Owners))
            {
                _context.Products.Remove(productInDb);
                _context.SaveChanges();

                return Ok();
            }

            return BadRequest();
        }
    }
}