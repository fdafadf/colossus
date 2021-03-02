using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace C64.Chess
{
    static class Extensions
    {
        public static T MinBy<T>(this IEnumerable<T> self, Func<T, int> selector)
        {
            T min = self.FirstOrDefault();

            foreach (var item in self)
            {
                if (selector(item) < selector(min))
                {
                    min = item;
                }
            }

            return min;
        }
    }
}
