using Martiello.Domain.Enums;
using Martiello.Domain.Extension;
using MongoDB.Bson.Serialization.Attributes;

namespace Martiello.Domain.Entity
{
    public class Order : BaseModel
    {

        [BsonElement("number")]
        public int Number { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("customer")]
        [BsonIgnoreIfNull]
        public Customer Customer { get; set; }

        [BsonElement("products")]
        public IEnumerable<Product> Products { get; set; } = new List<Product>();

        [BsonElement("total_price")]
        public decimal TotalPrice => Products.Sum(p => p.Price);

        public Order(Customer customer, IEnumerable<Product> products)
        {
            Customer = customer;
            Status = OrderStatus.Pending.GetDescription();
            Products = products;
        }
        public Order(IEnumerable<Product> products)
        {
            Customer = null;
            Status = OrderStatus.Pending.GetDescription();
            Products = products;
        }
    }
}
