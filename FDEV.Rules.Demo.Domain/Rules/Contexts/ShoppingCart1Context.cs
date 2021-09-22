using System.Collections.Generic;
using System.Linq;
using FDEV.Rules.Demo.Domain.Shopping;

namespace FDEV.Rules.Demo.Domain.Rules.Contexts
{
    /// <summary>
    /// The first of 3 shopping carts that are used in one of the problems.
    /// </summary>
    public class ShoppingCart1Context : RuleContextBase
    {
        public ShoppingCart1Context()
        {
            var customer = ShoppingData.FullCustomer();
            var seller = ShoppingData.SimpleEmployees(1).ToList()[0];
            Order = Order.Open(customer, seller);

            FillShoppingCart();
            AddAvailablePromotions();
        }

        private void FillShoppingCart()
        {
            Products.Add(ShoppingData.Products.ProductA);
            Products.Add(ShoppingData.Products.ProductA);
            Products.Add(ShoppingData.Products.ProductA);
            Products.Add(ShoppingData.Products.ProductA);
            Products.Add(ShoppingData.Products.ProductA);
        }

        private void AddAvailablePromotions()
        {
            Promotions.Add(ShoppingData.Promotions.PromoA3);
            Promotions.Add(ShoppingData.Promotions.PromoB2);
            Promotions.Add(ShoppingData.Promotions.PromoC1D1);
        }

        public Order Order { get; }

        public IList<Product> Products { get; set; }

        public IList<Promotion> Promotions{ get; set; }

    }
}