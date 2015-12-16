using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;
using CommandLine.Text;

namespace Ghostice.ApplicationKit
{
    public class ServerCommandLineOptions
    {
        [Option('e', "endpoint", Required = false,
          HelpText = "Ghostice Server RPC End-Point. e.g. ghostice -e http://localhost:21505/")]
        public String EndPoint { get; set; }

        [Option('v', "verbose", DefaultValue = false,
        HelpText = "Verbose Output.")]
        public Boolean Verbose { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

    }
}
