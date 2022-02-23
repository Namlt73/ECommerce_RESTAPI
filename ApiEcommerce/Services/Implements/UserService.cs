using ApiEcommerce.Entities;
using ApiEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly SignInManager<User> _signInManager;


        private readonly UserManager<User> _userManager;

        private readonly IConfigurationService _configurationService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<User> userManager,
            IConfigurationService configurationService,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _userManager = userManager;
            _configurationService = configurationService;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<List<User>> GetAll()
        {
            return await _userManager.Users.ToListAsync();
        }

        public string GetCurrentUserId()
        {
            string currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return currentUserId;
        }

        public long GetUserId(ClaimsPrincipal user)
        {
            return Convert.ToInt64(user.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        }

        public async Task<User> GetByIdAsync(long id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<User> GetByPrincipal(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        //Returns a list of the names of the roles of which the user is a member
        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }


        public async Task<User> GetByUserNameAsync(string username)
        {
            User user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));
            return user;
        }

        public async Task<bool> IsAdmin()
        {
            User user = await GetCurrentUserAsync();
            if (user == null)
                return false;
            return await IsUserInRoleAsync(user, _configurationService.GetAdminRoleName());
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<bool> IsUserInRole(int userId, string roleName)
        {
            return await IsUserInRoleAsync(await GetByIdAsync(userId), roleName);
        }

        public async Task<bool> IsUserInRole(User user, string roleName)
        {
            return await IsUserInRoleAsync(user, roleName);
        }

        public async Task<bool> IsUserInRole(string roleName)
        {
            var user = await GetCurrentUserAsync();
            return await IsUserInRoleAsync(user, roleName);
        }

        public bool IsUserInRole(ClaimsPrincipal user, string roleName)
        {
            return user.IsInRole(roleName);
        }

        public bool IsCurrentUserLoggedIn()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity.IsAuthenticated == true;
        }

        public bool IsUserLoggedIn(ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }

        public bool IsUserLoggedIn(IIdentity user)
        {
            return user != null && user.IsAuthenticated;
        }

        public async Task<IdentityResult> AddToRoleAsync(User user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }


        public async Task<IdentityResult> Create(User user, string password)
        {
            user.UserName = user.UserName;
            user.FirstName = user.FirstName;
            user.LastName = user.LastName;
            user.Email = user.Email;

            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> Create(string userName, string firstName, string lastName,
            string email,
            string password)
        {
            var user = new User
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
            };
            return await _userManager.CreateAsync(user, password);
        }


        public async Task<bool> CheckPasswordValid(string password, User user)
        {
            foreach (var userManagerPasswordValidator in _userManager.PasswordValidators)
            {
                IdentityResult res = await userManagerPasswordValidator.ValidateAsync(_userManager, user, password);
                if (res.Succeeded)
                    return true;
            }

            return false;
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword,
            string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }


        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> Delete(User user)
        {
            return await _userManager.DeleteAsync(user);
        }
    }
}
