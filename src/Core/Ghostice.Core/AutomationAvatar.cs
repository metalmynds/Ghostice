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

        public AutomationAvatar(String extensionsPath)
        {
            if (Directory.Exists(extensionsPath))
            {
                ExtensionManager.LoadExtensions(extensionsPath);
            }
            else
            {
                LogTo.Warn("Extensions Path Supplied Does Not Exist!\r\nPath: {0}", extensionsPath);
            }

        }

        public ApplicationInfo Start(String executablePath, String arguments, int startupTimeOutSeconds)
        {

            Stopwatch startWatch = new Stopwatch();

            startWatch.Start();

            try
            {

                _application = Assembly.LoadFrom(executablePath);

                _applicationThread = new Thread(() => StartApplication(executablePath, arguments));

                _applicationThread.SetApartmentState(ApartmentState.STA);

                _applicationThread.Start();

                Boolean applicationReady = false;

                // Wait until we find window that we can cast (i.e. we get a null in the collection until the main/first window has loaded).

                var started = DateTime.Now;

                while (!applicationReady)
                {

                    LogTo.Debug("Begin Window List");

                    var windowList = WindowManager.GetApplicationWindows();

                    foreach (var window in windowList)
                    {

                        if (window != null)
                        {
                            LogTo.Debug(window.Describe());
                        }
                    }

                    LogTo.Debug("End Window List Count: {0}", windowList.Count);

                    applicationReady = WindowManager.GetApplicationWindows().FindAll(
                        (control) => control != null).Count > 0;

                    if (started.AddSeconds(startupTimeOutSeconds) <= DateTime.Now)
                    {
                        startWatch.Stop();

                        return ApplicationInfo.ReportFailed(executablePath, arguments, String.Format("Timed Out Waiting {0} Seconds for Main/First Window to Load!", startupTimeOutSeconds), startWatch.Elapsed);
                    }

                }

                startWatch.Stop();

                return ApplicationInfo.ReportStarted(AppDomain.CurrentDomain.FriendlyName, executablePath, arguments, Process.GetCurrentProcess().Id, startWatch.Elapsed);

            }
            catch (Exception ex)
            {
                throw new AutomationAvatarException(String.Format("ApplicationManager Constructor Failed!\r\nPath: [{0}]\r\nArguments: [{1}]\r\nError: {2}", executablePath, String.Concat(arguments, " "), ex.Message), ex);
            }

        }

        public ActionResult Perform(ActionRequest request)
        {

            ActionResult result = null;

            try
            {

                var logArgsString = request.HasParameters ? String.Join(", ", from parameter in request.Parameters select parameter.Value != null ? parameter.Value.ToString() : "null") : "None";

                Locator windowLocator = request.HasTarget ? request.Target : null;
                Locator controlPath = windowLocator != null ? request.Target.GetRelativePath() : null;

                var targetWindow = windowLocator != null ? WindowWalker.LocateWindow(windowLocator) : null;

                switch (request.Operation)
                {

                    case ActionRequest.OperationType.Get:

                        LogTo.Info(String.Format("Target: {0} Get: {1}", request.Target, request.Name));

                        var getControl = WindowWalker.Locate(targetWindow, controlPath) as Control;

                        if (getControl != null)
                        {

                            result = ActionManager.Perform(getControl, request);
                            break;
                        }

                        var getComponent = WindowWalker.Locate(targetWindow, request.Target) as Component;

                        if (getComponent != null)
                        {

                            result = ActionManager.Perform(targetWindow, getComponent, request);
                            break;

                        }

                        break;

                    case ActionRequest.OperationType.Set:

                        LogTo.Info(String.Format("Target: {0} Set: {1} Value: {2}", request.Target.ToString(), request.Name, request.Value));

                        controlPath = request.Target.GetRelativePath();

                        var setControl = WindowWalker.Locate(targetWindow, controlPath) as Control;

                        if (setControl != null)
                        {

                            result = ActionManager.Perform(setControl, request);
                            break;

                        }

                        break;

                    case ActionRequest.OperationType.Execute:

                        LogTo.Info(String.Format("Target: {0} Execute: {1} Arguments: {2}", request.Target.ToString(), request.Name, logArgsString));

                        controlPath = request.Target.GetRelativePath();

                        var executeTarget = WindowWalker.Locate(targetWindow, controlPath);

                        if (executeTarget != null && executeTarget is Control)
                        {

                            result = ActionManager.Perform((Control)executeTarget, request);
                            break;
                        }

                        if (executeTarget != null && executeTarget is Component)
                        {

                            result = ActionManager.Perform(targetWindow, (Component)executeTarget, request);
                            break;
                        }

                        break;

                    case ActionRequest.OperationType.Map:

                        LogTo.Info(String.Format("Map: {0}", request.Target.ToString()));

                        controlPath = request.Target.GetRelativePath();

                        var mapControl = WindowWalker.Locate(targetWindow, controlPath) as Control;

                        if (mapControl != null)
                        {

                            result = ActionManager.Perform(mapControl, request);
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

                    case ActionRequest.OperationType.Wait:

                        LogTo.Info(String.Format("Wait: {0}", request.Target.ToString()));

                        var waitTarget = WindowWalker.Locate(targetWindow, controlPath);

                        if (waitTarget != null)
                        {

                            String waitType = Convert.ToString(request.Parameters[0].Value).ToLower();
                            String waitExpression = Convert.ToString(request.Parameters[1].Value);
                            int waitTimeout = Convert.ToInt32(request.Parameters[2].Value);
                            int waitInterval = Convert.ToInt32(request.Parameters[3].Value);

                            var preparedWaitCondition = ExpressionManager.Prepare(waitTarget, waitExpression);

                            var description = ((Control)waitTarget).Describe();

                            switch (waitType)
                            {

                                // Until

                                case "until":

                                    var untilComplete = false;

                                    Stopwatch untilWatch = Stopwatch.StartNew();

                                    while (!untilComplete && (untilWatch.Elapsed).Seconds <= waitTimeout)
                                    {

                                        untilComplete = ExpressionManager.Evaluate(waitTarget, preparedWaitCondition);

                                        if (!untilComplete)
                                        {

                                            Thread.Sleep(waitInterval);

                                        }

                                    }

                                    return untilComplete ? ActionResult.Successful(description, typeof(Boolean), "true") : ActionResult.Failed(description, String.Format("Timeout Waiting for Condition Util [{0}] Waited for {1} Seconds!", waitExpression, waitTimeout), typeof(Boolean), "false");

                                // While                    

                                case "while":

                                    var whileSucessful = true;

                                    Stopwatch whileWatch = Stopwatch.StartNew();

                                    while (whileSucessful && (whileWatch.Elapsed).Seconds <= waitTimeout)
                                    {

                                        whileSucessful = ExpressionManager.Evaluate(waitTarget, preparedWaitCondition);

                                        if (whileSucessful)
                                        {

                                            Thread.Sleep(waitInterval);

                                        }

                                    }

                                    return !whileSucessful ? ActionResult.Successful(description, typeof(Boolean), true.ToString()) : ActionResult.Failed(description, String.Format("Timeout Waiting for Condition While [{0}] Waited for {1} Seconds!", waitExpression, waitTimeout), typeof(Boolean), "false");

                                default:

                                    return ActionResult.Failed(description, String.Format("Unrecognised Wait Type [{0}] for Expression [{1}]", waitType, waitExpression), typeof(Boolean), "false");

                            }


                        }
                        else
                        {
                            return ActionResult.Failed(request.Target.ToString(), String.Format("Unable to Find Target Control!"), typeof(Boolean), "false");
                        }

                    //break;

                    case ActionRequest.OperationType.Ready:

                        // Find the Control

                        LogTo.Info(String.Format("Ready: {0}", request.Target.ToString()));

                        int timeoutSeconds = 30;

                        if (request.HasParameters)
                        {

                            int.TryParse(request.Parameters[0].ToString(), out timeoutSeconds);

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
                            // Check Ready State
                            result = ActionManager.Perform(targetControl, request);
                            break;

                        }
                        else
                        {
                            result = ActionResult.Failed(request.Target.ToString(), "Failed to Find Control!", typeof(Boolean), "false");
                            break;
                        }

                    case ActionRequest.OperationType.List:

                        LogTo.Info(String.Format("List:"));

                        result = ActionManager.Perform(null, request);
                        break;

                    case ActionRequest.OperationType.Unknown:
                    default:
                        LogTo.Error("Action Request Type Not Valid!");
                        result = ActionResult.Failed(request.Target.ToString(), String.Format("Action Request Type is Not Valid!\r\nOperation Type: {0}", Enum.GetName(typeof(ActionRequest.OperationType), request.Operation)));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new AutomationAvatarException(String.Format("ApplicationManager Perform Action Failed!\r\nPath: {0}\r\nMessage: {1}\r\nRequest: {2}", request.Target.ToString(), ex.Message, request.ToJson()), ex);
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
