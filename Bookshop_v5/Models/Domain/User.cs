    using Microsoft.AspNetCore.Identity;
    using System;

    namespace Bookshop_v5.Models.Domain
    {
        public class User : IdentityUser
        {
            public string Name { get; set; }

            public string Gender { get; set; }

            public string Address { get; set; }

            public DateTime Birthday { get; set; }

            public int CartId { get; set; }
            public Cart Cart { get; set; }

            public ICollection<Order> Orders { get; set; }
            public User()
            {
            }

        }
    }
