using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Helper
{
    public static class EnumerableExtensions
    {
        public static int Count(this IEnumerable source)
        {
            int res = 0;

            foreach (var item in source)
                res++;

            return res;
        }
    }

}