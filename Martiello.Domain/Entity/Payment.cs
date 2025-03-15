using Martiello.Domain.Enums;
using Martiello.Domain.Extension;
using MongoDB.Bson.Serialization.Attributes;

namespace Martiello.Domain.Entity {
    public class Payment : BaseModel {

        [BsonElement("order")]
        public int OrderNumber { get; set; }

        [BsonElement("status")]
        public PaymentStatus Status { get; set; }

        [BsonElement("customer")]
        [BsonIgnoreIfNull]
        public Customer Customer { get; set; }

        [BsonElement("qrcode")]
        public string Qrcode { get; set; }

        public Payment(int orderNumber, Customer customer, PaymentStatus status, string qrcode) {
            OrderNumber = orderNumber;
            Customer = customer;
            Status = status;
            Qrcode = qrcode;
        }

    }
}
