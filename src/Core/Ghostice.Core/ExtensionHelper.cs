using Ghostice.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    public static class ExtensionHelper
    {

        public static IEnumerable<Type> FindExtensionProviders(Assembly assembly)
        {
            var providers = from type in assembly.GetTypes() where AttributeHelper.HasAttribute<ControlExtensionProviderAttribute>(type) select type;

            return providers;
        }

    }
}
