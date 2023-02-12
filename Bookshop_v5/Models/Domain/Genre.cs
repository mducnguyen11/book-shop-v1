namespace Bookshop_v5.Models.Domain
{
    public class Genre
    {       
        public int Id { get; set; }

        public string Name { get; set; }    

        public ICollection<Book> Books { get; set; }
    }
}
