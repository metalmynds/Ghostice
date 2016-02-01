using Anotar.NLog;
using Ghostice.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core.Diagnostics
{
    public static class DiagnosticManager
    {

        public static List<ExtensionInfo> ScanExtensions(String path)
        {

            var diagnosticOnlyDomain = AppDomain.CreateDomain("ExtensionDiagnosticLevel1");

            var extensions = new List<ExtensionInfo>();

            try
            {
               
                var extensionAssemblyFileList = Directory.GetFiles(path, "*.dll");

                foreach (var extensionAssemblyFilename in extensionAssemblyFileList)
                {

                    Assembly extensionAssembly = null;

                    var errorMesssage = String.Empty;

                    if (!ReflectionHelper.TryLoadAssembly(diagnosticOnlyDomain, extensionAssemblyFilename, out extensionAssembly, out errorMesssage))
                    {
                        LogTo.Warn("Load Extension Assembly Failed! (Diagnostic App Domain Only)\r\nPath: {0}\r\nError: {1}", extensionAssemblyFilename, errorMesssage);
                    }
                    else
                    {
                        LogTo.Info("Loaded Assembly Name: {0} (Diagnostic App Domain Only)", extensionAssembly.FullName);

                        // Look for all Types (Classes) that have the ControlExtensionProvider Attribute

                        var extensionProviderTypes = ExtensionHelper.FindExtensionProviders(extensionAssembly);

                        if (extensionProviderTypes.Count() > 0)
                        {

                            foreach (var extensionProviderType in extensionProviderTypes)
                            {

                                var providerAttribute = AttributeHelper.GetAttribute<ControlExtensionProviderAttribute>(extensionProviderType);

                                var extension = new ExtensionInfo(providerAttribute.Provided);

                                extensions.Add(extension);

                                MethodInfo[] extensionMethodList = (from method in extensionProviderType.GetMethods(BindingFlags.Static | BindingFlags.Public) where method.GetParameters().Length > 0 && method.GetParameters()[0].ParameterType == providerAttribute.Provided select method).ToArray();

                                foreach (var method in extensionMethodList)
                                {
                                    var extensionMethod = new ExtensionMethodInfo(method.Name);

                                    extension.ExtensionMethods.Add(extensionMethod);

                                    foreach (var methodParameter in method.GetParameters())
                                    {
                                        var extensionMethodParameter = new ExtensionParameterInfo() { ParameterName = methodParameter.Name, ParameterType = methodParameter.ParameterType };

                                        extensionMethod.Parameters.Add(extensionMethodParameter);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                AppDomain.Unload(diagnosticOnlyDomain);
            }
            return extensions;
        }
    }
}