namespace Bookshop_v5.Models.Domain
{
    public class CartItem
    {
        public int Id { get; set; }

        public double UnitPrice { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }

        public int Quantity { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }

    }

}
