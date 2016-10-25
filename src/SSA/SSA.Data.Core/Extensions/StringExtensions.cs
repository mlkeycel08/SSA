using System.Linq;

namespace SSA.Data.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> elements, int? page, int? pageSize)
        {
            if (page.HasValue && pageSize.HasValue)
                return elements.Skip(page.Value * pageSize.Value).Take(pageSize.Value);
            return elements;
        }
    }
}