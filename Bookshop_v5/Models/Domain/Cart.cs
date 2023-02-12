namespace Bookshop_v5.Models.Domain
{
    public class Cart
    {
        public int Id { get; set; }

        public double TotalPrice { get; set; }  

        public ICollection<CartItem> Items { get; set;}

    }
}
