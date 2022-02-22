using ApiEcommerce.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Interfaces
{
    public interface IUserService
    {
        string GetCurrentUserId();
        Task<IList<string>> GetUserRolesAsync(User user);
        Task<bool> IsUserInRole(int userId, string roleName);
        bool IsUserInRole(ClaimsPrincipal user, string roleName);
        Task<User> GetByPrincipal(ClaimsPrincipal principal);

        Task<User> GetCurrentUserAsync();

        Task<User> GetByIdAsync(long id);
        Task<User> GetByUserNameAsync(string username);
        bool IsCurrentUserLoggedIn();
        bool IsUserLoggedIn(ClaimsPrincipal user);
        Task<bool> IsUserInRole(User user, string roleName);
        bool IsUserLoggedIn(IIdentity user);
        Task<IdentityResult> Create(User user, string password);
        

        Task<IdentityResult> Create(string userName,
            string firstName, string lastName,
            string email, string password);

        Task<IdentityResult> Delete(User user);
        Task<IdentityResult> AddToRoleAsync(User user, string roleName);

        Task<bool> IsAdmin();

        Task<bool> IsUserInRoleAsync(User user, string roleName);
    }
}
