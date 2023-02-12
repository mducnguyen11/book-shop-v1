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

       // public DbSet<Genre> Genre { get; set; }

    }
}
