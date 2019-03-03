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


            //services.AddDefaultIdentity<IdentityUser>()
            //        .AddDefaultUI(UIFramework.Bootstrap4)
            //        .AddEntityFrameworkStores<UserDbContext>();

            services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Configuration.GetConnectionString("Authentication:Microsoft:ApplicationId");
                microsoftOptions.ClientSecret = Configuration.GetConnectionString("Authentication:Microsoft:Password");
                microsoftOptions.SaveTokens = true;
                microsoftOptions.Events.OnCreatingTicket = ctx =>
                {
                    List<AuthenticationToken> tokens = ctx.Properties.GetTokens()
                    as List<AuthenticationToken>;
                    tokens.Add(new AuthenticationToken()
                    {
                        Name = "TicketCreated",
                        Value = DateTime.UtcNow.ToString()
                    });
                    ctx.Properties.StoreTokens(tokens);
                    return Task.CompletedTask;
                };
            });

            //services.Configure(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
            //        .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            //services.Configure(AzureADDefaults.OpenIdScheme, options =>
            //{
            //    options.Authority = options.Authority + "/v2.0/";         // Azure AD v2.0

            //    options.TokenValidationParameters.ValidateIssuer = false; // accept several tenants (here simplified)
            //});

            //services.AddMvc(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //                    .RequireAuthenticatedUser()
            //                    .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //})
            //.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        


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
