namespace Bookshop_v5.Models.Domain
{
    public class Book
    {
        public string Image { get; set; }
        public int Id { get; set; }

        public string Name { get; set; }   

        public string Description { get; set; }

        public int GenreId { get; set; }

        public int AuthorId { get; set; }
        public Genre Genre { get; set; }

        public Author Author { get; set; }

        public int OldPrice { get; set; }

        public int Price { get; set; }

        public int SoldQuantity { get; set; }   
        public Book()   
        {
            SoldQuantity = 0;
        }

    }
}
