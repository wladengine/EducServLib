using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EducServLib
{
    public static class LinqExtensions
    {
        public static IOrderedEnumerable<T> OrderByDynamic<T>(this IEnumerable<T> source, string propertyName, OrderDirection direction = OrderDirection.Ascending)
        {
            //propertyName = propertyName.Replace("it.", "");
            if (direction == OrderDirection.Ascending)
                return source.OrderBy(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
            else
                return source.OrderByDescending(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
        }
        public static IOrderedEnumerable<T> ThenByDynamic<T>(this IOrderedEnumerable<T> source, string propertyName, OrderDirection direction = OrderDirection.Ascending)
        {
            //propertyName = propertyName.Replace("it.", "");
            if (direction == OrderDirection.Ascending)
                return source.ThenBy(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
            else
                return source.ThenByDescending(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
        }
    }

    public enum OrderDirection
    {
        Ascending,
        Descending
    }
}
