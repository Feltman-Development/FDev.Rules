
using FDEV.Rules.Demo.Domain.Common;

namespace FDEV.Rules.Demo.Domain.Shopping
{
    /// <summary>
    /// The base type for the smallest building block of an order, the items - product, services and promotions so far
    /// </summary>
    public abstract class OrderLineItem : Entity
    {
        public OrderLineItem()
        {
        }

        protected OrderLineItem(string name, string sku, decimal amount)
        {
            Name = name;
            SKU = sku;
            ItemAmount = amount;
        }

        /// <summary>
        /// The order this item is part of.
        /// </summary>
        public Order Order { get; protected set; }

        /// <summary>
        /// The line on the oder this item is on.
        /// </summary>
        public OrderLine OrderLine { get; protected set; }

        /// <summary>
        /// Get or set the item SKU
        /// </summary>
        public string SKU { get; protected set; }

        /// <summary>
        /// The price or discount amount of the item. 
        /// NOTE: The property is used genericly and a more precise property may exist.
        /// </summary>
        public decimal ItemAmount { get; protected set; }

        /// <summary>
        /// Get the promotion if item is part of one. The promotion item is considered a part of itself.
        /// </summary>
        public Promotion PartOfPromotion { get; } = null;

        /// <summary>
        /// Get if the item is calculated. When calculated it won't be part of any further calculations.
        /// </summary>
        public bool IsCalculated { get; protected set; } = false;

        /// <summary>
        /// Get if the item is assigned. When assigned the item is "used".
        /// </summary>
        public bool IsAssigned { get; protected set; } = false;
    }
}
