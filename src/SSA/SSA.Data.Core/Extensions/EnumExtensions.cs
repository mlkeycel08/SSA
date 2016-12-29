using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SSA.Data.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplay(this Enum e)
        {
            var attribute = e.GetType()
                .GetField(e.ToString())
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .SingleOrDefault() as DisplayAttribute;
            return attribute == null ? e.ToString() : attribute.Name;
        }
    }
}