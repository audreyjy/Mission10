using BookStoreProject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public IConfiguration Configuration { get; set; }
        public Startup (IConfiguration temp)
        {
            Configuration = temp; 
        }




        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(); // setup 

            services.AddDbContext<BookstoreContext>(options =>
            {
                options.UseSqlite(Configuration["ConnectionStrings:BookStoreDBConnection"]);
            });

            services.AddScoped<iBookStoreRepository, EFBookstoreProjectRepository>();

            services.AddRazorPages();

            services.AddDistributedMemoryCache();
            services.AddSession(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(); // setup - connects to wwwroot 
            app.UseSession(); // session capability
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "typepage",
                    pattern: "{bookCategory}/Page{pageNum}", 
                    defaults: new { Controller = "Home", action = "Index" }); 


                endpoints.MapControllerRoute(
                    name: "type",
                    pattern: "{projectType}",
                    defaults: new { Controller = "Home", action = "Index", pageNum = 1 });

                endpoints.MapControllerRoute(
                name: "Paging",
                pattern: "{pageNum}",
                defaults: new { Controller = "Home", Action = "Index ", pageNum = 1 });

                endpoints.MapDefaultControllerRoute(); //

                endpoints.MapRazorPages(); 

            });
        }
    }
}
