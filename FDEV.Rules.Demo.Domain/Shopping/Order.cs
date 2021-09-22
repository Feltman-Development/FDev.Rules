using System;
using System.Collections.Generic;
using System.Linq;
using FDEV.Rules.Demo.Domain.Common;
using FDEV.Rules.Demo.Domain.Identity;

namespace FDEV.Rules.Demo.Domain.Shopping
{
    /// <summary>
    /// The top aggregate root, holding everything together and capable of calcuting iself and optimizing the use of promotions, to ensure the best (lowest) 
    /// price possible price for the customer, given the selected products and available promotions. At this stage, promotions are only calculated on a product 
    /// base, but can easily be extended to include other facts, like customer data, society event/dates (marketday/holiday), customer event/dates 
    /// (Order #1.000/customer birtday), 'happy-hour', shop-related promotions, and the most exciting of all: 
    /// 
    /// - - WARNING - - (VERY NEAR) FUTURE WORLD RANT ON - - WARNING - - 
    /// - - - 'Artificial Intelligence Promotions': helping to deciding when and what promotions to apply to ensure biggest up-sale, highest customer retention 
    /// - - - and general loyalty for repeat business - this hopefully in a relation where the customer experience is one of value and recognition, not manipulation
    /// - - - and product-pushing. That balance is very difficult to get right and this is what we need the AI to find and help us walk :-)                   
    /// - - WARNING - - (VERY NEAR) FUTURE WORLD RANT OFF - - WARNING - -
    /// </summary>
    public class Order : Entity
    {
        public static Order Open(User customer, Employee seller) => new Order { Customer = customer, Seller = seller };

        protected Order()
        {
            Seller = new Employee();
            Items = new List<OrderLineItem>();
            OrderDate = DateTime.UtcNow;
        }

        public User Customer { get; internal set; }

        public Employee Seller { get; internal set; }

        public DateTime OrderDate { get; internal set; }


        #region Order Content

        /// <summary>
        /// The place holder for items. 
        /// </summary>
        public ICollection<OrderLine> ItemLines { get; internal set; }

        /// <summary>
        /// Get the complete collection of order line items, that is all Product, Service (not implemented yet) and Promotion items
        /// </summary>
        public ICollection<OrderLineItem> Items { get; internal set; }

        /// <summary>
        /// Get all items of the promotional type. A promotional item may or may not be used on order, and can be used more then once.
        /// A more correct term would be to say that a promotional item can be applied zero and infinite times.
        /// </summary>
        /// <remarks>
        /// When populating the orderlines it makes sense to arrange products and promotions that are related.
        /// </remarks>
        public IEnumerable<Promotion> PromotionItems => Items.OfType<Promotion>();

        /// <summary>
        /// Get all items of the product type. A product item is a given size, the order holds the quantity that has been entered, no more or no less.
        /// </summary>
        public IEnumerable<Product> ProductItems => Items.OfType<Product>();

        /// <summary>
        /// Get total number of Products in order.
        /// </summary>
        public int NumberOfItemsOnOrder => ProductItems.Count();

        public void AddLine(OrderLineItem item, int quantity) => ItemLines.Add(new OrderLine(this, item, quantity));

        public void RemoveLine(OrderLineItem item) => ItemLines.Where(x => x.LineItem == item);

        public void RemoveLine(OrderLine line) => ItemLines.Remove(line);

        public void CalculateOrder()
        {
            var optimalPromotions = CalculateOptimalUseOfPromotions();
            AssignItemsWithPromotions(ProductItems.Where(x => x.IsAssigned), optimalPromotions);
            PopulateItemsWithPromotions(ProductItems.Where(x => !x.IsAssigned));
            PopulateItemsWithoutPromotions(ProductItems.Where(x => !x.IsAssigned));
        }

        /// <summary>
        /// Iterates all products and promotion possibilities to calculate the optimal use of promotions.
        /// </summary>
        /// <returns> 
        /// A collection of promotions, selected to provide the best (lowest) price possible withe the available promotions
        /// and the given products in combination 
        /// </returns>
        private IEnumerable<Promotion> CalculateOptimalUseOfPromotions()
        {
            foreach (var promotion in PromotionItems.Where(x => x.IsActive))
            {

                yield return promotion;
            }
        }

        /// <summary>
        /// Assigns products to the optimal promotions, making them as 
        /// </summary>
        private void AssignItemsWithPromotions(IEnumerable<Product> productItems, IEnumerable<Promotion> promotionItems)
        {
            foreach (var promotion in promotionItems)
            {

            }
        }

        // <summary>
        /// When all promoions have been assigned products, it makes sense to try and consolidate the same items and to make proomotions
        /// become child items to the products they include. All else equal, this should create an order that is easier to read and understand. 
        /// </summary>
        private void PopulateItemsWithPromotions(IEnumerable<Product> productItems)
        {
            foreach (var products in productItems)
            {

            }
        }

        /// <summary>
        /// When all promoions have been assigned products, it makes sense to try and consolidate the same items and to make proomotions
        /// become child items to the products they include. All else equal, this should create an order that is easier to read and understand. 
        /// </summary>
        private void PopulateItemsWithoutPromotions(IEnumerable<Product> productItems)
        {
            foreach (var products in productItems)
            {

            }
        }

        #endregion Order Content

        /// <summary>
        /// Get if the order is calculated.
        /// </summary>
        public decimal OrderTotal => ItemLines.Sum(x => x.LineTotal());

        /// <summary>
        /// Get if the order is calculated.
        /// </summary>
        public bool IsCalculated { get; }
    }
}
