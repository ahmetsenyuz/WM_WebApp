using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayoutKullanimi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection(); //https : güvenli sertifika ile çalýþmasý için
            app.UseStaticFiles(); //wwwroot klasöründeki statik dosyalara eriþmek için
            app.UseRouting(); //rooting mekanizmasý için

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
                //endpoints.MapGet("/detay", async context =>
                //{
                //    await context.Response.WriteAsync("Detay Sayfasý");
                //});
                endpoints.MapControllerRoute(
                    name : "default",
                    pattern : "{controller=Home}/{action=Index}/{id?}");
            }); //default routingin nasýl olacaðýný belirtmek için
        }
    }
}
