using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReFreshMVC.Data;
using ReFreshMVC.Models.Interfaces;
using ReFreshMVC.Models.Services;
using Microsoft.AspNetCore.Identity;
using ReFreshMVC.Models;
using ReFreshMVC.Models.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;

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
            services.AddScoped<ICartManager, CartManagementService>();
            //services.AddScoped<IAuthorizeNetManager, AuthorizeNetService>();


            services.AddAuthentication()
            .AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Configuration.GetConnectionString("Authentication:Microsoft:ApplicationId");
                microsoftOptions.ClientSecret = Configuration.GetConnectionString("Authentication:Microsoft:Password");
            })
            .AddFacebook(facebookOptions => 
            {
                facebookOptions.ClientId = Configuration.GetConnectionString("Authentication:Facebook:ApplicationId");
                facebookOptions.ClientSecret = Configuration.GetConnectionString("Authentication:Facebook:Password");
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Carnivore", policy => policy.Requirements.Add(new DietRestriction()));
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole(AppRoles.Admin));
            });

            services.AddScoped<IAuthorizationHandler, DietRestriction>();
            services.AddScoped<IEmailSender, EmailSender>();

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
