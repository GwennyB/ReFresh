using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReFreshMVC.Data;
using ReFreshMVC.Models.Interfaces;
using ReFreshMVC.Models.Services;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;
using ReFreshMVC.Models;
using ReFreshMVC.Models.Handler;
using Microsoft.AspNetCore.Authorization;

namespace ReFreshMVC
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            builder.AddUserSecrets<Startup>();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();

            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("UserConnection")));

            services.AddDbContext<ReFreshDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ProductionConnection")));
            services.AddScoped<IInventoryManager, InventoryManagementService>();
            services.AddScoped<ISearchBarManager, SearchBarManagementService>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Carnivore", policy => policy.Requirements.Add(new DietRestriction()));
            });

            services.AddScoped<IAuthorizationHandler, DietRestriction>();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            // error handling thanks to https://www.devtrends.co.uk/blog/handling-404-not-found-in-asp.net-core
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseMvc(route =>
            {
                route.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
