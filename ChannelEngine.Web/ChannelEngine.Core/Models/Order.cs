namespace ChannelEngine.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public List<OrderLine> Lines { get; set; }
    }

    public class OrderLine
    {
        public string Gtin { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        public string MerchantProductNo { get; set; }
    }
}
