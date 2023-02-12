namespace Bookshop_v5.Models.Domain
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }   

        public string Description { get; set; }

        public Genre Genre { get; set; }

        public Author Author { get; set; }
    }
}
