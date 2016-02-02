using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.WinForms.Infragistics.Extensions
{
    public static class ReflectionHelper
    {

        public static Object ExecuteMethod(Object Implementer, String MethodName, List<Object> Parameters)
        {
            Type implementerType = Implementer.GetType();

            MethodInfo methodInfo = implementerType.GetMethod(MethodName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.NonPublic);

            if (methodInfo != null)
            {
                return methodInfo.Invoke(Implementer, Parameters != null ? Parameters.ToArray() : null);
            }
            else
            {
                return null;
            }

        }

    }
}
