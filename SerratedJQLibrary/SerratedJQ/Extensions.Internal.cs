using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SerratedSharp.SerratedJQ
{
    internal static class Extensions
    {
        public static bool IsNullOrWhiteSpace(this string value )
        {
            return string.IsNullOrWhiteSpace( value );
        }
    }
}
