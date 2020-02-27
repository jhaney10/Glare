using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Glare.Data;
using Glare.Services;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using Glare.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
namespace Glare
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
            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/Checkout","Checkout/{id?}");
                });
            services.AddAuthentication()
                .AddGoogle(options => {

                    options.ClientId = Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                });
                
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/AccessDenied");
            });
            services.AddDbContext<ProductContext>(options =>
                {
                options.UseSqlServer(Configuration.GetConnectionString("ProductContext"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    });
            });

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<ProductContext>()
                .AddDefaultTokenProviders();
            //Email Sender Services
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddAuthorization(options =>
            {
                //Claims Based Policy
                options.AddPolicy("CreateRolePolicy",
                    policy => policy.RequireClaim("Create Role")
                                    .RequireClaim("Delete Role"));
                //Role Based Policy
                options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Admin"));
            });
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
               // options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/_{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            //Sets the app currency to Naira
            var cultureInfo = new CultureInfo("yo-NG");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
          
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
