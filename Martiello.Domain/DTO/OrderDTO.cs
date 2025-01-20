using System.Text.Json.Serialization;

namespace Martiello.Domain.DTO
{
    public class OrderDTO
    {
        public int Number { get; set; }
        public string Status { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CustomerId { get; set; }
        public IEnumerable<ProductDTO> Products { get; set; } = new List<ProductDTO>();
        public decimal TotalPrice => Products.Sum(p => p.Price);
    }
}
