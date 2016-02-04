using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;

namespace Ghostice.Core
{
    public static class ReflectionManager
    {

        public static Object Execute(Object target, String methodName, Object[] arguments)
        {

            try
            {               

                MethodInfo method = null;

                Boolean extension = false;

                method = MethodManager.Resolve(target, methodName, arguments);

                if (method == null)
                {
                    method = MethodManager.ResolveExtension(target, methodName, arguments);
                    extension = method != null;
                }

                if (method == null) throw new ReflectionManagerException(String.Format("Method or Extension Method Named [{0}] Not Found! for Target Type: {1}", methodName, target.GetType().FullName));

                if (extension)
                {
                    if (arguments != null)
                    {
                        var newArguments = new Object[arguments.Length + 1];

                        newArguments[0] = target;

                        Array.Copy(arguments, 0, newArguments, 1, arguments.Length);

                        arguments = newArguments;

                    }
                    else
                    {
                        arguments = new Object[] { target };
                    }
                }

                if (arguments != null)
                {
                    return method.Invoke(target, arguments);
                }
                else
                {
                    //return method.Invoke(target, null); // 4.5
                    return method.Invoke(target, new object[] {}); // 4.0
                }
            }
            catch (Exception ex)
            {
                if (ex is ReflectionManagerException) throw;

                String message = null;

                if (ex.InnerException != null)
                {
                    message = ex.InnerException.Message;
                }
                else
                {
                    message = ex.Message;
                }

                throw new ReflectionManagerException(String.Format("ReflectionManager Execute Method [{0}] on [{1}] Failed!\r\nError: {2}", methodName, target != null ? target.ToString() : "Null", message), ex);
            }

        }

        public static void Set(Object Target, String PropertyName, Object Value)
        {

            var controlProperties = Target.GetType().GetProperties();

            MethodInfo setMethod = null;

            foreach (var controlProperty in controlProperties)
            {

                if (String.Equals(controlProperty.Name, PropertyName, StringComparison.InvariantCultureIgnoreCase))
                {

                    var method = controlProperty.GetSetMethod();

                    var parameters = method.GetParameters();

                    var firstParameterType = parameters[0].ParameterType;

                    if (Type.Equals(firstParameterType, typeof(System.Object)))
                    {

                        setMethod = method;

                        break;

                    }
                    else if (Type.Equals(firstParameterType, Value.GetType()))
                    {

                        setMethod = method;

                        break;
                    }
                }
            }

            if (setMethod == null)
            {
                throw new ReflectionManagerException(String.Format("Unable to Find Setter for Property [{0}]\r\nTarget: {1}", PropertyName, Target as Control != null ? ((Control)Target).Name : Target.ToString()));
            }

            try
            {

                setMethod.Invoke(Target, new Object[] { Value });

            }
            catch (Exception ex)
            {
                throw new ReflectionManagerException(String.Format("Set Property Failed! Property: {0} Value: {1}\r\nError: {2}", PropertyName, Value, ex.Message), ex);
            }

        }

        public static Object Get(Object Target, String PropertyName)
        {
            var controlProperty = Target.GetType().GetProperty(PropertyName);

            try
            {

                var result = controlProperty.GetGetMethod().Invoke(Target, null);

                return result;
            }
            catch (Exception ex)
            {
                throw new ReflectionManagerException(String.Format("Get Property Failed! Property: {0}\r\nError: {1}", PropertyName, ex.Message), ex);
            }

        }

        [Serializable]
        public class ReflectionManagerException : Exception
        {

            protected ReflectionManagerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
                : base(info, context)
            { }


            public ReflectionManagerException(String Message)
                : base(Message)
            {

            }

            public ReflectionManagerException(String Message, Exception Inner)
                : base(Message, Inner)
            {

            }

        }

    }

}
