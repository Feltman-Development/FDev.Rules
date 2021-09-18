using System;

namespace FDEV.Rules.Demo.Core.Utility
{
    public static class DateUtility
    {
        public static string GetUIDate(DateTime date = default) => date != default ? $"{date.Year}-{date.Month}-{date.Day}" : $"{DateTime.UtcNow.Year}-{DateTime.UtcNow.Month}-{DateTime.UtcNow.Day}";
    }
}