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
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
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


            services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Configuration.GetConnectionString("Authentication:Microsoft:ApplicationId");
                microsoftOptions.ClientSecret = Configuration.GetConnectionString("Authentication:Microsoft:Password");
                //microsoftOptions.SaveTokens = true;
                //microsoftOptions.Events.OnCreatingTicket = ctx =>
                //{
                //    List<AuthenticationToken> tokens = ctx.Properties.GetTokens()
                //    as List<AuthenticationToken>;
                //    tokens.Add(new AuthenticationToken()
                //    {
                //        Name = "TicketCreated",
                //        Value = DateTime.UtcNow.ToString()
                //    });
                //    ctx.Properties.StoreTokens(tokens);
                //    return Task.CompletedTask;
                //};
            });


        services.AddAuthorization(options =>
            {
                options.AddPolicy("Carnivore", policy => policy.Requirements.Add(new DietRestriction()));
            });

            services.AddScoped<IAuthorizationHandler, DietRestriction>();
            services.AddScoped<IEmailSender, MailManager>();

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
