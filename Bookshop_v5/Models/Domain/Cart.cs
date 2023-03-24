namespace Bookshop_v5.Models.Domain
{
    public class Cart
    {
        public int Id { get; set; }

        public double TotalPrice { get; set; }  

        public ICollection<CartItem> Items { get; set;}

        public Cart(){           
            TotalPrice = 0;               
        }

        public void AddItem(Book book, int quantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.Book.Id == book.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem { Book = book, Quantity = quantity });
            }

            TotalPrice += book.Price * quantity;
        }


    }
}
