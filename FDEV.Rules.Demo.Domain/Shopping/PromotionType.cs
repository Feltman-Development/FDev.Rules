namespace FDEV.Rules.Demo.Domain.Shopping
{
    public enum PromotionType
    {
        /// <summary>
        /// Couldn't recognise type. Promotion will present regular price.
        /// </summary>
        Unknown,

        /// <summary>
        /// When chosen, a CustomValueType property needs to be set with an expression to define the type of promotion.
        /// </summary>
        Custom,

        /// <summary>
        /// Apply percentual discount. 
        /// </summary>
        PercentualDiscount,

        /// <summary>
        /// Apply a fixed amount discount 
        /// - If discount is higher than price, price will be set to 0.
        /// </summary>
        FixedDiscount,

        /// <summary>
        /// Set a fixed price. 
        /// - To make the item, line or order free - set price to 0.
        /// - If fixed price > than normal price, promotion is not applied.
        /// </summary>
        FixedPrice,
    }
}
