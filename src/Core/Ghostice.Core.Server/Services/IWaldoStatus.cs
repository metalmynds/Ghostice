using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core.Server.Services
{
    public interface IWaldoStatus
    {

        event EventHandler<ActionEventArgs> ActionPerformed;
        event EventHandler<StartupEventArgs> SystemUnderTestStarted;
        event EventHandler<ShutdownEventArgs> SystemUnderTestShutdown;

        void OnPerformed(ActionRequest Request, ActionResult Result);
        void OnStarted(String ExecutablePath, String Arguments);
        void OnShutdown(ApplicationInfo Application);

    }

    public class StartupEventArgs : EventArgs
    {

        public StartupEventArgs(String Path, String Arguments)
        {
            this.Path = Path;
            this.Arguments = Arguments;
        }

        public String Arguments { get; protected set; }

        public String Path { get; protected set; }

    }

    public class ShutdownEventArgs : EventArgs
    {

        public ShutdownEventArgs(ApplicationInfo Application)
        {
            this.Application = Application;
        }

        public ApplicationInfo Application { get; protected set; }

        public String Error { get; set; }

        public Boolean HasFailed
        {
            get { return !String.IsNullOrWhiteSpace(Error); }
        }

    }

    public class ActionEventArgs : EventArgs
    {

        public ActionEventArgs(ActionRequest Request, ActionResult Result)
        {
            this.Request = Request;
            this.Result = Result;
        }

        public ActionRequest Request { get; protected set; }

        public ActionResult Result { get; protected set; }

    }

}
