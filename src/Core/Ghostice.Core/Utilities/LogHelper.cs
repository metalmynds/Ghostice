using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core.Utilities
{
    public class LogHelper
    {
        public static String EscapeJson(String Json)
        {

            return Json.Replace("{", "{{").Replace("}", "}}");

        }

    }
}
