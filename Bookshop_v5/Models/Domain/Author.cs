namespace Bookshop_v5.Models.Domain
{
    public class Author
    {       
        public int Id { get; set; }

        public string Name { get; set; }    

        public DateTime BirthDay { get; set; }

        public ICollection<Book> Books { get; set;}
    }
}
