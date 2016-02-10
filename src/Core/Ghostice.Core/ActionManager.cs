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

        delegate ActionResult UIThreadSafePerform(Control Target, ActionRequest Action);

        static IgnorableSerializerContractResolver ignorableJsonResolver = new IgnorableSerializerContractResolver();

        static JsonSerializerSettings commonJsonSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = ignorableJsonResolver
        };

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

                    case ActionRequest.OperationType.Tell:

                        try
                        {

                            var serialised = JsonConvert.SerializeObject(Target, commonJsonSettings);

                            return ActionResult.Successful(Target.Describe(), Target.GetType(), serialised);

                        }
                        catch (Exception ex)
                        {

                            return ActionResult.Failed(Target.Describe(), ex);
                        }

                    case ActionRequest.OperationType.Map:

                        try
                        {

                            String[] properties = null;

                            if (Request.HasParameters)
                            {
                                var arguments = from argument in Request.Parameters select argument.ToString();

                                properties = arguments.ToList<String>().ToArray();

                            }

                            var tree = GetControlHierarchy(Target, properties);

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

                        List<String> propertyNames = new List<string>();

                        var windowList = from window in WindowManager.GetWindowControls() where window != null select window;

                        var windowInfoList = new List<WindowInfo>();

                        if (Request.HasParameters)
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

                case ActionRequest.OperationType.Tell:

                    try
                    {

                        var serialised = JsonConvert.SerializeObject(Target, commonJsonSettings);

                        return ActionResult.Successful(Target.Describe(), Target.GetType(), serialised);

                    }
                    catch (Exception ex)
                    {

                        return ActionResult.Failed(Target.Describe(), ex);
                    }



            }
            return null;
        }

        [JsonObject("Control")]
        public class ControlNode
        {

            public ControlNode(String name, String type)
            {
                this.Name = name;
                this.Properties = new Dictionary<String, Object>();
                this.Children = new List<ControlNode>();
                this.Type = type;
            }

            public String Name { get; protected set; }

            public String Type { get; protected set; }

            public Dictionary<String, Object> Properties { get; protected set; }

            public List<ControlNode> Children { get; protected set; }
        }

        private static Object GetControlHierarchy(Control Root, String[] RequestedProperties)
        {
            ControlNode rootNode = null;

            MapControl(Root, RequestedProperties, out rootNode);

            return rootNode;
        }

        private static void MapControl(Control Target, String[] RequestedProperties, out ControlNode MappedControl)
        {

            var ignoreProperties = new String[] { "RightToLeft" };

            var controlNode = new ControlNode(Target.Name, Target.GetType().FullName);

            var properties = Target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            var propertyList = from property in properties where RequestedProperties == null || RequestedProperties.Length == 0 ? true : RequestedProperties.Contains<String>(property.Name) && !ignoreProperties.Contains<String>(property.Name) select property;

            foreach (var property in propertyList)
            {

                var name = property.Name;

                Object value = null;

                try
                {

                    //value = property.GetValue(Target); // 4.5
                    value = property.GetValue(Target, null); // 4.0

                }
                catch (Exception ex)
                {

                    value = "Exception Getting Value! Error Message: " + ex.Message;

                }

                if (controlNode.Properties.ContainsKey(name))
                {
                    int count = 1;

                    String baseName = name;

                    while (controlNode.Properties.ContainsKey(name))
                    {
                        name = String.Format("{0}_{1}", baseName, count);

                        count++;
                    }
                }

                controlNode.Properties.Add(name, value);

            }

            foreach (Control childControl in Target.Controls)
            {
                ControlNode mappedChildControl = null;

                MapControl(childControl, RequestedProperties, out mappedChildControl);

                controlNode.Children.Add(mappedChildControl);
            }

            MappedControl = controlNode;


        }
    }




}
