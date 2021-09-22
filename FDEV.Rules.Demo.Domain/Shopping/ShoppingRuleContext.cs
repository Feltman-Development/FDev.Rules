using FDEV.Rules.Demo.Domain.Rules.Contexts;

namespace FDEV.Rules.Demo.Domain.Shopping
{
    public class ShoppingRuleContext : RuleContextBase
    {
        public ShoppingRuleContext(Order order) : base(order)
        {
     
        }
    }
}
