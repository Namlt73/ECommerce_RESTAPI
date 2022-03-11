using ApiEcommerce.Data;
using ApiEcommerce.Entities;
using ApiEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ApiEcommerce.Helper
{
    public static class AuthExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>();

            services.AddIdentity<User, Role>(options =>
             {
                 options.User.RequireUniqueEmail = true;

                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequireDigit = false;
                 options.Password.RequireUppercase = false;
             })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // --------> Add Jwt Authentication <-----------
            var issuer = configuration["Security:Jwt:JwtIssuer"];
            var audience = configuration.GetSection("Security:Jwt:JwtIssuer").Value;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWT_SUPER_SECRET")),
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });
        }

        public static void AddAppAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationService configurationService =
                services.BuildServiceProvider().GetRequiredService<IConfigurationService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(configurationService.GetManageProductPolicyName(), policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(
                        new ResourceAuthorizationHandler.AllowedToManageProductRequirement(configurationService
                            .GetWhoIsAllowedToManageProducts()));
                });


                options.AddPolicy(configurationService.GetCreateCommentPolicyName(), policy =>
                {
                    policy.Requirements.Add(
                        new ResourceAuthorizationHandler.AllowedToCreateCommentRequirement(configurationService
                            .GetWhoIsAllowedToCreateComments()));
                });


                options.AddPolicy(configurationService.GetUpdateCommentPolicyName(), policy =>
                {
                    policy.Requirements.Add(
                        new ResourceAuthorizationHandler.AllowedToUpdateCommentRequirement(configurationService
                            .GetWhoIsAllowedToUpdateComments()));
                });


                options.AddPolicy(configurationService.GetDeleteCommentPolicyName(), policy =>
                {
                    policy.Requirements.Add(
                        new ResourceAuthorizationHandler.AllowedToDeleteCommentRequirement(configurationService
                            .GetWhoIsAllowedToDeleteComments()));
                });
            });

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }


        public static void AddIdentityAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            
            services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/users/login";
                    options.LogoutPath = "/logout";
                });
        }

    }
}
