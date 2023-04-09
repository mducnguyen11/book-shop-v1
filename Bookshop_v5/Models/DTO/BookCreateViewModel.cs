using Bookshop_v5.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace Bookshop_v5.Models.DTO
{
    public class BookCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int GenreId { get; set; }

        [Required]
        public int AuthorId { get; set; }   

        [Required]
        [Range(0, int.MaxValue)]
        public int OldPrice { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        public string Image { get; set; }

        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Author> Authors { get; set; }
    }

}
