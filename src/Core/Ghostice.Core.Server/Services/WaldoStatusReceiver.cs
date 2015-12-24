using Anotar.NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core.Server.Services
{
    public class WaldoStatusReceiver : IWaldoStatus
    {

        public event EventHandler<ActionEventArgs> ActionPerformed;
        public event EventHandler<StartupEventArgs> SystemUnderTestStarted;
        public event EventHandler<ShutdownEventArgs> SystemUnderTestShutdown;

        public virtual void OnPerformed(ActionRequest Request, ActionResult Result)
        {
            try
            {

                var args = new ActionEventArgs(Request, Result);

                if (ActionPerformed != null)
                {
                    ActionPerformed.Invoke(this, args);
                }

            }
            catch (Exception ex)
            {
                LogTo.ErrorException("Error occurred during ActionPeformed Event Handler!", ex);
            }
        }

        public virtual void OnStarted(String ExecutablePath, String Arguments)
        {
            try
            {

                var args = new StartupEventArgs(ExecutablePath, Arguments);

                if (SystemUnderTestStarted != null)
                {
                    SystemUnderTestStarted.Invoke(this, args);
                }
            }
            catch (Exception ex)
            {
                LogTo.ErrorException("Error occurred during SystemUnderTestStarted Event Handler!", ex);
            }

        }

        public virtual void OnShutdown(ApplicationInfo Application)
        {

            try
            {
                var args = new ShutdownEventArgs(Application);

                if (SystemUnderTestShutdown != null)
                {
                    SystemUnderTestShutdown.Invoke(this, args);
                }
            }
            catch (Exception ex)
            {
                LogTo.ErrorException("Error occurred during SystemUnderTestShutdown Event Handler!", ex);
            }
        }

    }
}
