namespace Bookshop_v5.Models.Domain
{
    public class OrderItem
    {
        public int Id { get; set; }

        public Book Book { get; set; }  

        public int Quantity { get; set; }

        public double UnitPrice { get; set; }
    }
}
