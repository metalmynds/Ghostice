using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    internal static class NativeMethods
    {

        internal delegate bool EnumDesktopDelegate(IntPtr hWnd, int lParam);

        internal delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        internal delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDesktopDelegate lpEnumCallbackFunction, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn,
            IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
        internal static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32", CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCommand uCmd);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int GetWindowLong(IntPtr hWnd, WindowLongFlags nIndex);

        internal enum GetWindowCommand : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        internal const uint WS_OVERLAPPED = 0x00000000;
        internal const uint WS_POPUP = 0x80000000;
        internal const uint WS_CHILD = 0x40000000;
        internal const uint WS_MINIMIZE = 0x20000000;
        internal const uint WS_VISIBLE = 0x10000000;
        internal const uint WS_DISABLED = 0x08000000;
        internal const uint WS_CLIPSIBLINGS = 0x04000000;
        internal const uint WS_CLIPCHILDREN = 0x02000000;
        internal const uint WS_MAXIMIZE = 0x01000000;
        internal const uint WS_CAPTION = 0x00C00000;     /* WS_BORDER | WS_DLGFRAME  */
        internal const uint WS_BORDER = 0x00800000;
        internal const uint WS_DLGFRAME = 0x00400000;
        internal const uint WS_VSCROLL = 0x00200000;
        internal const uint WS_HSCROLL = 0x00100000;
        internal const uint WS_SYSMENU = 0x00080000;
        internal const uint WS_THICKFRAME = 0x00040000;
        internal const uint WS_GROUP = 0x00020000;
        internal const uint WS_TABSTOP = 0x00010000;

        internal const uint WS_MINIMIZEBOX = 0x00020000;
        internal const uint WS_MAXIMIZEBOX = 0x00010000;

        internal const uint WS_TILED = WS_OVERLAPPED;
        internal const uint WS_ICONIC = WS_MINIMIZE;
        internal const uint WS_SIZEBOX = WS_THICKFRAME;
        internal const uint WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW;

        // Common Window Styles

        internal const uint WS_OVERLAPPEDWINDOW =
            (WS_OVERLAPPED |
              WS_CAPTION |
              WS_SYSMENU |
              WS_THICKFRAME |
              WS_MINIMIZEBOX |
              WS_MAXIMIZEBOX);

        internal const uint WS_POPUPWINDOW =
            (WS_POPUP |
              WS_BORDER |
              WS_SYSMENU);

        internal const uint WS_CHILDWINDOW = WS_CHILD;

        //Extended Window Styles

        internal const uint WS_EX_DLGMODALFRAME = 0x00000001;
        internal const uint WS_EX_NOPARENTNOTIFY = 0x00000004;
        internal const uint WS_EX_TOPMOST = 0x00000008;
        internal const uint WS_EX_ACCEPTFILES = 0x00000010;
        internal const uint WS_EX_TRANSPARENT = 0x00000020;

        //#if(WINVER >= 0x0400)
        internal const uint WS_EX_MDICHILD = 0x00000040;
        internal const uint WS_EX_TOOLWINDOW = 0x00000080;
        internal const uint WS_EX_WINDOWEDGE = 0x00000100;
        internal const uint WS_EX_CLIENTEDGE = 0x00000200;
        internal const uint WS_EX_CONTEXTHELP = 0x00000400;

        internal const uint WS_EX_RIGHT = 0x00001000;
        internal const uint WS_EX_LEFT = 0x00000000;
        internal const uint WS_EX_RTLREADING = 0x00002000;
        internal const uint WS_EX_LTRREADING = 0x00000000;
        internal const uint WS_EX_LEFTSCROLLBAR = 0x00004000;
        internal const uint WS_EX_RIGHTSCROLLBAR = 0x00000000;

        internal const uint WS_EX_CONTROLPARENT = 0x00010000;
        internal const uint WS_EX_STATICEDGE = 0x00020000;
        internal const uint WS_EX_APPWINDOW = 0x00040000;

        internal const uint WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
        internal const uint WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
        //#endif /* WINVER >= 0x0400 */

        //#if(_WIN32_WINNT >= 0x0500)
        internal const uint WS_EX_LAYERED = 0x00080000;
        //#endif /* _WIN32_WINNT >= 0x0500 */

        //#if(WINVER >= 0x0500)
        internal const uint WS_EX_NOINHERITLAYOUT = 0x00100000; // Disable inheritence of mirroring by children
        internal const uint WS_EX_LAYOUTRTL = 0x00400000; // Right to left mirroring
                                                        //#endif /* WINVER >= 0x0500 */

        //#if(_WIN32_WINNT >= 0x0500)
        internal const uint WS_EX_COMPOSITED = 0x02000000;
        internal const uint WS_EX_NOACTIVATE = 0x08000000;
        //#endif /* _WIN32_WINNT >= 0x0500 */
 
        internal enum WindowLongFlags : int
        {
            GWL_EXSTYLE = -20,
            GWLP_HINSTANCE = -6,
            GWLP_HWNDPARENT = -8,
            GWL_ID = -12,
            GWL_STYLE = -16,
            GWL_USERDATA = -21,
            GWL_WNDPROC = -4,
            DWLP_USER = 0x8,
            DWLP_MSGRESULT = 0x0,
            DWLP_DLGPROC = 0x4
        }

        internal static IEnumerable<IntPtr> EnumerateDesktopWindowHandles()
        {
            var handles = new List<IntPtr>();

            NativeMethods.EnumDesktopWindows(IntPtr.Zero,
                (hWnd, lParam) => { handles.Add(hWnd); return true; }
            , IntPtr.Zero);

            return handles;
        }

        internal static IEnumerable<IntPtr> EnumerateOwnerWindowHandles(IntPtr ownerHandle)
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

        internal static IEnumerable<IntPtr> EnumerateProcessWindowHandles()
        {
            var handles = new List<IntPtr>();

            foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
                NativeMethods.EnumThreadWindows(thread.Id,
                    (hWnd, lParam) => { /*if (hWnd != _hostMainWindow)*/ handles.Add(hWnd); return true; }, IntPtr.Zero);

            return handles;
        }

        internal static List<IntPtr> EnumChildWindowHandles(IntPtr TopLevelHandle)
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

        internal static bool EnumWindow(IntPtr hWnd, IntPtr lParam)
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

    }
}
