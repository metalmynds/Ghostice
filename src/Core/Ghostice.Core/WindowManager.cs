using Newtonsoft.Json;
using Ghostice.Core.Serialisation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.Serialization;
using Anotar.NLog;

namespace Ghostice.Core
{
    public static class WindowManager
    {

        delegate List<Control> UIThreadSafeGetChildControls(Control parent);

        delegate List<Control> UIThreadSafeGetOwnedWindows(Control owner);

        static WindowManager()
        {

            

        }

        public static Object GetNestedControlPropertyValue(Control root, String name)
        {

            var property = root.GetType().GetProperty(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            if (property != null)
            {
                var value = property.GetValue(root, null);

                return value;

            }
            else
            {
                foreach (Control subControl in root.Controls)
                {
                    var subValue = GetNestedControlPropertyValue(subControl, name);

                    if (subValue != null)
                    {
                        return subValue;
                    }
                }
            }

            return null;
        }

        public static List<Control> GetDesktopWindowControls()
        {
            var windows = new List<Control>();

            var handles = EnumerateDesktopWindowHandles();

            foreach (var hwnd in handles)
            {
                var window = Control.FromChildHandle(hwnd);

                if (window != null)
                {
                    windows.Add(window);
                }
            }

            return windows;
        }

        public static List<Control> GetOwnedWindows(Control owner)
        {

            if (owner.InvokeRequired)
            {

                return
                    (List<Control>) owner.Invoke(new UIThreadSafeGetOwnedWindows(GetOwnedWindows), new Object[] {owner});

            }

            var windows = new List<Control>();

            var handles = EnumerateOwnerWindowHandles(owner.Handle);

            foreach (var hwnd in handles)
            {
                var window = Control.FromChildHandle(hwnd);

                if (window != null)
                {
                    windows.Add(window);
                }
            }

            return windows;
        }

        public static List<Control> GetApplicationWindows()
        {
            var windows = new List<Control>();

            var handles = EnumerateProcessWindowHandles();

            foreach (var hwnd in handles)
            {
                var window = Control.FromHandle(hwnd);

                if (window != null)
                {
                    windows.Add(window);
                }
            }

            return windows;
        }

        public static List<Control> GetWindowsChildren(Control child)
        {
            var controls = new List<Control>();

            try
            {
                if (child != null && !child.IsDisposed)
                {
                    if (child.InvokeRequired)
                    {
                        return (List<Control>)child.Invoke(new UIThreadSafeGetChildControls(GetWindowsChildren), new Object[] { child });
                    }

                    var handles = EnumChildWindowHandles(child.Handle);

                    foreach (var hwnd in handles)
                    {
                        var window = Control.FromChildHandle(hwnd);

                        if (window != null)
                        { controls.Add(window); }
                    }
                }
            }
            catch (ObjectDisposedException ex)
            {
                // Ignore as control has been destroyed!
            }


            return controls;
        }

        private static IEnumerable<IntPtr> EnumerateDesktopWindowHandles()
        {
            var handles = new List<IntPtr>();

            NativeMethods.EnumDesktopWindows(IntPtr.Zero, 
                (hWnd, lParam) => { handles.Add(hWnd); return true; }
            , IntPtr.Zero);

            return handles;
        }

        private static IEnumerable<IntPtr> EnumerateOwnerWindowHandles(IntPtr ownerHandle)
        {
            var handles = new List<IntPtr>();

            NativeMethods.EnumDesktopWindows(IntPtr.Zero,
                (hWnd, lParam) => 
                {
                    if ((NativeMethods.GetWindow(hWnd, NativeMethods.GetWindowCommand.GW_OWNER)) == ownerHandle)
                    {
                        handles.Add(hWnd);
                    }
                    return true;
                }
            , IntPtr.Zero);

            return handles;
        }

        private static IEnumerable<IntPtr> EnumerateProcessWindowHandles()
        {
            var handles = new List<IntPtr>();

            foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
                NativeMethods.EnumThreadWindows(thread.Id,
                    (hWnd, lParam) => { /*if (hWnd != _hostMainWindow)*/ handles.Add(hWnd); return true; }, IntPtr.Zero);

            return handles;
        }

        private static List<IntPtr> EnumChildWindowHandles(IntPtr TopLevelHandle)
        {
            List<IntPtr> childHandles = new List<IntPtr>();

            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                NativeMethods.EnumWindowProc childProc = new NativeMethods.EnumWindowProc(EnumWindow);
                NativeMethods.EnumChildWindows(TopLevelHandle, childProc, pointerChildHandlesList);
            }
            finally
            {
                gcChildhandlesList.Free();
            }

            return childHandles;
        }

        private static bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

            if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
            {
                return false;
            }

            List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
            childHandles.Add(hWnd);

            return true;
        }

        public static String ToYaml(Control Target, Boolean Recursive)
        {


            var serializer = new Serializer();

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, Target);

                writer.Flush();

                return writer.ToString();
            }
        }


        public static String ToJson(Control Target)
        {

            var jsonResolver = new IgnorableSerializerContractResolver();

            jsonResolver.Ignore(typeof(AccessibleObject));

            var jsonSettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, ContractResolver = jsonResolver };

            var serialised = JsonConvert.SerializeObject(Target, jsonSettings);

            return serialised;
        }
    }

    [Serializable]
    public class WindowManagerException : Exception
    {

        protected WindowManagerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }


        public WindowManagerException(String Message) :
            base(Message)
        {

        }

        public WindowManagerException(String Message, Exception Inner) :
            base(Message, Inner)
        {

        }
    }

}

