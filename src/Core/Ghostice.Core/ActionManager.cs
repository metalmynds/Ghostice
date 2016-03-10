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
using DynamicExpresso;

namespace Ghostice.Core
{
    public static class ActionManager
    {

        delegate ActionResult UIThreadSafeControlPerform(Control target, ActionRequest action);

        delegate ActionResult UIThreadSafeComponentPerform(Control parent, Component target, ActionRequest request);        

        static Dictionary<String, Lambda> preparedExpresionList = new Dictionary<String, Lambda>();

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

        public static ActionResult Perform(Control target, ActionRequest request)
        {
            if (target != null && target.InvokeRequired)
            {

                return (ActionResult)target.Invoke(new UIThreadSafeControlPerform(Perform), new Object[] { target, request });

            }
            else
            {

                switch (request.Operation)
                {
                    case ActionRequest.OperationType.Get:


                        try
                        {

                            var output = ReflectionManager.Get(target, request.Name);

                            var outputSerialised = JsonConvert.SerializeObject(output, commonJsonSettings);

                            return ActionResult.Successful(target.Describe(), output != null ? output.GetType() : null, outputSerialised);

                        }
                        catch (Exception ex)
                        {
                            return ActionResult.Failed(target.Describe(), ex);
                        }


                    case ActionRequest.OperationType.Set:

                        try
                        {
                            var typedValue = Convert.ChangeType(request.Value, request.ValueType);

                            ReflectionManager.Set(target, request.Name, typedValue);

                            return ActionResult.Successful(target.Describe());

                        }
                        catch (Exception ex)
                        {
                            return ActionResult.Failed(target.Describe(), ex);
                        }


                    case ActionRequest.OperationType.Execute:

                        try
                        {

                            var actualParameters = request.HasParameters ? GetTypedParameters(request.Parameters) : null;

                            var result = ReflectionManager.Execute(target, request.Name, actualParameters);

                            if (result != null)
                            {
                                var resultSerialised = JsonConvert.SerializeObject(result, commonJsonSettings);

                                return ActionResult.Successful(target.Describe(), result != null ? result.GetType() : null, resultSerialised);
                            }
                            else
                            {
                                return ActionResult.Successful(target.Describe());
                            }

                        }
                        catch (Exception ex)
                        {

                            return ActionResult.Failed(target.Describe(), ex);
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

                            if (request.HasParameters)
                            {

                                properties = (String[])request.Parameters[0].Value;

                            }

                            var tree = ControlNode.GetControlHierarchy(target, properties);

                            var serialised = JsonConvert.SerializeObject(tree, commonJsonSettings);

                            return ActionResult.Successful(target.Describe(), target.GetType(), serialised);

                        }
                        catch (Exception ex)
                        {

                            return ActionResult.Failed(target.Describe(), ex);
                        }

                    case ActionRequest.OperationType.Ready:


                        var enabledEvent = new AutoResetEvent(target.Enabled);

                        var visibleEvent = new AutoResetEvent(target.Visible);

                        var valueEvent = new AutoResetEvent(false);

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

                        EventHandler textChangedHandler = delegate (Object sender, EventArgs e)
                        {
                            valueEvent.Set();
                        };

                        try
                        {
                            int timeoutSeconds = 30; // Default only used if parameter is missing
                            String value = null;
                            bool anyValue = false;
                            bool valueFound = false;

                            if (request.HasParameters)
                            {

                                int.TryParse(request.Parameters[0].ToString(), out timeoutSeconds);

                                if (request.Parameters.Count() == 3)
                                {
                                    value = request.Parameters[1].ToString();
                                    bool.TryParse(request.Parameters[2].ToString(), out anyValue);
                                }
                            }

                            target.VisibleChanged += visibleHandler;

                            target.EnabledChanged += enabledHandler;

                            target.TextChanged += textChangedHandler;

                            if (!visibleEvent.WaitOne(timeoutSeconds))
                            {
                                return ActionResult.Failed(target.Describe(), String.Format("Timed Out Waiting for Control to be Visible!\r\nTimeout: {0}", timeoutSeconds), typeof(Boolean), "false");
                            }

                            if (!enabledEvent.WaitOne(timeoutSeconds))
                            {
                                return ActionResult.Failed(target.Describe(), String.Format("Timed Out Waiting for Control to be Enabled!\r\nTimeout: {0}", timeoutSeconds), typeof(Boolean), "false");
                            }

                            if (value != null && value != "null")
                            {
                                if (!valueEvent.WaitOne(timeoutSeconds))
                                {
                                    return ActionResult.Failed(target.Describe(),
                                        String.Format(
                                            "Timed Out Waiting for Text Value!\r\nTimeout: {0}\r\nValue: [{1}]",
                                            timeoutSeconds, value), typeof(Boolean), "false");
                                }
                                else
                                {
                                    if (anyValue && !String.IsNullOrWhiteSpace(target.Text))
                                    {
                                        valueFound = true;
                                    }
                                    else
                                    {
                                        if (target.Text.Equals(value))
                                        {
                                            valueFound = true;
                                        }
                                    }
                                }
                            }

                            if (target.Enabled && target.Visible && value != null && valueFound || target.Enabled && target.Visible)
                            {
                                return ActionResult.Successful(target.Describe(), typeof(Boolean), "true");
                            }
                            else
                            {
                                return ActionResult.Failed(target.Describe(),
                                    String.Format("Control failed to become Enabled & Visible!"), typeof(Boolean),
                                    "false");
                            }

                        }
                        catch (Exception ex)
                        {

                            return ActionResult.Failed(target.Describe(), ex);
                        }
                        finally
                        {
                            try
                            {
                                target.VisibleChanged -= visibleHandler;
                            }
                            catch
                            {

                            }
                            try
                            {
                                target.EnabledChanged -= enabledHandler;
                            }
                            catch
                            {

                            }
                            try
                            {
                                target.TextChanged -= textChangedHandler;
                            }
                            catch
                            {

                            }
                        }

                    case ActionRequest.OperationType.List:

                        var windowInfoList = new List<WindowInfo>();

                        List<Control> windowList = null;

                        List<String> propertyNames = new List<string>();

                        if (request.HasParameters && request.Parameters[0].Value is WindowInfo)
                        {
                            var ownerWindowInfo = request.Parameters[0].Value as WindowInfo;

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


                        if ((request.HasParameters) && (request.Parameters.Count() > 1 && request.Parameters[1].Value is String[]))
                        {
                            propertyNames.AddRange((String[])request.Parameters[1].Value);
                        }
                        else if ((request.HasParameters) && request.Parameters[0].Value is String[])
                        {
                            propertyNames.AddRange((String[])request.Parameters[0].Value);
                        }

                        foreach (var window in windowList)
                        {

                            if (request.HasParameters)
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

                        return ActionResult.Failed("ApplicationControl", String.Format("Execute Failed Due to Invalid Operation Type!\r\nOperation Type: {0} ", System.Enum.GetName(typeof(ActionRequest.OperationType), request.Operation)));

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


        public static ActionResult Perform(Control Parent, Component Target, ActionRequest Request)
        {
            if (Parent != null && Parent.InvokeRequired)
            {
                return (ActionResult)Parent.Invoke(new UIThreadSafeComponentPerform(Perform), new Object[] { Parent, Target, Request });

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


