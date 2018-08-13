using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityClaims.Data;
using IdentityClaims.Models;
using IdentityClaims.Services;

namespace IdentityClaims
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<IdentityClaimsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultDBContext")));


            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.ReturnUrlParameter = "/Home/Index";
                options.SlidingExpiration = true;
            });


            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("User-SalesOrders-Manage", policy => { policy.RequireClaim("User", "User-SalesOrders-Manage"); });
                options.AddPolicy("User-SalesOrders-View", policy => { policy.RequireClaim("User", "User-SalesOrders-View"); });
                options.AddPolicy("User-SalesOrders-Delete", policy => { policy.RequireClaim("User", "User-SalesOrders-Delete"); });
                options.AddPolicy("User-PurchaseOrders-Manage", policy => { policy.RequireClaim("User", "User-PurchaseOrders-Manage"); });
                options.AddPolicy("User-PurchaseOrders-View", policy => { policy.RequireClaim("User", "User-PurchaseOrders-View"); });
                options.AddPolicy("User-PurchaseOrders-Delete", policy => { policy.RequireClaim("User", "User-PurchaseOrders-Delete"); });
                options.AddPolicy("User-CustomerPricingReport-View", policy => { policy.RequireClaim("User", "User-CustomerPricingReport-View"); });
                options.AddPolicy("MasterData-Customers-Manage", policy => { policy.RequireClaim("MasterData", "MasterData-Customers-Manage"); });
                options.AddPolicy("MasterData-Customers-Delete", policy => { policy.RequireClaim("MasterData", "MasterData-Customers-Delete"); });
                options.AddPolicy("MasterData-Customers-View", policy => { policy.RequireClaim("MasterData", "MasterData-Customers-View"); });
                options.AddPolicy("MasterData-Vendors-Manage", policy => { policy.RequireClaim("MasterData", "MasterData-Vendors-Manage"); });
                options.AddPolicy("MasterData-Vendors-Delete", policy => { policy.RequireClaim("MasterData", "MasterData-Vendors-Delete"); });
                options.AddPolicy("MasterData-Vendors-View", policy => { policy.RequireClaim("MasterData", "MasterData-Vendors-View"); });
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
