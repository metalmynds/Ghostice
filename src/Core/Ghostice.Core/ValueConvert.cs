using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostice.Core
{
    public static class ValueConvert
    {

        public static String ToString(Object value)
        {
            return Convert.ToString(value);
        }

        public static String ToString(IntPtr value)
        {
            return value.ToString();
        }
    }
}
