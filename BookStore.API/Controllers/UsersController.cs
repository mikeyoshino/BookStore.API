using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.API.Contracts;
using BookStore.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILoggerService _loggerSerivce;

        public UsersController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILoggerService loggerService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _loggerSerivce = loggerService;
        }

        /// <summary>
        /// User Login Endpoint
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                var username = userDTO.UserName;
                var password = userDTO.Password;
                _loggerSerivce.LogInfor($"{location}: Login Attepmted for user: {username}.");
                var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

                if (result.Succeeded)
                {
                    _loggerSerivce.LogInfor($"{location} {username}: Successfully Authenticated.");
                    var user = await _userManager.FindByNameAsync(username);
                    return Ok(user);
                }
                _loggerSerivce.LogInfor($"{location} {username}: Not Authenticated.");
                return Unauthorized(userDTO);
            }
            catch (Exception e)
            {
                return InternalError($"{location}{e.Message} - {e.InnerException}");
            }
            
        }

        private string GetControllerActionNames()
        {
            var controllerName = ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ControllerContext.ActionDescriptor.ActionName;

            return $"{controllerName} - {actionName}";
        }

        private ObjectResult InternalError(string message)
        {
            _loggerSerivce.LogError(message);
            return StatusCode(500, "Somethings went wrong, contract admins asap.");
        }


    }
}
