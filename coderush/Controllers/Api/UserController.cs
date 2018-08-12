using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models;
using coderush.Models.AccountViewModels;
using coderush.Models.SyncfusionViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace coderush.Controllers.Api
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(ApplicationDbContext context,
                        UserManager<ApplicationUser> userManager,
                        RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult GetUser()
        {
            List<UserProfile> Items = new List<UserProfile>();
            Items = _context.UserProfile.ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetByApplicationUserId([FromRoute]string id)
        {
            UserProfile userProfile = _context.UserProfile.SingleOrDefault(x => x.ApplicationUserId.Equals(id));
            List<UserProfile> Items = new List<UserProfile>();
            if (userProfile != null)
            {
                Items.Add(userProfile);
            }
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Insert([FromBody]CrudViewModel<UserProfile> payload)
        {
            UserProfile register = payload.value;
            if (register.Password.Equals(register.ConfirmPassword))
            {
                ApplicationUser user = new ApplicationUser() { Email = register.Email, UserName = register.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    register.Password = user.PasswordHash;
                    register.ConfirmPassword = user.PasswordHash;
                    register.ApplicationUserId = user.Id;
                    _context.UserProfile.Add(register);
                    await _context.SaveChangesAsync();
                }
                
            }
            return Ok(register);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update([FromBody]CrudViewModel<UserProfile> payload)
        {
            UserProfile profile = payload.value;
            _context.UserProfile.Update(profile);
            await _context.SaveChangesAsync();
            return Ok(profile);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword([FromBody]CrudViewModel<UserProfile> payload)
        {
            UserProfile profile = payload.value;
            if (profile.Password.Equals(profile.ConfirmPassword))
            {
                var user = await _userManager.FindByIdAsync(profile.ApplicationUserId);
                var result = await _userManager.ChangePasswordAsync(user, profile.OldPassword, profile.Password);
            }
            profile = _context.UserProfile.SingleOrDefault(x => x.ApplicationUserId.Equals(profile.ApplicationUserId));
            return Ok(profile);
        }
        
        [HttpPost("[action]")]
        public IActionResult ChangeRole([FromBody]CrudViewModel<UserProfile> payload)
        {
            UserProfile profile = payload.value;
            return Ok(profile);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Remove([FromBody]CrudViewModel<UserProfile> payload)
        {
            var userProfile = _context.UserProfile.SingleOrDefault(x => x.UserProfileId.Equals((int)payload.key));
            if (userProfile != null)
            {
                var user = _context.Users.Where(x => x.Id.Equals(userProfile.ApplicationUserId)).FirstOrDefault();
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    _context.Remove(userProfile);
                    await _context.SaveChangesAsync();
                }
                
            }
            
            return Ok();

        }
        
        
    }
}