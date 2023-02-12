namespace Bookshop_v5.Models.Domain
{
    public class Order
    {
        public int Id { get; set; }

        public double TotalPrice { get; set; }

        public ICollection<OrderItem> Items { get; set; }   

        public string OrderStatus { get; set; } 
    }
}
