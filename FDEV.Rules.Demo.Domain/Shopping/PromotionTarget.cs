namespace FDEV.Rules.Demo.Domain.Shopping
{
    public enum PromotionTarget
    {
        /// <summary>
        /// Couldn't recognise type. Promotion will present regular price.
        /// </summary>
        Unknown,

        /// <summary>
        /// When chosen, a CustomPromotionScope property needs to be set with an expression to identify the scope.
        /// </summary>
        Custom,

        /// <summary>
        /// Apply to order total. 
        /// </summary>
        OrderTotal,

        /// <summary>
        /// Apply to item total. 
        /// </summary>
        ItemsTotal,

        /// <summary>
        /// Apply to all item but the most expensive one. 
        /// </summary>
        AllItemsButMostExpensive,

        /// <summary>
        /// Apply to most expensive item. 
        /// </summary>
        MostExpensiveItem,

        /// <summary>
        /// Apply to cheapest item. 
        /// </summary>
        CheapestItem,
    }
}
