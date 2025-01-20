using MongoDB.Bson.Serialization.Attributes;

namespace Martiello.Domain.Entity
{
    public class Product : BaseModel
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("time_to_prepare")]
        public int? TimeToPrepare { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }
    }
}
