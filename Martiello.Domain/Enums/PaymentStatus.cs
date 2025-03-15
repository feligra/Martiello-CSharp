using System.ComponentModel;

namespace Martiello.Domain.Enums {
    public enum PaymentStatus {
        [Description("Pendente pagamento")]
        Pending = 0,
        [Description("Aprovado")]
        Approved = 1,
        [Description("Recusado")]
        Refused = 2,
    }
}
