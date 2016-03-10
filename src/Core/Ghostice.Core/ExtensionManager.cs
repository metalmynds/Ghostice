using Anotar.NLog;
using Ghostice.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    public static class ExtensionManager
    {

        static Dictionary<Type, List<Type>> _extensions = new Dictionary<Type, List<Type>>();

        public static void LoadExtensions(String Path)
        {

            LogTo.Info("Loading Extensions");

            try
            {

                if (!String.IsNullOrWhiteSpace(Path))
                {

                    var extensionAssemblyFileList = Directory.GetFiles(Path, "*.dll");

                    foreach (var extensionAssemblyFilename in extensionAssemblyFileList)
                    {

                        Assembly extensionAssembly = null;
                        var errorMesssage = String.Empty;

                        if (!ReflectionHelper.TryLoadAssembly(AppDomain.CurrentDomain, extensionAssemblyFilename, out extensionAssembly, out errorMesssage))
                        {
                            LogTo.Warn("Load Extension Assembly Failed!\r\nPath: {0}\r\nError: {1}", extensionAssemblyFilename, errorMesssage);
                        }
                        else
                        {
                            LogTo.Info("Loaded Assembly Name: {0}", extensionAssembly.FullName);

                            // Look for all Types (Classes) that have the ControlExtensionProvider Attribute

                            var extensionProviderTypes = ExtensionHelper.FindExtensionProviders(extensionAssembly);

                            foreach (var extensionProvider in extensionProviderTypes)
                            {

                                var providerAttribute = AttributeHelper.GetAttribute<ControlExtensionProviderAttribute>(extensionProvider);

                                // Index the Provider by the type provided this allows multiple extensions to target the same currentControl (obviously as long as names don't clash!)

                                AddExtension(providerAttribute.Provided, extensionProvider);

                            }

                        }

                    }
                }

                LogTo.Info("Load Extensions Complete");
            }
            catch (Exception ex)
            {
                LogTo.ErrorException(String.Format("Load Extensions Failed!\r\nPath: {0}", Path), ex);
            }

        }

        public static void AddExtension(Type Target, Type Extension)
        {

            if (!_extensions.ContainsKey(Target))
            {

                _extensions.Add(Target, new List<Type>());

                _extensions[Target].Add(Extension);


            }
            else
            {

                _extensions[Target].Add(Extension);

            }
        }


        public static Boolean TryResolveExtensions(Type ControlType, out List<Type> Extensions)
        {

            Extensions = new List<Type>();

            if (!HasExtension(ControlType))
                return false;

            Extensions.AddRange(ExtensionManager.GetExtensions(ControlType));

            return Extensions.Count > 0;
        }

        public static List<Type> GetExtensions(Type controlType)
        {
            var extensionType = GetAssignableType(controlType);

            if (extensionType != null)
            {
                controlType = extensionType;
            }

            return _extensions[controlType];

        }

        public static Type GetAssignableType(Type controlType)
        {
            foreach (var extendedType in _extensions.Keys)
            {

                if (extendedType.IsAssignableFrom(controlType))
                {
                    return extendedType;                                        
                }

            }

            return null;
        }

        //public static Boolean ExtensionExists<T>()
        //{
        //    return _extensions.Keys.Contains(typeof(T));
        //}

        public static Boolean HasExtension(Type controlType)
        {

            // Direct Check
            if (_extensions.Keys.Contains(controlType)) return true;

            // Decendant Check
            return GetAssignableType(controlType) != null;

        }
    }
}
