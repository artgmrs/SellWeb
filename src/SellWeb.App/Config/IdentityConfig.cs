using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SellWeb.App.Data;

namespace SellWeb.App.Config
{
    public static class IdentityConfig
    {
        public static IServiceCollection AdicionarIdentityConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseSqlServer(
                                configuration.GetConnectionString("DefaultConnection")));


            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //Google
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthSection = configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthSection["ClientId"];
                    options.ClientSecret = googleAuthSection["ClientSecret"];
                });

            return services;
        }
    }
}
