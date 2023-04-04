namespace Bookshop_v5.Models.Domain
{
    public class Order
    {
        public int Id { get; set; }

        public double TotalPrice { get; set; }

        public ICollection<OrderItem> Items { get; set; }   

        public string OrderStatus { get; set; }


		public string UserId { get; set; }

        public User User { get; set; }

		public DateTime DateCreated { get; set; }
    }
}
