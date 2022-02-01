using ItServiceApp.Models.Entities;
using ItServiceApp.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItServiceApp.Data
{
    public class MyContext : IdentityDbContext<ApplicationUser ,ApplicationRole, string>
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)//decimal değerleri sqlde bozulmasın diye ayarlayacağız. öteki türlü virgülden sonra 18 hane alıyordu.
        {
            base.OnModelCreating(builder);//bu silinmeyecek kalacak!!!

            //adı Fluent API
            
            builder.Entity<Subscription>().Property(x=> x.Amount).HasPrecision(9,2);//subscription tablosundali amount için düzenleme
            builder.Entity<Subscription>().Property(x => x.PaidAmount).HasPrecision(9, 2);
            builder.Entity<SubscriptionType>().Property(x=> x.Price).HasPrecision(9,2);
        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
    }
}