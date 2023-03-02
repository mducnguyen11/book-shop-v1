using Microsoft.EntityFrameworkCore;

namespace Bookshop_v5.Models.Domain
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new DatabaseContext(
                serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                context.Database.EnsureCreated();

                // Tạo data seeding cho các class khác ở đây

                InitializeGenres(context);
            }
        }

        private static void InitializeGenres(DatabaseContext context)
        {
            if (!context.Genre.Any())
            {
                var genres = new Genre[]
                {
            new Genre{Name="Fantasy"},
            new Genre{Name="Science Fiction"},
            new Genre{Name="Mystery"},
            new Genre{Name="Romance"},
            new Genre{Name="Horror"},
            new Genre{Name="Thriller"},
            new Genre{Name="Historical Fiction"},
            new Genre{Name="Young Adult"},
            new Genre{Name="Children"},
                };

                context.Genre.AddRange(genres);
                context.SaveChanges();
            }
        }

    }
}
