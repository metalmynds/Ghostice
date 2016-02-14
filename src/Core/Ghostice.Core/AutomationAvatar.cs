using Anotar.NLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ghostice.Core
{
    public class AutomationAvatar : MarshalByRefObject
    {

        private Thread _applicationThread;
        private Assembly _application;

        public AutomationAvatar(String ExtensionsPath)
        {
            if (Directory.Exists(ExtensionsPath))
            {
                ExtensionManager.LoadExtensions(ExtensionsPath);
            }
            else
            {
                LogTo.Warn("Extensions Path Supplied Does Not Exist!\r\nPath: {0}", ExtensionsPath);
            }

        }

        public ApplicationInfo Start(String ExecutablePath, String Arguments, int StartupTimeOutSeconds)
        {

            Stopwatch startWatch = new Stopwatch();

            startWatch.Start();

            try
            {

                _application = Assembly.LoadFrom(ExecutablePath);

                _applicationThread = new Thread(() => StartApplication(ExecutablePath, Arguments));

                _applicationThread.SetApartmentState(ApartmentState.STA);

                _applicationThread.Start();

                Boolean applicationReady = false;

                // Wait until we find window that we can cast (i.e. we get a null in the collection until the main/first window has loaded).

                var started = DateTime.Now;

                while (!applicationReady)
                {

                    LogTo.Debug("Begin Window List");

                    var windowList = WindowManager.GetWindowControls();

                    foreach (var window in windowList)
                    {

                        if (window != null)
                        {
                            LogTo.Debug(window.Describe());
                        }
                    }

                    LogTo.Debug("End Window List Count: {0}", windowList.Count);

                    applicationReady = WindowManager.GetWindowControls().FindAll(
                        (control) => control != null).Count > 0;

                    if (started.AddSeconds(StartupTimeOutSeconds) <= DateTime.Now)
                    {
                        startWatch.Stop();

                        return ApplicationInfo.ReportFailed(ExecutablePath, Arguments, String.Format("Timed Out Waiting {0} Seconds for Main/First Window to Load!", StartupTimeOutSeconds), startWatch.Elapsed);
                    }

                }

                startWatch.Stop();

                return ApplicationInfo.ReportStarted(AppDomain.CurrentDomain.FriendlyName, ExecutablePath, Arguments, Process.GetCurrentProcess().Id, startWatch.Elapsed);

            }
            catch (Exception ex)
            {
                throw new AutomationAvatarException(String.Format("ApplicationManager Constructor Failed!\r\nPath: [{0}]\r\nArguments: [{1}]\r\nError: {2}", ExecutablePath, String.Concat(Arguments, " "), ex.Message), ex);
            }

        }

        public ActionResult Perform(ActionRequest Request)
        {

            ActionResult result = null;

            try
            {

                var logArgsString = Request.HasParameters ? String.Join(", ", from parameter in Request.Parameters select parameter.Value.ToString()) : "None";

                Locator windowLocator = Request.HasTarget ? Request.Target : null;
                Locator controlPath = null;

                var targetWindow = windowLocator != null ? WindowWalker.LocateWindow(windowLocator) : null;            
                                             
                switch (Request.Operation)
                {

                    case ActionRequest.OperationType.Get:

                        LogTo.Info(String.Format("Target: {0} Get: {1}", Request.Target, Request.Name));

                        controlPath = Request.Target.GetRelativePath();
                        
                        var getControl = WindowWalker.Locate(targetWindow, controlPath) as Control;

                        if (getControl != null)
                        {

                            result = ActionManager.Execute(getControl, Request);

                            break;
                        }

                        var getComponent = WindowWalker.Locate(targetWindow, Request.Target) as Component;

                        if (getComponent != null)
                        {

                            result = ActionManager.Execute(getComponent, Request);
                            break;

                        }

                        break;

                    case ActionRequest.OperationType.Set:


                        LogTo.Info(String.Format("Target: {0} Set: {1} Value: {2}", Request.Target.ToString(), Request.Name, Request.Value));

                        controlPath = Request.Target.GetRelativePath();

                        var setControl = WindowWalker.Locate(targetWindow, controlPath) as Control;

                        if (setControl != null)
                        {

                            result = ActionManager.Execute(setControl, Request);
                            break;

                        }

                        break;

                    case ActionRequest.OperationType.Execute:

                        LogTo.Info(String.Format("Target: {0} Execute: {1} Arguments: {2}", Request.Target.ToString(), Request.Name, logArgsString));

                        controlPath = Request.Target.GetRelativePath();

                        var executeTarget = WindowWalker.Locate(targetWindow, controlPath);

                        if (executeTarget != null && executeTarget is Control)
                        {

                            result = ActionManager.Execute((Control)executeTarget, Request);
                            break;
                        }

                        if (executeTarget != null && executeTarget is Component)
                        {

                            result = ActionManager.Execute((Component)executeTarget, Request);
                            break;
                        }

                        break;

                    case ActionRequest.OperationType.Map:

                        LogTo.Info(String.Format("Map: {0}", Request.Target.ToString()));

                        controlPath = Request.Target.GetRelativePath();

                        var mapControl = WindowWalker.Locate(targetWindow, controlPath) as Control;

                        if (mapControl != null)
                        {

                            result = ActionManager.Execute(mapControl, Request);
                            break;

                        }

                        break;

                    //case ActionRequest.OperationType.Tell:

                    //    LogTo.Info(String.Format("Map: {0}", Request.Location.ToString()));

                    //    windowDescriptor = Request.Location.Path[0];

                    //    var tellTargetWindow = WindowWalker.Locate(windowDescriptor);

                    //    controlPath = Request.Location.GetRelativePath();

                    //    Control target = null;

                    //    if (controlPath.Path.Count > 0)
                    //    {
                    //        target = WindowWalker.Locate(tellTargetWindow, controlPath) as Control;
                    //    }
                    //    else
                    //    {
                    //        target = tellTargetWindow;
                    //    }

                    //    if (target != null)
                    //    {

                    //        result = ActionManager.Execute(target, Request);
                    //        break;

                    //    }

                    //    break;


                    case ActionRequest.OperationType.Ready:

                        LogTo.Info(String.Format("Ready: {0}", Request.Target.ToString()));

                        controlPath = Request.Target.GetRelativePath();

                        int timeoutSeconds = 30;

                        if (Request.HasParameters)
                        {

                            int.TryParse(Request.Parameters[0].ToString(), out timeoutSeconds);

                        }

                        Control targetControl = null;

                        var started = DateTime.Now;

                        while (targetControl == null && ((DateTime.Now - started).Seconds <= timeoutSeconds))
                        {
                            try
                            {

                                Application.DoEvents();

                                var readyTargetWindow = WindowWalker.LocateWindow(windowLocator);

                                if (readyTargetWindow != null)
                                {
                                    targetControl = WindowWalker.Locate(readyTargetWindow, controlPath) as Control;
                                }

                            }
                            catch (Exception ex)
                            {
                                LogTo.DebugException("Anticipated Exception Waiting for Control to be Instantiated", ex);
                            }

                        }

                        if (targetControl != null)
                        {

                            result = ActionManager.Execute(targetControl, Request);
                            break;

                        }
                        else
                        {
                            result = ActionResult.Failed(Request.Target.ToString(), "Failed to Find Control!", typeof(Boolean), "false");
                            break;
                        }

                    case ActionRequest.OperationType.List:

                        LogTo.Info(String.Format("List:"));

                        result = ActionManager.Execute(null, Request);
                        break;

                    case ActionRequest.OperationType.Unknown:
                    default:
                        LogTo.Error("Action Request Type Not Valid!");
                        result = ActionResult.Failed(Request.Target.ToString(), String.Format("Action Request Type is Not Valid!\r\nOperation Type: {0}", Enum.GetName(typeof(ActionRequest.OperationType), Request.Operation)));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new AutomationAvatarException(String.Format("ApplicationManager Perform Action Failed!\r\nPath: {0}\r\nMessage: {1}\r\nRequest: {2}", Request.Target.ToString(), ex.Message, Request.ToJson()), ex);
            }

            return result;

        }


        //public String Perform(String Request)
        //{

        //    ActionRequest request = ActionRequest.FromJson(Request);

        //    ActionResult result = Perform(request);

        //    return result.ToJson();

        //}

        protected void StartApplication(String Path, String Arguments)
        {
            try
            {
                Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(Path);

                AppDomain.CurrentDomain.ExecuteAssembly(Path, Arguments.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            }
            catch (Exception ex)
            {

                LogTo.ErrorException(String.Format("ApplicationManager StartApplication Failed!!\r\nPath: [{0}]\r\nArguments: [{1}]\r\nError: {2}", Path, String.Concat(Arguments, " "), ex.Message), ex);

            }
        }

        public void Shutdown()
        {

            if (_applicationThread != null)
            {
                try
                {
                    _applicationThread.Abort();
                }
                catch (Exception ex)
                {
                    LogTo.WarnException("ApplicationManager Shutdown SystemUnderTest Failed!", ex);

                }
            }

        }

        protected Thread ApplicationThread
        {
            get
            {
                return _applicationThread;
            }
        }
    }

    [Serializable]
    public class AutomationAvatarException : Exception
    {

        protected AutomationAvatarException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }

        public AutomationAvatarException(String Message) :
            base(Message)
        {

        }

        public AutomationAvatarException(String Message, Exception Inner) :
            base(Message, Inner)
        {

        }
    }
}
