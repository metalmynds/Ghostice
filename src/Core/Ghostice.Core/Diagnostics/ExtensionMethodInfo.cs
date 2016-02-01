using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core.Diagnostics
{
    public class ExtensionMethodInfo
    {

        public ExtensionMethodInfo(String methodName)
        {
            this.MethodName = methodName;
            this.Parameters = new List<ExtensionParameterInfo>();
        }

        public String MethodName { get; internal set; }

        public List<ExtensionParameterInfo> Parameters { get; internal set; }
    }
}
