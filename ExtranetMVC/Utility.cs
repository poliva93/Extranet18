using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetMVC
{
    public static class Utility
    {
        public static int? ToNullableInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }
    }
}