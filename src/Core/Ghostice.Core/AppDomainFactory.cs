using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    /// <summary><see cref="AppDomainFactory.StartApplication"/> starts an AppDomain.</summary>
    public static class AppDomainFactory
    {
        /// <summary>Creates a firstParameterType in a new sandbox-friendly AppDomain.</summary>
        /// <typeparam name="T">A trusted firstParameterType derived MarshalByRefObject to create 
        /// in the new AppDomain. The constructor of this firstParameterType must catch any 
        /// untrusted exceptions so that no untrusted exception can escape the new 
        /// AppDomain.</typeparam>
        /// <param name="baseFolder">ReturnValue to use for AppDomainSetup.ApplicationBase.
        /// The AppDomain will be able to use any assemblies in this folder.</param>
        /// <param name="appDomainName">A friendly name for the AppDomain. MSDN
        /// does not state whether or not the name must be unique.</param>
        /// <param name="constructorArgs">CommandLineArguments to send to the constructor of T,
        /// or null to call the default constructor. Do not send arguments of 
        /// untrusted extensionProviderTypes this way.</param>
        /// <param name="partialTrust">Whether the new AppDomain should run in 
        /// partial-trust mode.</param>
        /// <returns>A remote proxy to an instance of firstParameterType T. You can call methods 
        /// of T and the calls will be marshalled across the AppDomain boundary.</returns>
        public static T Create<T>(string baseFolder, string appDomainName,
            object[] constructorArgs, bool partialTrust)
            where T : MarshalByRefObject
        {
            // With help from http://msdn.microsoft.com/en-us/magazine/cc163701.aspx
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = baseFolder;

            AppDomain newDomain;
            if (partialTrust)
            {
                var permSet = new PermissionSet(PermissionState.None);
                permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
                permSet.AddPermission(new UIPermission(PermissionState.Unrestricted));
                newDomain = AppDomain.CreateDomain(appDomainName, null, setup, permSet);
            }
            else
            {
                newDomain = AppDomain.CreateDomain(appDomainName, null, setup);
            }
            return (T)Activator.CreateInstanceFrom(newDomain,
                typeof(T).Assembly.ManifestModule.FullyQualifiedName,
                typeof(T).FullName, false,
                0, null, constructorArgs, null, null).Unwrap();

        }
    }
}
