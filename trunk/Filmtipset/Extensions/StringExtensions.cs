using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Filmtipset.Extensions
{
    public static class StringExtensions
    {
        public static string ToUppercaseFirst(this string self)
        {
            if (string.IsNullOrEmpty(self))
                return self;
            char[] a = self.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}
