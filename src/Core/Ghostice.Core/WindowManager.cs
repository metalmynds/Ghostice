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
using Anotar.Log4Net;

namespace Ghostice.Core
{
    public static class WindowManager
    {

        private static Process _process;

        private static IntPtr _hostMainWindow;

        static WindowManager()
        {

            _process = Process.GetCurrentProcess();

            _hostMainWindow = _process.MainWindowHandle;

            if (_hostMainWindow == IntPtr.Zero)
            {
                LogTo.Error("Failed to Get Window Manager Host Process Main Window Handle!");
            }

        }        

        public static List<Control> GetWindowControls()
        {
            var windows = new List<Control>();

            var handles = EnumerateWindowHandles();

            // DO NOT REMOVE

            windows.Add(Control.FromHandle(_process.MainWindowHandle));

            // DO NOT REMOVE (Causes unit test failure trying to find nested control

            //var handles = EnumChildWindowHandles(Process.GetCurrentProcess().MainWindowHandle);

            foreach (var hwnd in handles)
            {
                var window = Control.FromHandle(hwnd);

                if (window != null)
                { windows.Add(window); }
            }

            return windows;
        }

        public static List<Control> GetWindowChildControls(Control Parent)
        {
            var controls = new List<Control>();

            var handles = EnumChildWindowHandles(Parent.Handle);

            //var handles = EnumChildWindowHandles(Process.GetCurrentProcess().MainWindowHandle);

            foreach (var hwnd in handles)
            {
                var window = Control.FromChildHandle(hwnd);

                if (window != null)
                { controls.Add(window); }
            }

            return controls;
        }

        private static IEnumerable<IntPtr> EnumerateWindowHandles()
        {
            var handles = new List<IntPtr>();

            foreach (ProcessThread thread in _process.Threads)
                NativeMethods.EnumThreadWindows(thread.Id,
                    (hWnd, lParam) => { if (hWnd != _hostMainWindow) handles.Add(hWnd); return true; }, IntPtr.Zero);

            return handles;
        }

        private static List<IntPtr> EnumChildWindowHandles(IntPtr MainWindowHandle)
        {
            List<IntPtr> childHandles = new List<IntPtr>();

            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                NativeMethods.EnumWindowProc childProc = new NativeMethods.EnumWindowProc(EnumWindow);
                NativeMethods.EnumChildWindows(MainWindowHandle, childProc, pointerChildHandlesList);
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
            : base(info, context) { }


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

