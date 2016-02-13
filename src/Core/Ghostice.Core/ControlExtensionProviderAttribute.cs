using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControlExtensionProviderAttribute : Attribute
    {
        private Type _provided;

        public ControlExtensionProviderAttribute(Type Provided)
        {
            _provided = Provided;
        }

        public Type Provided
        {
            get { return _provided; }
        }
    }
}
