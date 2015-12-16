using Ghostice.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core.Utilities
{
    public class AttributeHelper
    {

        public static Boolean HasAttribute<T>(Type Target)
        {
            var attributes = Target.GetCustomAttributes(typeof(T), false);

            if (attributes.Any())
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static T GetAttribute<T>(Type Class)
        {
            var attributes = Class.GetCustomAttributes(typeof(T), false);

            if (attributes.Any())
            {
                return (T)attributes[0];

            }

            return default(T);
        }

        public static MethodInfo[] GetForgets(Type Window)
        {
            var forgetMethods = new List<MethodInfo>();

            const BindingFlags allInstanceBindingOptions = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            var methods = Window.GetMethods(allInstanceBindingOptions);

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(ForgetsAttribute), false);

                if (attributes.Any())
                {
                    forgetMethods.Add(method);
                }
            }

            return forgetMethods.ToArray();
        }


        public static Boolean HasNotAutomationWindow(Type Class)
        {
            var attributes = Class.GetCustomAttributes(typeof(NotAutomationWindowAttribute), false);

            if (attributes.Any())
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static Boolean HasAutomationWindow(Type Class)
        {
            var attributes = Class.GetCustomAttributes(typeof(AutomationWindowAttribute), false);

            if (attributes.Any())
            {
                return true;
            }
            else
            {
                return false;
            }

        }


    }
}
