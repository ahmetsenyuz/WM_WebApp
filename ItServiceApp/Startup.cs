using ItServiceApp.Data;
using ItServiceApp.MapperProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.IO;
using ItServiceApp.Business.MapperProfiles;
using ItServiceApp.Business.Services.Email;
using ItServiceApp.Business.Services.Payment;
using ItServiceApp.Core.Identity;

namespace ItServiceApp
{
    //https://www.syncfusion.com/products/communitylicense
    //https://js.devexpress.com/Demos/WidgetsGallery/Demo/DataGrid/Overview/jQuery/Light/
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            //devextreme
            services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 5;

                //lockout setting 
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //atma s�resi
                options.Lockout.MaxFailedAccessAttempts = 3; //3ten fazla yanl�� girerse atar
                options.Lockout.AllowedForNewUsers = false; //kitlediyse kay�t olamas�n
                //User Setting
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
                //options.SignIn.RequireConfirmedEmail = true; //confirm edilmemi� e maillerin giri�ini engelliyor.
            }).AddEntityFrameworkStores<MyContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                //cookie settings
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            services.AddTransient<IEmailSender, EmailSender>();//loose coupling --yar�n bi g�n servis de�iirse git sa�dakini sil yeni servis sa�lay�c�n� yazars�n
            services.AddScoped<IPaymentService, IyzicoPaymentService>();//servis tan�mland�
            //services.AddScoped<IyzicoPaymentService>(); �sttekiyle ayn i�i yap�yor ancak yar�n bi g�n servis sa�layc� de�i�ir i�e gidip Payment kontrollerdaki 
            //yorum sATIRI �eklinde tan�mlanan yerler de de�i�ecekti.tasar�m desenleri ve mimarileri engin bulut
            services.AddAutoMapper(options =>
            {
                options.AddProfile<PaymentProfile>();
                //options.AddProfile(typeof(PaymentProfile)); ya b�yle ya da �stteki gibi ama ayn� bok
                options.AddProfile<EntityProfile>();
            });
            //ba�l� taplolardan i� i�e veri d�ng�s� olmamas� i�in yaz�yoruz. ��rne�in ki�i tablosu ve il tablosu gibi bire �ok ili�kili tablolarda
            //ki�iyi okuyor ilini okuyor ilden tekrar ki�iyi okuyor sonra ki�iden tekrar il vsvs.
            services.AddControllersWithViews().AddNewtonsoftJson(options=>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        //https://obfuscator.io/ javascript kodunu �ifrelemeye yar�yor.

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles(); //wwwroot klas�r�ndeki statik dosyalar� kullanabilmek i�in
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules")),
                RequestPath = new PathString("/vendor")
            });//kullanabilmek i�in node_modules klas�r�n� de wwwrot gibi static file oldu�unu tan�t�yoruz.
            
            app.UseRouting();
            app.UseAuthentication();//login logout kullanabilmek i�in
            app.UseAuthorization();//authorization attiribute kullanabilmek i�in

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"); 
                endpoints.MapAreaControllerRoute(
                    name: "admin",
                    areaName: "admin",
                    pattern: "admin/{controller=Manage}/{action=Index}/{id?}");
            });
        }
    }
}

//open project in file explorer, type cmd, npm, npm init, npm i ==> projedeki paketleri indirir(package.jsondaki), �te t�rl� ayn� ad�mlar ama her biri
//nde npm i paketin ad�
