using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Ghostice.Core.Utilities;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace Ghostice.Core
{
    [Serializable]
    public class ApplicationInfo
    {

        public enum ApplicationStatus
        {
            Unknown,
            Started,
            Failed
        }

        [JsonConstructor]
        public ApplicationInfo(String ApplicationPath, String CommandLineArguments, String ApplicationVersion, String InstanceIdentifier, String[] IPAddressList, String MachineName, String FullyQualifiedDomainName, String OperatingSystem, ApplicationStatus Status, String Error, int Pid, String StartupTime)
        {
            
            this.InstanceIdentifier = Guid.NewGuid().ToString("N");

            this.ApplicationPath = ApplicationPath;

            this.CommandLineArguments = CommandLineArguments;

            var machineIPList = from address in Dns.GetHostAddresses(Environment.MachineName) select address.ToString();

            this.IPAddressList = machineIPList.ToArray<String>();

            this.MachineName = Environment.MachineName;

            this.FullyQualifiedDomainName = Dns.GetHostEntry(Environment.MachineName).HostName;

            this.OperatingSystem = Environment.OSVersion.ToString();

            this.ApplicationVersion = ApplicationVersion;

            this.Status = Status;

            this.StartupTime = StartupTime;
        }

        public static ApplicationInfo ReportStarted(String ApplicationPath, String CommandLineArguments, int Pid, TimeSpan Duration)
        {
            return Create(GenerateIdentifier(), ApplicationPath, CommandLineArguments, ApplicationStatus.Started, String.Empty, Pid, Duration);
        }

        public static ApplicationInfo ReportFailed(String ApplicationPath, String CommandLineArguments, String Error, TimeSpan Duration)
        {
            return Create(null, ApplicationPath, CommandLineArguments, ApplicationStatus.Started, Error, -1, Duration);
        }

        static String GenerateIdentifier()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static ApplicationInfo Create(String InstanceIdentifier, String ApplicationPath, String CommandLineArguments, ApplicationStatus Status, String Error, int Pid, TimeSpan Duration)
        {

            var applicationVersion = String.Format("{0} v{1}", Assembly.GetExecutingAssembly().GetName().Name, Assembly.GetExecutingAssembly().GetName().Version.ToString());

            var machineIPList = from address in Dns.GetHostAddresses(Environment.MachineName) select address.ToString();

            var iPAddressList = machineIPList.ToArray<String>();

            var applicationInfo = new ApplicationInfo(ApplicationPath,
                                                    CommandLineArguments,
                                                    applicationVersion,
                                                    InstanceIdentifier,
                                                    iPAddressList,
                                                    Environment.MachineName,
                                                    Dns.GetHostEntry(Environment.MachineName).HostName,
                                                    Environment.OSVersion.ToString(), Status, Error, Pid, Duration.ToString());

            return applicationInfo;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public ApplicationStatus Status { get; protected set; }

        public String InstanceIdentifier { get; protected set; }

        public String ApplicationPath { get; protected set; }

        public String CommandLineArguments { get; protected set; }

        public String ApplicationVersion { get; protected set; }

        public String MachineName { get; protected set; }

        public String FullyQualifiedDomainName { get; protected set; }

        public String OperatingSystem { get; protected set; }

        public String[] IPAddressList { get; protected set; }

        public int Pid { get; protected set; }

        public String StartupTime { get; protected set; }
    }
}
