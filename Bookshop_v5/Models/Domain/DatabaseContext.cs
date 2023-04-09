using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Bookshop_v5.Models.Domain
{

    public class DatabaseContext : IdentityDbContext<User>

    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Genre> Genre { get; set; }

        public DbSet<Book> Book { get; set; }

        public DbSet<Author> Author { get; set; }


        public DbSet<Cart> Cart { get; set; }
        public DbSet<Order> Order { get; set; }

        public DbSet<CartItem> CartItem { get; set; }

        public DbSet<OrderItem> OrderItem { get; set; }






    }
}
