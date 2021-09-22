using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FDEV.Rules.Demo.Domain.Rules.Actions;
using FDEV.Rules.Demo.Domain.Rules.Conditions;

namespace FDEV.Rules.Demo.Domain.Shopping
{

    /// <summary>
    /// Defines a promotion and how to calculate it. 
    /// The promotion holds a rule that defines _when_ the promotion is triggered and can be used. 
    /// </summary>
    public class Promotion : OrderLineItem
    {
        public Promotion()
        {
            //RuleToGoWithEngine = new RuleBase();
            ItemsInPromotion = new List<Product>();
        }

        public Promotion(string name, string sku, decimal amount) : base(name, sku, amount)
        {
            // RuleToGoWithEngine = new RuleBase();
            ItemsInPromotion = new List<Product>();
        }

        /// <summary>
        /// Get the rule - with conditions and actions - that determine where the promotion applies. 
        /// </summary>
        public RuleConditions Contitions { get; protected set; }

        public RuleActions Actions { get; protected set; }

        /// <summary>
        /// Get the type of promotion will be calculated and appplied - the type and scope to apply it.
        /// You will also need the "PromotionTarget" and "PromotionValue" to calculate the promotion. 
        /// </summary>
        public virtual PromotionType PromotionType { get; }

        /// <summary>
        /// Get the target of the promotion application.
        /// You will also need the "PromotionType" and "PromotionValue" to define the application of the promotion. 
        /// </summary>
        public virtual PromotionTarget PromotionTarget { get; }

        /// <summary>
        /// Get the value of the promotion value to be applied.
        /// You will also need the "PromotionType" and "PromotionTarget" to define the application of the promotion. 
        /// </summary>
        public virtual decimal PromotionValue { get; }

        /// <summary>
        /// Get the items that are included - and calculated - as part of the promotion.
        /// </summary>
        public virtual IEnumerable<Product> ItemsInPromotion { get; }

        /// <summary>
        /// Get the regular price of the items included in the promotion.
        /// </summary>
        [NotMapped]
        public virtual decimal RegularPriceOfItems => ItemsInPromotion.Sum(x => x.UnitPrice);

        /// <summary>
        /// Get the price of the items when promotion is applied.
        /// </summary>
        public decimal PromotionalPriceOfItems => CalculatePromotionalPriceOfItems();

        //TODO:
        private decimal CalculatePromotionalPriceOfItems()
        {
            return ItemsInPromotion.Sum(i => i.ItemAmount);
        }

        /// <summary>
        /// Get calculated value of the promotional discount.
        /// </summary>
        [NotMapped]
        public decimal DiscountValueAbsolute => PromotionalPriceOfItems - RegularPriceOfItems;

        /// <summary>
        /// Get calculated percentual value of the promotinal discount.
        /// </summary>
        [NotMapped]
        public decimal DiscountValuePercentage => PromotionalPriceOfItems / RegularPriceOfItems;

        /// <summary>
        /// Get if rule is enabled
        /// </summary>
        public bool IsActive { get; set; }
    }
}
