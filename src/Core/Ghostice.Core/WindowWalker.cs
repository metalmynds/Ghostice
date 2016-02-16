using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ghostice.Core
{
    public static class WindowWalker
    {
        delegate Object UIThreadSafeLocate(Control Root, Locator Path);

        public static Control LocateWindow(Locator windowLocator)
        {

            Control rootWindow = null;
            Control targetWindow = null;

            Descriptor rootWindowDescriptor = windowLocator.GetRootWindowDescriptor();

            rootWindow = LocateDesktopLevelWindow(rootWindowDescriptor);

            if (rootWindow != null)
            {

                targetWindow = rootWindow;

                var windowRelativePath = windowLocator.GetWindowPath(rootWindowDescriptor);

                foreach (var windowDescriptor in windowRelativePath.Path)
                {
                    var processTopLevelWindow = LocateDesktopLevelWindows(rootWindow);

                    foreach (var ownedWindow in processTopLevelWindow)
                    {
                        //targetWindow = LocateChildWindow(ownedWindow, windowDescriptor);

                        //if (targetWindow != null)
                        //{
                        //    break;
                        //}
                        if (WindowWalker.Compare(windowDescriptor, ownedWindow))
                        {
                            targetWindow = ownedWindow;
                            break;
                        }
                    }

                    //var topLevelChildWindow = LocateDesktopLevelWindow(windowDescriptor);

                    //if (topLevelChildWindow != null)
                    //{

                    //    targetWindow = LocateChildWindow(topLevelChildWindow, windowDescriptor);

                    //}

                    if (targetWindow == null)
                    {
                        targetWindow = LocateChildWindow(rootWindow, windowDescriptor);

                    }

                }

            }

            return targetWindow;

        }


        //}

        //foreach (var subWindowDesciptor in windowLocator.GetWindowPath(rootWindowDescriptor).Path)
        //{



        //    if (targetWindow == null)
        //    {

        //        foreach (var subWindow in topLevelWindows)
        //        {

        //            if (WindowWalker.Compare(rootWindowDescriptor, subWindow))
        //            {
        //                targetWindow = subWindow;
        //                break;
        //            }
        //        }


        //    }

        //}

        //    targetWindow = rootWindow;
        //}

        //return targetWindow;

        //return null;



        private static Control LocateChildWindow(Control parentWindow, Descriptor childWindowDescriptor)
        {
            var childWindows = WindowManager.GetChildWindowControls(parentWindow);

            foreach (var childWindow in childWindows)
            {

                if (childWindow != null)
                {

                    if (WindowWalker.Compare(childWindowDescriptor, childWindow))
                    {
                        return childWindow;
                    }

                }
            }

            return null;
        }

        private static Control LocateDesktopLevelWindow(Descriptor windowDescriptor)
        {
            var topLevelWindows = WindowManager.GetProcessWindowControls();

            foreach (var topWindow in topLevelWindows)
            {

                if (WindowWalker.Compare(windowDescriptor, topWindow))
                {
                    return topWindow;
                }
            }

            return null;
        }

        private static List<Control> LocateDesktopLevelWindows(Control owner)
        {
            return WindowManager.GetDesktopWindowControls(owner);
        }

        public static Object Locate(Control Root, Locator Target)
        {

            Object currentControl = Root;
            Boolean failed = false;
            Descriptor notFoundDescriptor = null;

            if (Root.InvokeRequired)
            {
                return Root.Invoke(new UIThreadSafeLocate(Locate), new Object[] { Root, Target });
            }
            else
            {

                foreach (var descriptor in Target.Path)
                {
                    Control childControl = null;

                    if (TryLocate((Control)currentControl, descriptor, out childControl))
                    {
                        currentControl = childControl;
                    }
                    else
                    {

                        notFoundDescriptor = descriptor;
                        failed = true;
                        break;
                    }

                }

                if (failed && (currentControl is Form || currentControl is UserControl))
                {

                    failed = false;

                    var form = currentControl;

                    var currentFields = form.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                    //var description = Location.GetRelativePath(lastFoundDescriptor);

                    var descriptor = notFoundDescriptor;

                    if (descriptor != null)
                    {

                        foreach (var field in currentFields)
                        {

                            var childComponent = field.GetValue(form);

                            if (childComponent != null)
                            {
                                var properties = GetProperties(childComponent, descriptor.RequiredProperties);

                                if (!properties.HasProperty("Name"))
                                {
                                    properties.List.Add(Property.Create("Name", field.Name));
                                }

                                if (Compare(descriptor, properties))
                                {
                                    currentControl = field.GetValue(form) as Component;
                                    failed = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (failed) throw new LocationNotFoundException(Root, Target.ToString());

            }

            return currentControl;
        }

        private static Boolean TryLocate(Object Root, Descriptor Description, out Control Found)
        {

            if (Root is Control)
            {
                var root = Root as Control;

                var properties = GetProperties(root, Description.RequiredProperties);

                if (Compare(Description, properties))
                {
                    Found = root;
                    return true;

                }

                foreach (Control control in root.Controls)
                {

                    if (TryLocate(control, Description, out Found))
                    {
                        return true;
                    }
                }

                Found = null;
                return false;

            }
            else
            {
                Found = null;
                return false;
            }
        }

        public static Boolean Compare(Descriptor Description, Control Control)
        {
            return Compare(Description, GetProperties(Control, Description.RequiredProperties));
        }

        public static Boolean Compare(Descriptor Description, PropertyCollection ControlProperties)
        {
            foreach (var propertyName in Description.RequiredProperties)
            {
                var expected = Description.GetProperty(propertyName);

                foreach (var property in ControlProperties.List)
                {
                    if (property.HasValue)
                    {
                        if (property.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase)
                            && !property.Value.Equals(expected.Value, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return false;
                        }
                    }
                    else if (expected.HasValue)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static PropertyCollection GetProperties(Object Target, params String[] Names)
        {
            var propertiesCollection = new PropertyCollection();

            //var controlProperties = Path.GetType().GetProperties();

            foreach (var propertyName in Names)
            {

                if (propertyName.Equals("Type", StringComparison.InvariantCultureIgnoreCase) || propertyName.Equals("Class", StringComparison.InvariantCultureIgnoreCase))
                {
                    var newProperty = new Property(propertyName, (String)Target.GetType().Name);

                    propertiesCollection.List.Add(newProperty);

                }
                else if (propertyName.Equals("FullType", StringComparison.InvariantCultureIgnoreCase) || propertyName.Equals("FullClass", StringComparison.InvariantCultureIgnoreCase))
                {
                    var newProperty = new Property(propertyName, (String)Target.GetType().FullName);

                    propertiesCollection.List.Add(newProperty);
                }
                else
                {

                    var property = Target.GetType().GetProperty(propertyName);

                    if (property != null && Names.Contains<String>(property.Name) || Names == null)
                    {
                        var value = (String)property.GetValue(Target, null);

                        var newProperty = Property.Create(property.Name, value);

                        propertiesCollection.List.Add(newProperty);
                    }
                }
            }

            return propertiesCollection;
        }

    }

    [Serializable]
    public class WindowWalkerException : Exception
    {

        protected WindowWalkerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }

        public WindowWalkerException(String Message) :
            base(Message)
        {

        }

        public WindowWalkerException(String Message, Exception Inner) :
            base(Message, Inner)
        {

        }

    }

    [Serializable]
    public class LocationNotFoundException : WindowWalkerException
    {

        protected LocationNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }

        public LocationNotFoundException(Control Root, String Path) :
            base(String.Format("Window Walker Can't Locate Control!\r\nPath: {0}\r\nRoot: {1}", Path, Root.GetType().FullName))
        {

        }


    }

}
