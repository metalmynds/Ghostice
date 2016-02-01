using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core.Diagnostics
{
    public class ExtensionInfo
    {

        public ExtensionInfo(Type type)
        {
            this.ExtendsType = type;
            this.ExtensionMethods = new List<ExtensionMethodInfo>();
        }

        public Type ExtendsType
        {
            get; internal set;
        }

        public List<ExtensionMethodInfo> ExtensionMethods
        {
            get; internal set;
        }

        public override string ToString()
        {
            return String.Format("ExtensionInfo: {0} Method Count: {1}", ExtendsType.FullName, ExtensionMethods.Count());
        }
    }
}
