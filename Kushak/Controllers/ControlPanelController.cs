using Kushak.DTOs;
using Kushak.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;

namespace Kushak.Controllers
{
    [Authorize(Roles = RoleName.OwnersAndAdmins)]
    public class ControlPanelController : ApiController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // get site statistics
        [HttpGet]
        [Route("api/Statistics")]
        public IHttpActionResult Statistics()
        {
            var lastSignedUsers = _context.Users.OrderByDescending(u => u.JoinDate).Take(4);
            var topSellingProducts = _context.Products.OrderByDescending(p => p.CountOfSale).Take(3);

            var dto = new StatisticsDto
            {
                ProductsCount = _context.Products.Count(),
                OrdersCount = _context.Orders.Count(),
                ReviewsCount = _context.Reviews.Count(),
                UsersCount = _context.Users.Count(),
                LastUsers = lastSignedUsers.Select(u => new UserDto
                {
                    Id = u.Id,
                    JoinDate = u.JoinDate,
                    FullName = u.FullName,
                    Email = u.Email,
                    ProfileImageSrc = u.ProfileImageSrc,
                }),

                TopSellingProducts = topSellingProducts.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageSrc = p.ImageSrc,
                    Price = p.Price,
                    Description = p.Description,
                    AvailableToSale = p.AvailableToSale,
                    CountOfSale = p.CountOfSale,
                    UnitsInStore = p.UnitsInStore,
                })
            };

            return Ok(dto);
        }

        [HttpGet]
        [Route("api/GetSiteSettings")]
        public IHttpActionResult GetSiteSettings()
        {
            var dto = new SiteSettingsDto
            {
                CurrencySign = ConfigurationManager.AppSettings["currencySign"].ToString(),
                ShippingFee = decimal.Parse(ConfigurationManager.AppSettings["shippingFee"]),
                CashOnDelivaryFee = decimal.Parse(ConfigurationManager.AppSettings["cashOnDelivaryFee"])
            };

            return Ok(dto);
        }

        [HttpPost]
        [Route("api/UpdateSiteSettings")]
        public IHttpActionResult UpdateSiteSettings(SiteSettingsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Configuration configFlie = WebConfigurationManager.OpenWebConfiguration("~");
            AppSettingsSection objAppSettings = (AppSettingsSection)configFlie.GetSection("appSettings");

            if (objAppSettings != null)
            {
                objAppSettings.Settings["currencySign"].Value = dto.CurrencySign;
                objAppSettings.Settings["shippingFee"].Value = dto.ShippingFee.ToString();
                objAppSettings.Settings["cashOnDelivaryFee"].Value = dto.CashOnDelivaryFee.ToString();

                configFlie.Save();
            }


            return Ok(dto);
        }

    }
}
