using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Martiello.Domain.Entity
{
    public abstract class BaseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; private set; }

        [BsonElement("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [BsonElement("active")]
        public bool Active { get; set; }

        protected BaseModel()
        {
            Id = ObjectId.GenerateNewId().ToString();
            CreatedAt = DateTime.UtcNow;
            Active = true;
        }
    }
}
