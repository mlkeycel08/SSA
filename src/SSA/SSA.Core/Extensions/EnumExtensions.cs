using System;
using System.ComponentModel;
using System.Linq;

namespace SSA.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum e)
        {
            var field = e.ToString();
            var attribute =
                e.GetType().GetField(field).GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();

            return attribute != null ? ((DescriptionAttribute) attribute).Description : field;
        }
    }
}