using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vanp.DAL.Utils.Extentions
{
    public static class Extention
    {
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query, string orderBy, bool asc) where TEntity : class
        {
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                var propertyInfo = typeof(TEntity).GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    IOrderedQueryable<TEntity> order = null;
                    if (asc)
                    {
                        order = query.OrderBy(o => propertyInfo.GetValue(o, null));
                    }
                    else
                    {
                        order = query.OrderByDescending(o => propertyInfo.GetValue(o, null));
                    }
                    return order;
                }
            }
            return query.OrderBy(o => o.GetType());
        }
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items,
                    Func<T, IEnumerable<T>> childSelector)
        {
            var stack = new Stack<T>(items);
            while (stack.Any())
            {
                var next = stack.Pop();
                yield return next;
                foreach (var child in childSelector(next))
                    stack.Push(child);
            }
        }
        public static bool Contains(this string source, string target, StringComparison comp)
        {
            return source.IndexOf(target, comp) >= 0;
        }
    }
}
