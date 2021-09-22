
using System.Linq;

namespace FDEV.Rules.Demo.Domain.Rules.Utility
{
    public static class ExpressionUtils
    {
        public static bool CheckContains(string check, string valList)
        {
            if (string.IsNullOrEmpty(check) || string.IsNullOrEmpty(valList)) return false;

            var list = valList.Split(',').ToList();
            return list.Contains(check);
        }
    }
}
