using ApiEcommerce.Entities;
using ApiEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Helper
{
    public class ResourceAuthorizationHandler : AuthorizationHandler<
        ResourceAuthorizationHandler.ResourceAuthorizationRequirement, object>
    {
        private readonly IUserService _userService;
        private readonly IConfigurationService _configurationService;

        public ResourceAuthorizationHandler(IConfigurationService configurationService, IUserService userService)
        {
            _configurationService = configurationService;
            _userService = userService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ResourceAuthorizationRequirement requirement,
            object resource)
        {


            var user = await _userService.GetByPrincipal(context.User);

            if (requirement.RoleBased)
            {
                string roleName = requirement.RoleName;

                if (requirement.GetType() == typeof(AllowedToManageProductRequirement) ||
                    requirement.GetType() == typeof(AllowedToCreateCommentRequirement))
                {
                    if (roleName == _configurationService.GetAdminRoleName())
                    {
                    }
                }
            }
            else
            {
                // For creation we have no Resource
                var policy = requirement.AuthorizationPolicy;
                if (policy == AuthorizationPolicy.ADMIN)
                {
                    if (context.User.Identity != null &&
                        context.User.Identity.Name != null &&
                        context.User.IsInRole(_configurationService.GetAdminRoleName()))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                }
                else if (policy == AuthorizationPolicy.ADMIN_AND_STAFF)
                {
                    bool isAdmin = await _userService.IsUserInRole(user, _configurationService.GetAdminRoleName());
                    if (isAdmin ||
                    (resource.GetType() == typeof(Comment) && ((Comment)resource).User.Id == user.Id)
                    )
                    {
                        context.Succeed(requirement);
                    }

                }
                else if (policy == AuthorizationPolicy.STAFF)
                {
                }
                else if (policy == AuthorizationPolicy.AUTHENTICATED_USER)
                {
                    if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
                    {
                        context.Succeed(requirement);
                    }
                }
            }
        }


        public abstract class ResourceAuthorizationRequirement : IAuthorizationRequirement
        {
            public string RoleName { get; set; }
            public bool RoleBased { get; set; }
            public AuthorizationPolicy AuthorizationPolicy { get; set; }

            protected ResourceAuthorizationRequirement(string roleName)
            {
                this.RoleName = roleName;
                this.RoleBased = true;
            }

            protected ResourceAuthorizationRequirement(AuthorizationPolicy authorizationPolicy)
            {
                this.AuthorizationPolicy = authorizationPolicy;
            }
        }



        public class AllowedToManageProductRequirement : ResourceAuthorizationRequirement
        {
            public AllowedToManageProductRequirement(string roleName) : base(roleName)
            {
            }
        }


        public class AllowedToCreateCommentRequirement : ResourceAuthorizationRequirement
        {
            public AllowedToCreateCommentRequirement(string roleName) : base(roleName)
            {
            }
        }

        public class AllowedToUpdateCommentRequirement : ResourceAuthorizationRequirement
        {
            public AllowedToUpdateCommentRequirement(AuthorizationPolicy authorizationPolicy) : base(
                authorizationPolicy)
            {
            }
        }

        public class AllowedToDeleteCommentRequirement : ResourceAuthorizationRequirement
        {
            public AllowedToDeleteCommentRequirement(AuthorizationPolicy authorizationPolicy) : base(
                authorizationPolicy)
            {
            }
        }
    }
}
