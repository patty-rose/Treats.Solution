using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models;

namespace Library
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
          services.AddControllersWithViews();

          services.AddDbContext<LibraryContext>(options => options
          .UseMySql(Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(Configuration["ConnectionStrings:DefaultConnection"])));

          services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddRoles<IdentityRole>()
          .AddEntityFrameworkStores<LibraryContext>()
          .AddDefaultTokenProviders();

          services.Configure<IdentityOptions>(options =>
          {
          options.Password.RequireDigit = false;
          options.Password.RequiredLength = 0;
          options.Password.RequireLowercase = false;
          options.Password.RequireNonAlphanumeric = false;
          options.Password.RequireUppercase = false;
          options.Password.RequiredUniqueChars = 0;
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseStaticFiles();

      app.Run(async (context) =>
      {
        await context.Response.WriteAsync("awaiting..");
      });
        }
    }
}
