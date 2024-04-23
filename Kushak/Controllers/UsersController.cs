using Kushak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Kushak.DTOs;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Kushak.Controllers
{
    [Authorize(Roles = RoleName.OwnersAndAdminsAndSellersAndShippingStaff)]
    public class UsersController : ApiController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        [HttpGet]
        public IHttpActionResult Index(string filter = "all", int pageNumber = 1)
        {
            int elementInPage = 10;
            var groupOfUsers = new List<ApplicationUser>();

            switch (filter)
            {
                case "owners":
                    groupOfUsers = GetUsersInRole(RoleName.Owners);
                    break;

                case "admins":
                    groupOfUsers = GetUsersInRole(RoleName.Admins);
                    break;

                case "sellers":
                    groupOfUsers = GetUsersInRole(RoleName.Sellers);
                    break;

                case "shippingStaff":
                    groupOfUsers = GetUsersInRole(RoleName.ShippingStaff);
                    break;

                default:
                    groupOfUsers = _context.Users.ToList();
                    break;
            }

            // create pagenation
            groupOfUsers = groupOfUsers
                                     .OrderBy(u => u.JoinDate)
                                     .Skip((pageNumber - 1) * elementInPage)
                                     .Take(elementInPage)
                                     .ToList();

            var usersDto = groupOfUsers.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                JoinDate = u.JoinDate,
                ProfileImageSrc = u.ProfileImageSrc
            });

            return Ok(usersDto);
        }

        private List<ApplicationUser> GetUsersInRole(string roleName)
        {
            var role = _context.Roles
                                .Include(r => r.Users)
                                .SingleOrDefault(r => r.Name == roleName);

            var usersInRole = new List<ApplicationUser>();

            if (role != null)
            {
                foreach (var userRole in role.Users)
                {
                    var userInDb = _context.Users.Find(userRole.UserId);
                    usersInRole.Add(userInDb);
                }
            }

            return usersInRole;
        }

        [HttpGet]
        public IHttpActionResult UserAccount(string id)
        {
            var userInDb = _context.Users.SingleOrDefault(u => u.Id == id);

            if (userInDb == null)
                return NotFound();

            var userDto = new UserDto
            {
                Id = userInDb.Id,
                FullName = userInDb.FullName,
                Email = userInDb.Email,
                JoinDate = userInDb.JoinDate,
                ProfileImageSrc = userInDb.ProfileImageSrc
            };

            return Ok(userDto);
        }


        [HttpPost]
        [Route("Api/Users/AddToRole")]
        [Authorize(Roles = RoleName.OwnersAndAdmins)]
        public IHttpActionResult AddUserToRole(UserRoleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

            var userInDb = _context.Users.Find(dto.UserId);
            if (userInDb == null)
                return BadRequest();

            if (dto.RoleName != RoleName.Owners)
            {
                _userManager.AddToRole(dto.UserId, dto.RoleName);
            }
            else if (dto.RoleName == RoleName.Owners && User.IsInRole(RoleName.Owners))
            {
                // just super admins can add users as super admins
                _userManager.AddToRole(dto.UserId, dto.RoleName);
            }

            return Ok();
        }


        [HttpPost]
        [Route("Api/Users/RemoveFromRole")]
        [Authorize(Roles = RoleName.OwnersAndAdmins)]
        public IHttpActionResult RemoveUserFromRole(UserRoleDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var userInDb = _context.Users.Find(dto.UserId);
            if (userInDb == null)
                return BadRequest();

            if (dto.RoleName != RoleName.Owners)
            {
                _userManager.RemoveFromRole(dto.UserId, dto.RoleName);
            }
            else if (dto.RoleName == RoleName.Owners && User.IsInRole(RoleName.Owners))
            {
                _userManager.RemoveFromRole(dto.UserId, dto.RoleName);
            }

            return Ok();
        }


        // use it when devloping site to populate roles into database 
        //[HttpPost]
        //[AllowAnonymous]
        //[Route("api/CreateMainRoles")]
        //public IHttpActionResult CreateMainRoles()
        //{
        //    string[] nameOfRoles = {
        //        RoleName.Owners,
        //        RoleName.Admins,
        //        RoleName.Sellers,
        //        RoleName.ShippingStaff
        //    };
        //    foreach (var roleName in nameOfRoles)
        //    {
        //        var role = new IdentityRole
        //        {
        //            Name = roleName
        //        };

        //        _context.Roles.Add(role);
        //        _context.SaveChanges();
        //    }

        //    return Ok();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("api/AddUserToOwners")]
        //public IHttpActionResult AddUserToOwners()
        //{
        //    var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        //    _userManager.AddToRole("f79e4da2-278f-439f-8b7f-6d1b9fc49c73", RoleName.Owners);
        //    return Ok();
        //}
    }
}
