using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    internal static class StringExtensions
    {
        internal static bool Eq(this string data, string other, bool ignoreCase = false)
        {
            if (string.IsNullOrEmpty(other) && string.IsNullOrEmpty(data))
                return true;

            return string.Compare(data, other, ignoreCase) == 0;
        }
    }
}
