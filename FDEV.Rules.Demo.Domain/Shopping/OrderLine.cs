using System.Collections.Generic;
using System.Linq;

namespace FDEV.Rules.Demo.Domain.Shopping
{
    /// <summary>
    /// A placeholder for items, to help organize the layout of an order.
    /// Basically just an order-2-item relation.
    /// </summary>
    public class OrderLine
    {
        public OrderLine(Order order, OrderLineItem item, int quantity, OrderLine parentLine = null)
        {
            Order = order;
            LineItem = item;
            Quantity = quantity;
            if (parentLine != null) return;

            ParentLine = parentLine;
            ParentLine.ChildOrderLines.Add(this);
        }

        /// <summary>
        /// The order this item is part of.
        /// </summary>
        public Order Order { get; }

        /// <summary>
        /// The number of the line on the order.
        /// </summary>
        public int LineNumber { get; }

        /// <summary>
        /// The product, service or promotion assigned to this line.
        /// </summary>
        public OrderLineItem LineItem { get; }

        /// <summary>
        /// The quantity of the item on the line.
        /// </summary>
        public int Quantity { get; }

        /// <summary>
        /// The 
        /// </summary>
        public decimal ItemAmount => (LineItem as Product).UnitPrice;

        /// <summary>
        /// Get the parent order line if this is a child line.
        /// </summary>
        public OrderLine ParentLine { get; }

        /// <summary>
        /// Get any child items, making the system capable of creating items that are related so that for example
        /// some items can be made dependant on others - forcing bundle buys or limit use of promotions
        /// </summary>
        public IList<OrderLine> ChildOrderLines { get; }

        /// <summary>
        /// The calculated amount for the line and any child lines (amount x quantity).
        /// </summary>
        public decimal LineTotal()
        {
            var total = ItemAmount * Quantity;

            if (ChildOrderLines.Any())
            {
                foreach (var detailLine in ChildOrderLines)
                {
                    detailLine.LineTotal();
                }
            }
            return total;
        }
    }
}
