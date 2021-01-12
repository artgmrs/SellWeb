using AspNetCoreHero.ToastNotification;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SellWeb.App.Config;
using SellWeb.Data.Context;

namespace SellWeb.App
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder();

            builder.AddUserSecrets<Startup>();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AdicionarIdentityConfig(Configuration);

            services.AddDbContext<SellWebDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(typeof(Startup));

            services.AdicionarMvcConfig();

            services.ResolverDependencias();

            services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.TopRight;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/erro/500");
                app.UseStatusCodePagesWithRedirects("/erro/{0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UsarGlobalizacaoConfig();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
