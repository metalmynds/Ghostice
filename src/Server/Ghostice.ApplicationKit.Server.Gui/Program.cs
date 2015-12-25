using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Anotar.NLog;
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
                    if (Uri.IsWellFormedUriString(serverOptions.EndPoint, UriKind.Absolute))
                    {
                        Ghostice.ApplicationKit.Properties.Settings.Default.AppKitRpcEndpointAddress = serverOptions.EndPoint;

                    } else
                    {
                        LogTo.Fatal("Supplied EndPoint Parameter is Not a Valid Url!\nSupplied Url: {0}", serverOptions.EndPoint);
                        return 1;
                    }
                }

            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormAppKit());

            return 0;

        }
    }
}
