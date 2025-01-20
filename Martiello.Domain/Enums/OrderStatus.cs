using System.ComponentModel;

namespace Martiello.Domain.Enums
{
    public enum OrderStatus
    {
        [Description("Pendente pagamento")]
        Pending = 0,

        [Description("Recebido")]
        Received = 1,

        [Description("Em preparação")]
        InPreparation = 2,

        [Description("Pronto")]
        Ready = 3,

        [Description("Finalizado")]
        Completed = 4
    }
}
