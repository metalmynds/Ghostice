using System;
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

        delegate ActionResult UIThreadSafeControlExecute(Control Target, ActionRequest Action);

        delegate ActionResult UIThreadSafeComponentExecute(Control Parent, Component Target, ActionRequest Request);

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
                return (ActionResult)Target.Invoke(new UIThreadSafeControlExecute(Execute), new Object[] { Target, Request });

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

                    case ActionRequest.OperationType.Evaluate:


                        // Dynamic C# Expression Evaluation
                        return ActionResult.Failed(Target.Describe(),"Not Implemented!");


                    case ActionRequest.OperationType.Map:

                        try
                        {

                            String[] properties = null;

                            if (Request.HasParameters)
                            {

                                properties = (String[])Request.Parameters[0].Value;

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

                        var valueEvent = new AutoResetEvent(false);

                        EventHandler visibleHandler = delegate(Object sender, EventArgs e)
                        {
                            if (((Control)sender).Visible)
                            {
                                visibleEvent.Set();
                            }
                        };

                        EventHandler enabledHandler = delegate(Object sender, EventArgs e)
                        {
                            if (((Control)sender).Enabled)
                            {
                                enabledEvent.Set();
                            }
                        };

                        EventHandler textChangedHandler = delegate(Object sender, EventArgs e)
                        {
                            valueEvent.Set();
                        };

                        try
                        {
                            int timeoutSeconds = 30; // Default only used if parameter is missing
                            String value = null;
                            bool anyValue = false;
                            bool valueFound = false;

                            if (Request.HasParameters)
                            {

                                int.TryParse(Request.Parameters[0].ToString(), out timeoutSeconds);

                                if (Request.Parameters.Count() == 3)
                                {
                                    value = Request.Parameters[1].ToString();
                                    bool.TryParse(Request.Parameters[2].ToString(), out anyValue);
                                }
                            }

                            Target.VisibleChanged += visibleHandler;

                            Target.EnabledChanged += enabledHandler;

                            Target.TextChanged += textChangedHandler;

                            if (!visibleEvent.WaitOne(timeoutSeconds))
                            {
                                return ActionResult.Failed(Target.Describe(), String.Format("Timed Out Waiting for Control to be Visible!\r\nTimeout: {0}", timeoutSeconds), typeof(Boolean), "false");
                            }

                            if (!enabledEvent.WaitOne(timeoutSeconds))
                            {
                                return ActionResult.Failed(Target.Describe(), String.Format("Timed Out Waiting for Control to be Enabled!\r\nTimeout: {0}", timeoutSeconds), typeof(Boolean), "false");
                            }

                            if (value != null && value != "null")
                            {
                                if (!valueEvent.WaitOne(timeoutSeconds))
                                {
                                    return ActionResult.Failed(Target.Describe(),
                                        String.Format(
                                            "Timed Out Waiting for Text Value!\r\nTimeout: {0}\r\nValue: [{1}]",
                                            timeoutSeconds, value), typeof (Boolean), "false");
                                }
                                else
                                {
                                    if (anyValue && !String.IsNullOrWhiteSpace(Target.Text))
                                    {
                                        valueFound = true;
                                    }
                                    else
                                    {
                                        if (Target.Text.Equals(value))
                                        {
                                            valueFound = true;
                                        }
                                    }
                                }
                            }

                            if (Target.Enabled && Target.Visible && value != null && valueFound || Target.Enabled && Target.Visible)
                            {
                                return ActionResult.Successful(Target.Describe(), typeof(Boolean), "true");
                            }
                            else
                            {
                                return ActionResult.Failed(Target.Describe(),
                                    String.Format("Control failed to become Enabled & Visible!"), typeof(Boolean),
                                    "false");
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
                            try
                            {
                                Target.TextChanged -= textChangedHandler;
                            }
                            catch
                            {

                            }
                        }

                    case ActionRequest.OperationType.List:

                        var windowInfoList = new List<WindowInfo>();

                        List<Control> windowList = null;

                        List<String> propertyNames = new List<string>();

                        if (Request.HasParameters && Request.Parameters[0].Value is WindowInfo)
                        {
                            var ownerWindowInfo = Request.Parameters[0].Value as WindowInfo;

                            if (ownerWindowInfo != null)
                            {
                                var owningWindow = Control.FromHandle(ownerWindowInfo.Handle);

                                windowList = WindowManager.GetOwnedWindows(owningWindow);
                            }

                        }
                        else
                        {
                            windowList = WindowManager.GetApplicationWindows();
                        }

                        var listArray = windowList.ToArray();

                        foreach (var window in listArray)
                        {
                            var childWindows = WindowManager.GetWindowsChildren(window);

                            windowList.AddRange(childWindows);
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


        public static ActionResult Execute(Control Parent, Component Target, ActionRequest Request)
        {
            if (Parent != null && Parent.InvokeRequired)
            {
                return (ActionResult)Parent.Invoke(new UIThreadSafeComponentExecute(Execute), new Object[] { Parent, Target, Request });

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

                            return ActionResult.Successful(Target.Describe(), output != null ? output.GetType() : null,
                                outputSerialised);

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

                                return ActionResult.Successful(Target.Describe(),
                                    result != null ? result.GetType() : null, resultSerialised);
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
            }
            return null;
        }

    }


}


