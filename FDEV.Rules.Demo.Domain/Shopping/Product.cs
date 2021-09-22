namespace FDEV.Rules.Demo.Domain.Shopping
{
    public class Product : OrderLineItem
    {
        public Product()
        {

        }

        public Product(Order order)
        {

        }

        public Product(string name, string sku, decimal amount) : base(name, sku, amount)
        {

        }

        public decimal UnitPrice => ItemAmount;


    }
}
