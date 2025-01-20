using System.ComponentModel;

namespace Martiello.Domain.Enums
{
    public enum ProductCategory
    {
        [Description("Lanche")]
        Sandwich = 1,

        [Description("Acompanhamento")]
        SideDish = 2,

        [Description("Bebida")]
        Drink = 3,

        [Description("Sobremesa")]
        Dessert = 4
    }
}
