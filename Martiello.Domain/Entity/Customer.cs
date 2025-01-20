using MongoDB.Bson.Serialization.Attributes;

namespace Martiello.Domain.Entity
{
    public class Customer : BaseModel
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("document")]
        public long Document { get; set; }
    }
}
