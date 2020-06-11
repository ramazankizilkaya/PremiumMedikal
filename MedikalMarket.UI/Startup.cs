using AutoMapper;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Data.Mapper;
using MedikalMarket.UI.Data.Repositories;
using MedikalMarket.UI.Database.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MedikalMarket.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MedikalMarketContext>
               (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))/*.UseLazyLoadingProxies()*/);

            //services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<MedikalMarketContext>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddHttpClient();
            //WebEssentials.AspNetCore.OutputCaching
            services.AddOutputCaching();
            services.AddLocalization(opts => opts.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(x => x.ResourcesPath = "Resources")
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("tr"),
                    new CultureInfo("en"),
                    new CultureInfo("ru"),
                };

                opts.DefaultRequestCulture = new RequestCulture("tr");
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            });

            services.AddTransient<IAdProductRepository, AdProductRepository>();
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<IBrandRepository, BrandRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IContactUsRepository, ContactUsRepository>();
            services.AddTransient<IEmailNewsletterRepository, EmailNewsletterRepository>();
            services.AddTransient<IErrorLogRepository, ErrorLogRepository>();
            services.AddTransient<IFavoriteProductRepository, FavoriteProductRepository>();
            services.AddTransient<IMiddleCategoryRepository, MiddleCategoryRepository>();
            services.AddTransient<IMiniSliderRepository, MiniSliderRepository>();
            services.AddTransient<IProductPhotoRepository, ProductPhotoRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ISubCategoryRepository, SubCategoryRepository>();
            services.AddTransient<ITopCategoryRepository, TopCategoryRepository>();
            services.AddTransient<ISliderRepository, SliderRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddTransient<MedikalMarketContext>();
            services.AddAutoMapper(typeof(MedikalMarketMapper));
            //services.AddDistributedMemoryCache();
            //services.AddMemoryCache()
            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true; // make the session cookie Essential
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                //options.IdleTimeout = TimeSpan.FromSeconds(10);
                //options.Cookie.HttpOnly = true;
                //options.Cookie.Name = "premiummedikal.com";
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "premiummedikalCookie";
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/500");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            app.UseSession();
            app.UseRouting();
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UseOutputCaching();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
