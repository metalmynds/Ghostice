using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core.Utilities
{
    public class ReflectionHelper
    {

        public static String GetVersion(Assembly Target)
        {
            return Target.GetName().Version.ToString();
        }

        public static String ApplicationVersion { get { return Assembly.GetEntryAssembly().GetName().Version.ToString(); } } // { get { return Assembly.GetEntryAssembly().GetName().Version.ToString(); } }

        public static string ApplicationCompany
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        public static string ApplicationCopyright
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public static string GetProduct(Assembly Target)
        {
            object[] attributes = Target.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyProductAttribute)attributes[0]).Product;

        }

        public static string ApplicationProduct
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public static string ApplicationTrademark
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTrademarkAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyTrademarkAttribute)attributes[0]).Trademark;
            }
        }


        public static string ApplicationTitle
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().CodeBase);
            }
        }


        public static string ApplicationDescription
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }


        public static String ApplicationExecuablePath { get { return Assembly.GetEntryAssembly().Location; } }

        public static String ApplicationDirectory { get { return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location); } }

        public static String AssemblyDirectory(Type Type)
        {
            return Path.GetDirectoryName(Assembly.GetAssembly(Type).Location);
        }

        public static String ExecutingAssemblyDirectory
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        public static String ApplicationFilename { get { return Path.GetFileName(Assembly.GetEntryAssembly().Location); } }

        public static Boolean TryInstantiate(AppDomain Domain, String AssemblyPath, String FQClassName, Object[] Parameters, out Object Instance, out String Error)
        {
            try
            {
                Object instance;

                instance = Instantiate(Domain, AssemblyPath, FQClassName, Parameters);

                if (instance != null)
                {
                    Instance = instance;
                    Error = String.Empty;
                    return true;
                }
                else
                {
                    Instance = null;
                    Error = String.Format("ReflectionHelper:\nInstantiate [{0}] From [{1}] Failed!", FQClassName, AssemblyPath);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Error = String.Format("ReflectionHelper:\nInstantiate [{0}] From [{1}] Failed!\nError: {2}", FQClassName, AssemblyPath, ex.Message);
                Instance = null;
                return false;
            }
        }

        public static Boolean TryInstantiate(Assembly Assembly, String FQClassName, Object[] Parameters, out Object Instance, out String Error)
        {
            try
            {
                Object instance;

                instance = Instantiate(Assembly, FQClassName, Parameters);

                if (instance != null)
                {
                    Instance = instance;
                    Error = String.Empty;
                    return true;
                }
                else
                {
                    Instance = null;
                    Error = String.Format("ReflectionHelper:\nInstantiate [{0}] From [{1}] Failed!", FQClassName, Assembly.FullName);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Error = String.Format("ReflectionHelper:\nInstantiate [{0}] From [{1}] Failed!\nError: {2}", FQClassName, Assembly.FullName, ex.Message);
                Instance = null;
                return false;
            }
        }

        public static Boolean TryInstantiate(Assembly Assembly, Type ClassType, Object[] Parameters, out Object Instance, out String Error)
        {
            return TryInstantiate(Assembly, ClassType.FullName, Parameters, out Instance, out Error);
        }

        public static Object Instantiate(Assembly Assembly, String FQClassName, Object[] Parameters)
        {
            Type[] types = Assembly.GetTypes();

            foreach (Type type in types)
            {
                if (type.FullName == FQClassName)
                {
                    Object instance = Assembly.CreateInstance(type.FullName, true, BindingFlags.CreateInstance, null, Parameters, null, null);

                    return instance;
                }
            }

            return null;
        }

        public static Boolean TryLoadAssembly(AppDomain Domain, String Path, out Assembly Assembly, out String Error)
        {
            Assembly assembly;

            try
            {
                if (TryGetLoadedAssembly(Domain, Path, out assembly))
                {
                    Error = String.Empty;
                    Assembly = assembly;
                    return true;
                }
                else
                {
                    assembly = Assembly.LoadFrom(Path);
                    Error = String.Empty;
                    Assembly = assembly;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Assembly = null;
                Error = String.Format("Try Load Assembly Failed!\nError: {0}\nFilename: {1}", ex.Message, Path);
                return false;
            }
        }

        public static Object Instantiate(AppDomain Domain, String AssemblyPath, String FQClassName, Object[] Parameters)
        {
            String error = String.Empty;
            Assembly assembly = null;

            if (!File.Exists(AssemblyPath))
            {
                throw new FileNotFoundException(String.Format("ReflectionHelper:\nUnable to Find Assembly [{0}]", AssemblyPath));
            }
            else if (TryLoadAssembly(Domain, AssemblyPath, out assembly, out error))
            {
                assembly = Assembly.LoadFile(AssemblyPath);
            }
            else
            {
                throw new InvalidOperationException(String.Format("ReflectionHelper:\nUnable to Load Assembly [{0}]\nError: {1}", AssemblyPath, error));
            }

            return Instantiate(assembly, FQClassName, Parameters);
        }

        public static Boolean TryGetLoadedAssembly(AppDomain Domain, String AssemblyPath, out Assembly LoadedAssembly)
        {
            var targetAssemblyFilename = Path.GetFileName(AssemblyPath);

            Assembly[] assemblies = Domain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {               

                var assemblyFilename = Path.GetFileName(assembly.Location);

                if (assemblyFilename.Equals(targetAssemblyFilename, StringComparison.InvariantCultureIgnoreCase))
                {
                    LoadedAssembly = assembly;
                    return true;
                }
            }

            LoadedAssembly = null;
            return false;
        }

        public static Boolean DerivesFrom(Type Base, Type Target, Boolean ExcludeBase = true)
        {
            if (Base == Target && ExcludeBase) return false;

            return Base.IsAssignableFrom(Target);
        }

        public static List<Type> GetTypesDerivedFrom(Assembly Assembly, Type Base, Boolean IncludeAbstract)
        {
            List<Type> derivedTypes = new List<Type>();

            Type[] types = Assembly.GetTypes();

            foreach (Type type in types)
            {
                if ((type.IsAbstract && IncludeAbstract) || (type.IsAbstract == false))
                {
                    if (Base != type)
                    {
                        if (DerivesFrom(Base, type))
                        {
                            derivedTypes.Add(type);
                        }
                    }
                }
            }

            return derivedTypes;
        }

        public static Object GetPropertyValue(Object Source, String PropertyName, bool IgnoreCase)
        {
            Type sourceType = Source.GetType();

            if (!IgnoreCase)
            {
                PropertyInfo propertyInfo = sourceType.GetProperty(PropertyName);

                if (propertyInfo != null)
                {
                    return propertyInfo.GetValue(Source, null);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                PropertyName = PropertyName.ToLower();

                PropertyInfo[] properties = sourceType.GetProperties();

                foreach (PropertyInfo propertyInfo in properties)
                {
                    if (propertyInfo.Name.ToLower() == PropertyName)
                    {
                        return propertyInfo.GetValue(Source, null);
                    }
                }
            }
            return null;
        }

        public static Object GetStaticFieldValue(Type Implementer, String FieldName, bool IgnoreCase)
        {
            if (!IgnoreCase)
            {

                FieldInfo fieldInfo = Implementer.GetField(FieldName);

                if (fieldInfo != null)
                {
                    return fieldInfo.GetValue(null);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                FieldName = FieldName.ToLower();

                FieldInfo[] fields = Implementer.GetFields();

                foreach (FieldInfo fieldInfo in fields)
                {
                    if (fieldInfo.Name.ToLower() == FieldName)
                    {
                        return fieldInfo.GetValue(null);
                    }
                }
            }
            return null;
        }


        public static Object GetFieldValue(Object Source, String FieldName, bool IgnoreCase)
        {
            Type sourceType = Source.GetType();

            if (!IgnoreCase)
            {
                FieldInfo fieldInfo = sourceType.GetField(FieldName);

                if (fieldInfo != null)
                {
                    return fieldInfo.GetValue(Source);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                FieldName = FieldName.ToLower();

                FieldInfo[] fields = sourceType.GetFields();

                foreach (FieldInfo fieldInfo in fields)
                {
                    if (fieldInfo.Name.ToLower() == FieldName)
                    {
                        return fieldInfo.GetValue(Source);
                    }
                }
            }
            return null;
        }

        public static Object ExecuteMethod(Object Implementer, String MethodName, List<Object> Parameters, bool IgnoreCase)
        {
            Type implementerType = Implementer.GetType();

            if (!IgnoreCase)
            {
                MethodInfo methodInfo = implementerType.GetMethod(MethodName);

                if (methodInfo != null)
                {
                    return methodInfo.Invoke(Implementer, Parameters != null ? Parameters.ToArray() : null);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                MethodName = MethodName.ToLower();

                MethodInfo[] methods = implementerType.GetMethods();

                foreach (MethodInfo methodInfo in methods)
                {
                    if (methodInfo.Name.ToLower() == MethodName && methodInfo.GetParameters().Count() == Parameters.Count)
                    {
                        return methodInfo.Invoke(Implementer, Parameters != null ? Parameters.ToArray() : null);
                    }
                }
            }
            return null;
        }

        public static Object ExecuteStaticMethod(Type Implementer, String StaticMethodName, List<Object> Parameters, bool IgnoreCase)
        {
            if (!IgnoreCase)
            {
                MethodInfo methodInfo = Implementer.GetMethod(StaticMethodName, BindingFlags.Static);

                if (methodInfo != null)
                {
                    return methodInfo.Invoke(Implementer, Parameters != null ? Parameters.ToArray() : null);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                StaticMethodName = StaticMethodName.ToLower();

                MethodInfo[] methods = Implementer.GetMethods();

                foreach (MethodInfo methodInfo in methods)
                {
                    if (methodInfo.Name.ToLower() == StaticMethodName && methodInfo.GetParameters().Count() == Parameters.Count && (methodInfo.Attributes.HasFlag(MethodAttributes.Static)))
                    {
                        return methodInfo.Invoke(Implementer, Parameters != null ? Parameters.ToArray() : null);
                    }
                }
            }
            return null;
        }

    }


}
