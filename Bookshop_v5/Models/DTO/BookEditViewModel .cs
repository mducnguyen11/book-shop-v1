using Bookshop_v5.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Bookshop_v5.Models.DTO
{
    public class BookEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OldPrice { get; set; }
        public int Price { get; set; }
        public int GenreId { get; set; }
        public int AuthorId { get; set; }
        public string Image { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Author> Authors { get; set; }     
        
    }
}
