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

                // Tạo dữ liệu seeding cho các class
                if (!context.Genre.Any())
                {
                    var genres = new List<Genre>
                    {
                        new Genre { Name = "Fiction" },
                        new Genre { Name = "Non-fiction" },
                        new Genre { Name = "Science fiction" },
                        new Genre { Name = "Mystery" },
                        new Genre { Name = "Romance" }
                    };
                    context.Genre.AddRange(genres);
                    context.SaveChanges();
                }

                if (!context.Author.Any())
                {
                    var authors = new List<Author>
                    {
                        new Author { Name = "J.K. Rowling", BirthDay = new DateTime(1965, 7, 31) },
                        new Author { Name = "Dan Brown", BirthDay = new DateTime(1964, 6, 22) },
                        new Author { Name = "Stephen King", BirthDay = new DateTime(1947, 9, 21) },
                        new Author { Name = "Agatha Christie", BirthDay = new DateTime(1890, 9, 15) },
                        new Author { Name = "Nicholas Sparks", BirthDay = new DateTime(1965, 12, 31) }
                    };
                    context.Author.AddRange(authors);
                    context.SaveChanges();
                }

                if (!context.Book.Any())
                {
                    string url = "https://salt.tikicdn.com/cache/750x750/ts/product/97/61/91/a9b293f184d4dbbc2afc416b539f2bca.jpg.webp";
                    var books = new List<Book>
                    {
                        new Book { Image = url  ,OldPrice = 220 , Price = 200, Name = "Harry Potter and the Philosopher's Stone", Description = "The first book in the Harry Potter series", GenreId = 1, AuthorId = 1 },
                        new Book { Image = url  , OldPrice = 270 , Price = 250, Name = "The Da Vinci Code", Description = "A thriller novel by Dan Brown", GenreId = 4, AuthorId = 2 },
                        new Book { Image = url  , OldPrice = 200 , Price = 100, Name = "The Shining", Description = "A horror novel by Stephen King", GenreId = 1, AuthorId = 3 },
                        new Book {Image = url  , OldPrice = 170 , Price = 120, Name = "Murder on the Orient Express", Description = "A detective novel by Agatha Christie", GenreId = 4, AuthorId = 4 },
                        new Book {Image = url  , OldPrice = 320 , Price = 290,  Name = "The Notebook", Description = "A romantic novel by Nicholas Sparks", GenreId = 5, AuthorId = 5 }
                    };
                    context.Book.AddRange(books);
                    context.SaveChanges();
                }
            }
        }


    }
}
