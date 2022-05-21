using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecom.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ecom.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Writter_Book>().HasKey(wb => new 
            {
                wb.WritterId,
                wb.BookId
            });

            modelBuilder.Entity<Writter_Book>().HasOne(b => b.Book).WithMany(wb => wb.Writter_Books).HasForeignKey(b => b.BookId);

            modelBuilder.Entity<Writter_Book>().HasOne(b => b.Writter).WithMany(wb => wb.Writter_Books).HasForeignKey(b => b.WritterId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Writter> Writters { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Writter_Book> Writter_Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        //Order related tables
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
