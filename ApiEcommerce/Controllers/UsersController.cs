using ApiEcommerce.Dtos.UserDtos;
using ApiEcommerce.Entities;
using ApiEcommerce.Helper;
using ApiEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _usersService;
        private readonly SignInManager<User> _signInManager;

        public UsersController(IUserService usersService,SignInManager<User> signInManager)
        {
            _usersService = usersService;
            _signInManager = signInManager;
        }

       


        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("There's something wrong!");
            var user = new User
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                UserName = registerRequest.Username,
                Email = registerRequest.Email
            };

            var result = await _usersService.Create(user, registerRequest.Password);

            if (result.Succeeded)
            {
                if (result.Succeeded)
                {
                    result = await _usersService.AddToRoleAsync(user, "ROLE_USER");
                    if (result.Succeeded)
                    {
                        return StatusCodeAndDtoWrapper.BuildSuccess("Registered successfully");
                    }
                    else
                    {
                        return StatusCodeAndDtoWrapper.BuildBadRequest(result.Errors);
                    }
                }
                else
                {
                    return StatusCodeAndDtoWrapper.BuildBadRequest(result.Errors);
                }
            }
            else
                return StatusCodeAndDtoWrapper.BuildBadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<object> Login([FromBody] LoginRequest loginRequest)
        {
            // Sign in the user, don't persis cookies, don't lockout on failure
            var result = await _signInManager.PasswordSignInAsync(loginRequest.UserName, loginRequest.Password,
                false, false);

            if (result.Succeeded)
            {
                User user = await _usersService.GetByUserNameAsync(loginRequest.UserName);
                return await GenerateJwtToken(user);
            }
            else
            {
                return StatusCodeAndDtoWrapper.BuildErrorResponse("Invalid credentials");
            }
        }

        private async Task<object> GenerateJwtToken(IdentityUser<long> user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWT_SUPER_SECRET"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var token = new JwtSecurityToken(
                issuer: "",
                claims: new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                },
                expires: DateTime.Now.AddDays(2),
                signingCredentials: creds);

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                Success = true,
                Data = new
                {
                    id = user.Id,
                    username = user.UserName,
                    token = tokenStr,
                    roles = (await _usersService.GetUserRolesAsync((User)user))
                },
            });
        }
    }
}
