using Microsoft.AspNetCore.Identity;

namespace Bookshop_v5.Models.Domain
{
    public class User : IdentityUser
    {
        public string Nane { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; } 

        public DateTime Birthday { get; set; }

        public Cart Cart { get; set; }

        public ICollection<Order> Orders { get; set; }


    }
}
