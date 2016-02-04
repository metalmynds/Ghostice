using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    public static class MethodManager
    {

        public static MethodInfo Resolve(Object Target, String MethodName, Object[] Parameters)
        {

            // First Try to Find Method on Target Class

            // Change any Int64 Types to Int32 (or Method will may not be found)
            var paramterTypes = Parameters != null ? from arg in Parameters select arg.GetType() == typeof(Int64) ? typeof(Int32) : arg.GetType() : null;

            // Change any instances of Int64 in CommandLineArguments to Int32
            //var normalisedArguments = Parameters != null ? from arg in Parameters select arg.GetType() == typeof(Int64) ? Convert.ToInt32(arg) : arg : null;

            MethodInfo method = null;

            if (paramterTypes != null)
            {

                method = Target.GetType().GetMethod(MethodName, paramterTypes.ToArray());
            }
            else
            {

                method = Target.GetType().GetMethod(MethodName);
            }


            return method;

        }

        public static MethodInfo ResolveExtension(Object Target, String MethodName, Object[] Parameters)
        {

            Type[] parameterTypeList = null;

            if (Parameters != null)
            {

                var types = from arg in Parameters select arg.GetType() == typeof(Int64) ? typeof(Int32) : arg.GetType();

                var typeList = types.ToList<Type>();

                typeList.Insert(0, Target.GetType());           

                parameterTypeList = typeList.ToArray();

            }

            MethodInfo resolvedMethod = null;

            var targetType = Target.GetType();

            List<Type> extensions = null;

            if (ExtensionManager.TryResolveExtensions(targetType, out extensions))
            {

                foreach (var extensionClass in extensions)
                {

                    MethodInfo extensionMethod = null;

                    // 4.5

                    //if (Parameters != null)
                    //{

                    //    extensionMethod = extensionClass.GetMethod(MethodName, parameterTypeList.ToArray());

                    //}
                    //else
                    //{
                    //    extensionMethod = extensionClass.GetMethod(MethodName);
                    //}

                    // 4.0

                    extensionMethod = extensionClass.GetMethod(MethodName);

                    if (extensionMethod != null)
                    {

                        resolvedMethod = extensionMethod;

                    }

                }

            }

            return resolvedMethod;

        }

    }
}
