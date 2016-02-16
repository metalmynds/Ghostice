﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Newtonsoft.Json;
using System.ComponentModel;
using Ghostice.Core.Serialisation;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Ghostice.Core
{
    public static class ActionManager
    {

        delegate ActionResult UIThreadSafePerform(Control Target, ActionRequest Action);

        static IgnorableSerializerContractResolver ignorableJsonResolver = new IgnorableSerializerContractResolver();

        static JsonSerializerSettings commonJsonSettings = new JsonSerializerSettings()
        {
            MaxDepth = 10,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Error = new EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs>(handleJsonError),
            ContractResolver = ignorableJsonResolver,
        };

        static void handleJsonError(Object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
        {
            args.ErrorContext.Handled = true;
            System.Diagnostics.Debug.WriteLine(args.CurrentObject.ToString());
        }

        static ActionManager()
        {

            ignorableJsonResolver.Ignore(typeof(AccessibleObject));
            ignorableJsonResolver.Ignore(typeof(System.Windows.Forms.ToolStripOverflowButton));

        }

        public static ActionResult Execute(Control Target, ActionRequest Request)
        {
            if (Target != null && Target.InvokeRequired)
            {
                return (ActionResult)Target.Invoke(new UIThreadSafePerform(Execute), new Object[] { Target, Request });

            }
            else
            {

                switch (Request.Operation)
                {
                    case ActionRequest.OperationType.Get:


                        try
                        {

                            var output = ReflectionManager.Get(Target, Request.Name);

                            var outputSerialised = JsonConvert.SerializeObject(output, commonJsonSettings);

                            return ActionResult.Successful(Target.Describe(), output != null ? output.GetType() : null, outputSerialised);

                        }
                        catch (Exception ex)
                        {
                            return ActionResult.Failed(Target.Describe(), ex);
                        }


                    case ActionRequest.OperationType.Set:

                        try
                        {
                            var typedValue = Convert.ChangeType(Request.Value, Request.ValueType);

                            ReflectionManager.Set(Target, Request.Name, typedValue);

                            return ActionResult.Successful(Target.Describe());

                        }
                        catch (Exception ex)
                        {
                            return ActionResult.Failed(Target.Describe(), ex);
                        }


                    case ActionRequest.OperationType.Execute:

                        try
                        {

                            var actualParameters = Request.HasParameters ? GetTypedParameters(Request.Parameters) : null;

                            var result = ReflectionManager.Execute(Target, Request.Name, actualParameters);

                            if (result != null)
                            {
                                var resultSerialised = JsonConvert.SerializeObject(result, commonJsonSettings);

                                return ActionResult.Successful(Target.Describe(), result != null ? result.GetType() : null, resultSerialised);
                            }
                            else
                            {
                                return ActionResult.Successful(Target.Describe());
                            }

                        }
                        catch (Exception ex)
                        {

                            return ActionResult.Failed(Target.Describe(), ex);
                        }

                    //case ActionRequest.OperationType.Tell:

                    //    try
                    //    {

                    //        var serialised = JsonConvert.SerializeObject(Target, commonJsonSettings);

                    //        return ActionResult.Successful(Target.Describe(), Target.GetType(), serialised);

                    //    }
                    //    catch (Exception ex)
                    //    {

                    //        return ActionResult.Failed(Target.Describe(), ex);
                    //    }

                    case ActionRequest.OperationType.Map:

                        try
                        {

                            String[] properties = null;

                            if (Request.HasParameters)
                            {
                                var arguments = from argument in Request.Parameters select argument.ToString();

                                properties = arguments.ToList<String>().ToArray();

                            }

                            var tree = ControlNode.GetControlHierarchy(Target, properties);

                            var serialised = JsonConvert.SerializeObject(tree, commonJsonSettings);

                            return ActionResult.Successful(Target.Describe(), Target.GetType(), serialised);

                        }
                        catch (Exception ex)
                        {

                            return ActionResult.Failed(Target.Describe(), ex);
                        }

                    case ActionRequest.OperationType.Ready:


                        var enabledEvent = new AutoResetEvent(Target.Enabled);

                        var visibleEvent = new AutoResetEvent(Target.Visible);

                        EventHandler visibleHandler = delegate (Object sender, EventArgs e)
                        {
                            if (((Control)sender).Visible)
                            {
                                visibleEvent.Set();
                            }
                        };

                        EventHandler enabledHandler = delegate (Object sender, EventArgs e)
                        {
                            if (((Control)sender).Enabled)
                            {
                                enabledEvent.Set();
                            }
                        };

                        try
                        {
                            int timeoutSeconds = 30;

                            if (Request.HasParameters)
                            {

                                int.TryParse(Request.Parameters[0].ToString(), out timeoutSeconds);

                            }

                            Target.VisibleChanged += visibleHandler;

                            Target.EnabledChanged += enabledHandler;

                            if (!visibleEvent.WaitOne(timeoutSeconds))
                            {
                                return ActionResult.Failed(Target.Describe(), String.Format("Timed Out Waiting for Control to be Visible!\r\nTimeout: {0}", timeoutSeconds), typeof(Boolean), "false");
                            }

                            if (!enabledEvent.WaitOne(timeoutSeconds))
                            {
                                return ActionResult.Failed(Target.Describe(), String.Format("Timed Out Waiting for Control to be Enabled!\r\nTimeout: {0}", timeoutSeconds), typeof(Boolean), "false");
                            }

                            if (Target.Enabled && Target.Visible)
                            {
                                return ActionResult.Successful(Target.Describe(), typeof(Boolean), "true");
                            }
                            else
                            {
                                return ActionResult.Failed(Target.Describe(), String.Format("Control failed to become Enabled & Visible!"), typeof(Boolean), "false");
                            }

                        }
                        catch (Exception ex)
                        {

                            return ActionResult.Failed(Target.Describe(), ex);
                        }
                        finally
                        {
                            try
                            {
                                Target.VisibleChanged -= visibleHandler;
                            }
                            catch
                            {

                            }
                            try
                            {
                                Target.EnabledChanged -= enabledHandler;
                            }
                            catch
                            {

                            }
                        }

                    case ActionRequest.OperationType.List:

                        var windowInfoList = new List<WindowInfo>();

                        IEnumerable<Control> windowList = null;

                        List<String> propertyNames = new List<string>();

                        if (Request.HasParameters && Request.Parameters[0].Value is WindowInfo)
                        {
                            var parentWindowInfo = Request.Parameters[0].Value as WindowInfo;

                            if (parentWindowInfo != null)
                            {
                                var windowParent = Control.FromHandle(parentWindowInfo.Handle);

                                windowList = from window in WindowManager.GetChildWindowControls(windowParent) where window != null select window;
                            }

                        }
                        else
                        {
                            windowList = from window in WindowManager.GetProcessWindowControls() where window != null select window;

                        }

                        var listArray = windowList.ToArray();

                        foreach (var window in listArray)
                        {
                            var topLevelList = WindowManager.GetDesktopWindowControls(window);

                            foreach (var topLevel in topLevelList)
                            {
                                var subTopLevel = WindowManager.GetChildWindowControls(topLevel);

                                foreach (var subWindow in subTopLevel)
                                {
                                    windowInfoList.Add(WindowInfo.Create(subWindow));
                                }


                            }
                        }


                        if ((Request.HasParameters) && (Request.Parameters.Count() > 1 && Request.Parameters[1].Value is String[]))
                        {
                            propertyNames.AddRange((String[])Request.Parameters[1].Value);
                        }
                        else if ((Request.HasParameters) && Request.Parameters[0].Value is String[])
                        {
                            propertyNames.AddRange((String[])Request.Parameters[0].Value);
                        }

                        foreach (var window in windowList)
                        {

                            if (Request.HasParameters)
                            {
                                windowInfoList.Add(WindowInfo.Create(window, propertyNames.ToArray()));
                            }
                            else
                            {
                                windowInfoList.Add(WindowInfo.Create(window));
                            }

                        }

                        return ActionResult.Successful("ApplicationControl", typeof(List<WindowInfo>), JsonConvert.SerializeObject(windowInfoList));

                    default:

                        return ActionResult.Failed("ApplicationControl", String.Format("Execute Failed Due to Invalid Operation Type!\r\nOperation Type: {0} ", System.Enum.GetName(typeof(ActionRequest.OperationType), Request.Operation)));

                }

            }


        }

        private static Object[] GetTypedParameters(ActionParameter[] parameters)
        {
            var typed = new List<Object>();

            foreach (var parameter in parameters)
            {

                if (parameter.Value as JArray != null)
                {
                    ///NEED TO FORCE TYPE HERE (JUST FOR NOW)

                    //switch (parameter.ValueType.GenericTypeArguments[0].Name) // 4.5
                    switch (parameter.ValueType.GetGenericArguments()[0].Name) // 4.0
                    {

                        case "Int32":
                            break;

                        case "String":
                            String[] items = (parameter.Value as JArray).Select(jv => (String)jv).ToArray();
                            typed.Add(items);
                            break;
                    }
                }
                else
                {
                    typed.Add(parameter.Value);
                }

            }

            return typed.ToArray();
        }


        public static ActionResult Execute(Component Target, ActionRequest Request)
        {

            switch (Request.Operation)
            {
                case ActionRequest.OperationType.Get:


                    try
                    {
                        var output = ReflectionManager.Get(Target, Request.Name);

                        var outputSerialised = JsonConvert.SerializeObject(output, commonJsonSettings);

                        return ActionResult.Successful(Target.Describe(), output != null ? output.GetType() : null, outputSerialised);

                    }
                    catch (Exception ex)
                    {
                        return ActionResult.Failed(Target.Describe(), ex);
                    }


                case ActionRequest.OperationType.Set:

                    try
                    {
                        var typedValue = Convert.ChangeType(Request.Value, Request.ValueType);

                        ReflectionManager.Set(Target, Request.Name, typedValue);

                        return ActionResult.Successful(Target.Describe());

                    }
                    catch (Exception ex)
                    {
                        return ActionResult.Failed(Target.Describe(), ex);
                    }


                case ActionRequest.OperationType.Execute:

                    try
                    {
                        //var arguments = new Object[] { };

                        //if (Request.HasParameters)
                        //{
                        //    arguments = (from ActionParameter argument in arguments select argument.Value).ToArray();
                        //}

                        var actualParameters = Request.HasParameters ? GetTypedParameters(Request.Parameters) : null;

                        var result = ReflectionManager.Execute(Target, Request.Name, actualParameters);

                        if (result != null)
                        {
                            var resultSerialised = JsonConvert.SerializeObject(result, commonJsonSettings);

                            return ActionResult.Successful(Target.Describe(), result != null ? result.GetType() : null, resultSerialised);
                        }
                        else
                        {
                            return ActionResult.Successful(Target.Describe());
                        }
                    }
                    catch (Exception ex)
                    {

                        return ActionResult.Failed(Target.Describe(), ex);
                    }

                    //case ActionRequest.OperationType.Tell:

                    //    try
                    //    {

                    //        var serialised = JsonConvert.SerializeObject(Target, commonJsonSettings);

                    //        return ActionResult.Successful(Target.Describe(), Target.GetType(), serialised);

                    //    }
                    //    catch (Exception ex)
                    //    {

                    //        return ActionResult.Failed(Target.Describe(), ex);
                    //    }



            }
            return null;
        }

    }


}


