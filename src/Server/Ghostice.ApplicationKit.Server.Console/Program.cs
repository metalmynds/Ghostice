using Anotar.NLog;
using Ghostice.Core.Server;
using Ghostice.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.ApplicationKit.Server
{
    class Program
    {

        static GhosticeServer _server;

        static int Main(string[] args)
        {

            Console.Title = "Ghostice Application Kit Server - v" + ReflectionHelper.ApplicationVersion;

            //var logo = @"'    _______  __    __    ______        _______..___________. __    ______  _______ \r\n" + 
            //@"'   /  _____||  |  |  |  /  __  \      /       ||           ||  |  /      ||   ____|\r\n"+
            //@"'  |  |  __  |  |__|  | |  |  |  |    |   (----``---|  |----`|  | |  ,----'|  |__   \r\n"+
            //@"'  |  | |_ | |   __   | |  |  |  |     \   \        |  |     |  | |  |     |   __|  \r\n"+
            //@"'  |  |__| | |  |  |  | |  `--'  | .----)   |       |  |     |  | |  `----.|  |____ \r\n"+
            //@"'   \______| |__|  |__|  \______/  |_______/        |__|     |__|  \______||_______|\r\n"+
            //@"'                                                                                   \r\n";

            //Console.WriteLine(String.Format(logo));

            LogTo.Info("Starting Ghostice Application Kit v{0}", ReflectionHelper.GetVersion(Assembly.GetExecutingAssembly()));

            var serverOptions = new ServerCommandLineOptions();

            if (CommandLine.Parser.Default.ParseArguments(args, serverOptions))
            {

                if (!String.IsNullOrWhiteSpace(serverOptions.EndPoint))
                {
                    Ghostice.ApplicationKit.Server.Properties.Settings.Default.AppKitRpcEndpointAddress = serverOptions.EndPoint;
                }


            }

            var executablePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            var extensions = Path.Combine(executablePath, "Extensions");

            _server = new GhosticeServer(extensions);

            _server.Start(new Uri(Ghostice.ApplicationKit.Server.Properties.Settings.Default.AppKitRpcEndpointAddress));

            Console.WriteLine("Press Control + C to Close");

            Console.ReadKey();

            return 0;

        }


    }
}
