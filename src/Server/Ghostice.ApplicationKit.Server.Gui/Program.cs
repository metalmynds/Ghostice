using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using Anotar.Log4Net;
using Ghostice.Core.Utilities;
using System.Reflection;

namespace Ghostice.ApplicationKit
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {

            LogTo.Info("Starting Ghostice Application Kit v{0}", ReflectionHelper.GetVersion(Assembly.GetExecutingAssembly())); 

            var serverOptions = new ServerCommandLineOptions();

            if (CommandLine.Parser.Default.ParseArguments(args, serverOptions))
            {

                if (!String.IsNullOrWhiteSpace(serverOptions.EndPoint))
                {
                    Ghostice.ApplicationKit.Properties.Settings.Default.AppKitRpcEndpointAddress = serverOptions.EndPoint;
                }

            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormAppKit());

            return 0;

        }
    }
}
