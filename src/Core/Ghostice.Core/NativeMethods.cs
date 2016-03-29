using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Security;
using System.IO;
using Accessibility;

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

        [DllImport("user32", CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCommand uCmd);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        internal static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        internal static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        // This static method is required because Win32 does not support
        // GetWindowLongPtr directly
        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return GetWindowLongPtr32(hWnd, nIndex);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

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

        /// <summary>
        /// Windows Messages
        /// Defined in winuser.h from Windows SDK v6.1
        /// Documentantion pulled from MSDN.
        /// </summary>
        internal enum WindowMessage : uint
        {
            /// <summary>
            /// The WM_NULL message performs no operation. An application sends the WM_NULL message if it wants to post a message that the recipient window will ignore.
            /// </summary>
            NULL = 0x0000,
            /// <summary>
            /// The WM_CREATE message is sent when an application requests that a window be created by calling the CreateWindowEx or CreateWindow function. (The message is sent before the function returns.) The window procedure of the new window receives this message after the window is created, but before the window becomes visible.
            /// </summary>
            CREATE = 0x0001,
            /// <summary>
            /// The WM_DESTROY message is sent when a window is being destroyed. It is sent to the window procedure of the window being destroyed after the window is removed from the screen. 
            /// This message is sent first to the window being destroyed and then to the child windows (if any) as they are destroyed. During the processing of the message, it can be assumed that all child windows still exist.
            /// /// </summary>
            DESTROY = 0x0002,
            /// <summary>
            /// The WM_MOVE message is sent after a window has been moved. 
            /// </summary>
            MOVE = 0x0003,
            /// <summary>
            /// The WM_SIZE message is sent to a window after its size has changed.
            /// </summary>
            SIZE = 0x0005,
            /// <summary>
            /// The WM_ACTIVATE message is sent to both the window being activated and the window being deactivated. If the windows use the same input queue, the message is sent synchronously, first to the window procedure of the top-level window being deactivated, then to the window procedure of the top-level window being activated. If the windows use different input queues, the message is sent asynchronously, so the window is activated immediately. 
            /// </summary>
            ACTIVATE = 0x0006,
            /// <summary>
            /// The WM_SETFOCUS message is sent to a window after it has gained the keyboard focus. 
            /// </summary>
            SETFOCUS = 0x0007,
            /// <summary>
            /// The WM_KILLFOCUS message is sent to a window immediately before it loses the keyboard focus. 
            /// </summary>
            KILLFOCUS = 0x0008,
            /// <summary>
            /// The WM_ENABLE message is sent when an application changes the enabled state of a window. It is sent to the window whose enabled state is changing. This message is sent before the EnableWindow function returns, but after the enabled state (WS_DISABLED style bit) of the window has changed. 
            /// </summary>
            ENABLE = 0x000A,
            /// <summary>
            /// An application sends the WM_SETREDRAW message to a window to allow changes in that window to be redrawn or to prevent changes in that window from being redrawn. 
            /// </summary>
            SETREDRAW = 0x000B,
            /// <summary>
            /// An application sends a WM_SETTEXT message to set the text of a window. 
            /// </summary>
            SETTEXT = 0x000C,
            /// <summary>
            /// An application sends a WM_GETTEXT message to copy the text that corresponds to a window into a buffer provided by the caller. 
            /// </summary>
            GETTEXT = 0x000D,
            /// <summary>
            /// An application sends a WM_GETTEXTLENGTH message to determine the length, in characters, of the text associated with a window. 
            /// </summary>
            GETTEXTLENGTH = 0x000E,
            /// <summary>
            /// The WM_PAINT message is sent when the system or another application makes a request to paint a portion of an application's window. The message is sent when the UpdateWindow or RedrawWindow function is called, or by the DispatchMessage function when the application obtains a WM_PAINT message by using the GetMessage or PeekMessage function. 
            /// </summary>
            PAINT = 0x000F,
            /// <summary>
            /// The WM_CLOSE message is sent as a signal that a window or an application should terminate.
            /// </summary>
            CLOSE = 0x0010,
            /// <summary>
            /// The WM_QUERYENDSESSION message is sent when the user chooses to end the session or when an application calls one of the system shutdown functions. If any application returns zero, the session is not ended. The system stops sending WM_QUERYENDSESSION messages as soon as one application returns zero.
            /// After processing this message, the system sends the WM_ENDSESSION message with the wParam parameter set to the results of the WM_QUERYENDSESSION message.
            /// </summary>
            QUERYENDSESSION = 0x0011,
            /// <summary>
            /// The WM_QUERYOPEN message is sent to an icon when the user requests that the window be restored to its previous size and position.
            /// </summary>
            QUERYOPEN = 0x0013,
            /// <summary>
            /// The WM_ENDSESSION message is sent to an application after the system processes the results of the WM_QUERYENDSESSION message. The WM_ENDSESSION message informs the application whether the session is ending.
            /// </summary>
            ENDSESSION = 0x0016,
            /// <summary>
            /// The WM_QUIT message indicates a request to terminate an application and is generated when the application calls the PostQuitMessage function. It causes the GetMessage function to return zero.
            /// </summary>
            QUIT = 0x0012,
            /// <summary>
            /// The WM_ERASEBKGND message is sent when the window background must be erased (for example, when a window is resized). The message is sent to prepare an invalidated portion of a window for painting. 
            /// </summary>
            ERASEBKGND = 0x0014,
            /// <summary>
            /// This message is sent to all top-level windows when a change is made to a system color setting. 
            /// </summary>
            SYSCOLORCHANGE = 0x0015,
            /// <summary>
            /// The WM_SHOWWINDOW message is sent to a window when the window is about to be hidden or shown.
            /// </summary>
            SHOWWINDOW = 0x0018,
            /// <summary>
            /// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.
            /// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.
            /// </summary>
            WININICHANGE = 0x001A,
            /// <summary>
            /// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.
            /// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.
            /// </summary>
            SETTINGCHANGE = WININICHANGE,
            /// <summary>
            /// The WM_DEVMODECHANGE message is sent to all top-level windows whenever the user changes device-mode settings. 
            /// </summary>
            DEVMODECHANGE = 0x001B,
            /// <summary>
            /// The WM_ACTIVATEAPP message is sent when a window belonging to a different application than the active window is about to be activated. The message is sent to the application whose window is being activated and to the application whose window is being deactivated.
            /// </summary>
            ACTIVATEAPP = 0x001C,
            /// <summary>
            /// An application sends the WM_FONTCHANGE message to all top-level windows in the system after changing the pool of font resources. 
            /// </summary>
            FONTCHANGE = 0x001D,
            /// <summary>
            /// A message that is sent whenever there is a change in the system time.
            /// </summary>
            TIMECHANGE = 0x001E,
            /// <summary>
            /// The WM_CANCELMODE message is sent to cancel certain modes, such as mouse capture. For example, the system sends this message to the active window when a dialog box or message box is displayed. Certain functions also send this message explicitly to the specified window regardless of whether it is the active window. For example, the EnableWindow function sends this message when disabling the specified window.
            /// </summary>
            CANCELMODE = 0x001F,
            /// <summary>
            /// The WM_SETCURSOR message is sent to a window if the mouse causes the cursor to move within a window and mouse input is not captured. 
            /// </summary>
            SETCURSOR = 0x0020,
            /// <summary>
            /// The WM_MOUSEACTIVATE message is sent when the cursor is in an inactive window and the user presses a mouse button. The parent window receives this message only if the child window passes it to the DefWindowProc function.
            /// </summary>
            MOUSEACTIVATE = 0x0021,
            /// <summary>
            /// The WM_CHILDACTIVATE message is sent to a child window when the user clicks the window's title bar or when the window is activated, moved, or sized.
            /// </summary>
            CHILDACTIVATE = 0x0022,
            /// <summary>
            /// The WM_QUEUESYNC message is sent by a computer-based training (CBT) application to separate user-input messages from other messages sent through the WH_JOURNALPLAYBACK Hook procedure. 
            /// </summary>
            QUEUESYNC = 0x0023,
            /// <summary>
            /// The WM_GETMINMAXINFO message is sent to a window when the size or position of the window is about to change. An application can use this message to override the window's default maximized size and position, or its default minimum or maximum tracking size. 
            /// </summary>
            GETMINMAXINFO = 0x0024,
            /// <summary>
            /// Windows NT 3.51 and earlier: The WM_PAINTICON message is sent to a minimized window when the icon is to be painted. This message is not sent by newer versions of Microsoft Windows, except in unusual circumstances explained in the Remarks.
            /// </summary>
            PAINTICON = 0x0026,
            /// <summary>
            /// Windows NT 3.51 and earlier: The WM_ICONERASEBKGND message is sent to a minimized window when the background of the icon must be filled before painting the icon. A window receives this message only if a class icon is defined for the window; otherwise, WM_ERASEBKGND is sent. This message is not sent by newer versions of Windows.
            /// </summary>
            ICONERASEBKGND = 0x0027,
            /// <summary>
            /// The WM_NEXTDLGCTL message is sent to a dialog box procedure to set the keyboard focus to a different control in the dialog box. 
            /// </summary>
            NEXTDLGCTL = 0x0028,
            /// <summary>
            /// The WM_SPOOLERSTATUS message is sent from Print Manager whenever a job is added to or removed from the Print Manager queue. 
            /// </summary>
            SPOOLERSTATUS = 0x002A,
            /// <summary>
            /// The WM_DRAWITEM message is sent to the parent window of an owner-drawn button, combo box, list box, or menu when a visual aspect of the button, combo box, list box, or menu has changed.
            /// </summary>
            DRAWITEM = 0x002B,
            /// <summary>
            /// The WM_MEASUREITEM message is sent to the owner window of a combo box, list box, list view control, or menu item when the control or menu is created.
            /// </summary>
            MEASUREITEM = 0x002C,
            /// <summary>
            /// Sent to the owner of a list box or combo box when the list box or combo box is destroyed or when items are removed by the LB_DELETESTRING, LB_RESETCONTENT, CB_DELETESTRING, or CB_RESETCONTENT message. The system sends a WM_DELETEITEM message for each deleted item. The system sends the WM_DELETEITEM message for any deleted list box or combo box item with nonzero item data.
            /// </summary>
            DELETEITEM = 0x002D,
            /// <summary>
            /// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_KEYDOWN message. 
            /// </summary>
            VKEYTOITEM = 0x002E,
            /// <summary>
            /// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_CHAR message. 
            /// </summary>
            CHARTOITEM = 0x002F,
            /// <summary>
            /// An application sends a WM_SETFONT message to specify the font that a control is to use when drawing text. 
            /// </summary>
            SETFONT = 0x0030,
            /// <summary>
            /// An application sends a WM_GETFONT message to a control to retrieve the font with which the control is currently drawing its text. 
            /// </summary>
            GETFONT = 0x0031,
            /// <summary>
            /// An application sends a WM_SETHOTKEY message to a window to associate a hot key with the window. When the user presses the hot key, the system activates the window. 
            /// </summary>
            SETHOTKEY = 0x0032,
            /// <summary>
            /// An application sends a WM_GETHOTKEY message to determine the hot key associated with a window. 
            /// </summary>
            GETHOTKEY = 0x0033,
            /// <summary>
            /// The WM_QUERYDRAGICON message is sent to a minimized (iconic) window. The window is about to be dragged by the user but does not have an icon defined for its class. An application can return a handle to an icon or cursor. The system displays this cursor or icon while the user drags the icon.
            /// </summary>
            QUERYDRAGICON = 0x0037,
            /// <summary>
            /// The system sends the WM_COMPAREITEM message to determine the relative position of a new item in the sorted list of an owner-drawn combo box or list box. Whenever the application adds a new item, the system sends this message to the owner of a combo box or list box created with the CBS_SORT or LBS_SORT style. 
            /// </summary>
            COMPAREITEM = 0x0039,
            /// <summary>
            /// Active Accessibility sends the WM_GETOBJECT message to obtain information about an accessible object contained in a server application. 
            /// Applications never send this message directly. It is sent only by Active Accessibility in response to calls to AccessibleObjectFromPoint, AccessibleObjectFromEvent, or AccessibleObjectFromWindow. However, server applications handle this message. 
            /// </summary>
            GETOBJECT = 0x003D,
            /// <summary>
            /// The WM_COMPACTING message is sent to all top-level windows when the system detects more than 12.5 percent of system time over a 30- to 60-second interval is being spent compacting memory. This indicates that system memory is low.
            /// </summary>
            COMPACTING = 0x0041,
            /// <summary>
            /// WM_COMMNOTIFY is Obsolete for Win32-Based Applications
            /// </summary>
            [Obsolete]
            COMMNOTIFY = 0x0044,
            /// <summary>
            /// The WM_WINDOWPOSCHANGING message is sent to a window whose size, position, or place in the Z order is about to change as a result of a call to the SetWindowPos function or another window-management function.
            /// </summary>
            WINDOWPOSCHANGING = 0x0046,
            /// <summary>
            /// The WM_WINDOWPOSCHANGED message is sent to a window whose size, position, or place in the Z order has changed as a result of a call to the SetWindowPos function or another window-management function.
            /// </summary>
            WINDOWPOSCHANGED = 0x0047,
            /// <summary>
            /// Notifies applications that the system, typically a battery-powered personal computer, is about to enter a suspended mode.
            /// Use: POWERBROADCAST
            /// </summary>
            [Obsolete]
            POWER = 0x0048,
            /// <summary>
            /// An application sends the WM_COPYDATA message to pass data to another application. 
            /// </summary>
            COPYDATA = 0x004A,
            /// <summary>
            /// The WM_CANCELJOURNAL message is posted to an application when a user cancels the application's journaling activities. The message is posted with a NULL window handle. 
            /// </summary>
            CANCELJOURNAL = 0x004B,
            /// <summary>
            /// Sent by a common control to its parent window when an event has occurred or the control requires some information. 
            /// </summary>
            NOTIFY = 0x004E,
            /// <summary>
            /// The WM_INPUTLANGCHANGEREQUEST message is posted to the window with the focus when the user chooses a new input language, either with the hotkey (specified in the Keyboard control panel application) or from the indicator on the system taskbar. An application can accept the change by passing the message to the DefWindowProc function or reject the change (and prevent it from taking place) by returning immediately. 
            /// </summary>
            INPUTLANGCHANGEREQUEST = 0x0050,
            /// <summary>
            /// The WM_INPUTLANGCHANGE message is sent to the topmost affected window after an application's input language has been changed. You should make any application-specific settings and pass the message to the DefWindowProc function, which passes the message to all first-level child windows. These child windows can pass the message to DefWindowProc to have it pass the message to their child windows, and so on. 
            /// </summary>
            INPUTLANGCHANGE = 0x0051,
            /// <summary>
            /// Sent to an application that has initiated a training card with Microsoft Windows Help. The message informs the application when the user clicks an authorable button. An application initiates a training card by specifying the HELP_TCARD command in a call to the WinHelp function.
            /// </summary>
            TCARD = 0x0052,
            /// <summary>
            /// Indicates that the user pressed the F1 key. If a menu is active when F1 is pressed, WM_HELP is sent to the window associated with the menu; otherwise, WM_HELP is sent to the window that has the keyboard focus. If no window has the keyboard focus, WM_HELP is sent to the currently active window. 
            /// </summary>
            HELP = 0x0053,
            /// <summary>
            /// The WM_USERCHANGED message is sent to all windows after the user has logged on or off. When the user logs on or off, the system updates the user-specific settings. The system sends this message immediately after updating the settings.
            /// </summary>
            USERCHANGED = 0x0054,
            /// <summary>
            /// Determines if a window accepts ANSI or Unicode structures in the WM_NOTIFY notification message. WM_NOTIFYFORMAT messages are sent from a common control to its parent window and from the parent window to the common control.
            /// </summary>
            NOTIFYFORMAT = 0x0055,
            /// <summary>
            /// The WM_CONTEXTMENU message notifies a window that the user clicked the right mouse button (right-clicked) in the window.
            /// </summary>
            CONTEXTMENU = 0x007B,
            /// <summary>
            /// The WM_STYLECHANGING message is sent to a window when the SetWindowLong function is about to change one or more of the window's styles.
            /// </summary>
            STYLECHANGING = 0x007C,
            /// <summary>
            /// The WM_STYLECHANGED message is sent to a window after the SetWindowLong function has changed one or more of the window's styles
            /// </summary>
            STYLECHANGED = 0x007D,
            /// <summary>
            /// The WM_DISPLAYCHANGE message is sent to all windows when the display resolution has changed.
            /// </summary>
            DISPLAYCHANGE = 0x007E,
            /// <summary>
            /// The WM_GETICON message is sent to a window to retrieve a handle to the large or small icon associated with a window. The system displays the large icon in the ALT+TAB dialog, and the small icon in the window caption. 
            /// </summary>
            GETICON = 0x007F,
            /// <summary>
            /// An application sends the WM_SETICON message to associate a new large or small icon with a window. The system displays the large icon in the ALT+TAB dialog box, and the small icon in the window caption. 
            /// </summary>
            SETICON = 0x0080,
            /// <summary>
            /// The WM_NCCREATE message is sent prior to the WM_CREATE message when a window is first created.
            /// </summary>
            NCCREATE = 0x0081,
            /// <summary>
            /// The WM_NCDESTROY message informs a window that its nonclient area is being destroyed. The DestroyWindow function sends the WM_NCDESTROY message to the window following the WM_DESTROY message. WM_DESTROY is used to free the allocated memory object associated with the window. 
            /// The WM_NCDESTROY message is sent after the child windows have been destroyed. In contrast, WM_DESTROY is sent before the child windows are destroyed.
            /// </summary>
            NCDESTROY = 0x0082,
            /// <summary>
            /// The WM_NCCALCSIZE message is sent when the size and position of a window's client area must be calculated. By processing this message, an application can control the content of the window's client area when the size or position of the window changes.
            /// </summary>
            NCCALCSIZE = 0x0083,
            /// <summary>
            /// The WM_NCHITTEST message is sent to a window when the cursor moves, or when a mouse button is pressed or released. If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise, the message is sent to the window that has captured the mouse.
            /// </summary>
            NCHITTEST = 0x0084,
            /// <summary>
            /// The WM_NCPAINT message is sent to a window when its frame must be painted. 
            /// </summary>
            NCPAINT = 0x0085,
            /// <summary>
            /// The WM_NCACTIVATE message is sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.
            /// </summary>
            NCACTIVATE = 0x0086,
            /// <summary>
            /// The WM_GETDLGCODE message is sent to the window procedure associated with a control. By default, the system handles all keyboard input to the control; the system interprets certain types of keyboard input as dialog box navigation keys. To override this default behavior, the control can respond to the WM_GETDLGCODE message to indicate the types of input it wants to process itself.
            /// </summary>
            GETDLGCODE = 0x0087,
            /// <summary>
            /// The WM_SYNCPAINT message is used to synchronize painting while avoiding linking independent GUI threads.
            /// </summary>
            SYNCPAINT = 0x0088,
            /// <summary>
            /// The WM_NCMOUSEMOVE message is posted to a window when the cursor is moved within the nonclient area of the window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCMOUSEMOVE = 0x00A0,
            /// <summary>
            /// The WM_NCLBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCLBUTTONDOWN = 0x00A1,
            /// <summary>
            /// The WM_NCLBUTTONUP message is posted when the user releases the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCLBUTTONUP = 0x00A2,
            /// <summary>
            /// The WM_NCLBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCLBUTTONDBLCLK = 0x00A3,
            /// <summary>
            /// The WM_NCRBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCRBUTTONDOWN = 0x00A4,
            /// <summary>
            /// The WM_NCRBUTTONUP message is posted when the user releases the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCRBUTTONUP = 0x00A5,
            /// <summary>
            /// The WM_NCRBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCRBUTTONDBLCLK = 0x00A6,
            /// <summary>
            /// The WM_NCMBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCMBUTTONDOWN = 0x00A7,
            /// <summary>
            /// The WM_NCMBUTTONUP message is posted when the user releases the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCMBUTTONUP = 0x00A8,
            /// <summary>
            /// The WM_NCMBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCMBUTTONDBLCLK = 0x00A9,
            /// <summary>
            /// The WM_NCXBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCXBUTTONDOWN = 0x00AB,
            /// <summary>
            /// The WM_NCXBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCXBUTTONUP = 0x00AC,
            /// <summary>
            /// The WM_NCXBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCXBUTTONDBLCLK = 0x00AD,
            /// <summary>
            /// The WM_INPUT_DEVICE_CHANGE message is sent to the window that registered to receive raw input. A window receives this message through its WindowProc function.
            /// </summary>
            INPUT_DEVICE_CHANGE = 0x00FE,
            /// <summary>
            /// The WM_INPUT message is sent to the window that is getting raw input. 
            /// </summary>
            INPUT = 0x00FF,
            /// <summary>
            /// This message filters for keyboard messages.
            /// </summary>
            KEYFIRST = 0x0100,
            /// <summary>
            /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed. 
            /// </summary>
            KEYDOWN = 0x0100,
            /// <summary>
            /// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, or a keyboard key that is pressed when a window has the keyboard focus. 
            /// </summary>
            KEYUP = 0x0101,
            /// <summary>
            /// The WM_CHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_CHAR message contains the character code of the key that was pressed. 
            /// </summary>
            CHAR = 0x0102,
            /// <summary>
            /// The WM_DEADCHAR message is posted to the window with the keyboard focus when a WM_KEYUP message is translated by the TranslateMessage function. WM_DEADCHAR specifies a character code generated by a dead key. A dead key is a key that generates a character, such as the umlaut (double-dot), that is combined with another character to form a composite character. For example, the umlaut-O character (Ö) is generated by typing the dead key for the umlaut character, and then typing the O key. 
            /// </summary>
            DEADCHAR = 0x0103,
            /// <summary>
            /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user presses the F10 key (which activates the menu bar) or holds down the ALT key and then presses another key. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter. 
            /// </summary>
            SYSKEYDOWN = 0x0104,
            /// <summary>
            /// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user releases a key that was pressed while the ALT key was held down. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter. 
            /// </summary>
            SYSKEYUP = 0x0105,
            /// <summary>
            /// The WM_SYSCHAR message is posted to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. It specifies the character code of a system character key — that is, a character key that is pressed while the ALT key is down. 
            /// </summary>
            SYSCHAR = 0x0106,
            /// <summary>
            /// The WM_SYSDEADCHAR message is sent to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. WM_SYSDEADCHAR specifies the character code of a system dead key — that is, a dead key that is pressed while holding down the ALT key. 
            /// </summary>
            SYSDEADCHAR = 0x0107,
            /// <summary>
            /// The WM_UNICHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_UNICHAR message contains the character code of the key that was pressed. 
            /// The WM_UNICHAR message is equivalent to WM_CHAR, but it uses Unicode Transformation Format (UTF)-32, whereas WM_CHAR uses UTF-16. It is designed to send or post Unicode characters to ANSI windows and it can can handle Unicode Supplementary Plane characters.
            /// </summary>
            UNICHAR = 0x0109,
            /// <summary>
            /// This message filters for keyboard messages.
            /// </summary>
            KEYLAST = 0x0109,
            /// <summary>
            /// Sent immediately before the IME generates the composition string as a result of a keystroke. A window receives this message through its WindowProc function. 
            /// </summary>
            IME_STARTCOMPOSITION = 0x010D,
            /// <summary>
            /// Sent to an application when the IME ends composition. A window receives this message through its WindowProc function. 
            /// </summary>
            IME_ENDCOMPOSITION = 0x010E,
            /// <summary>
            /// Sent to an application when the IME changes composition status as a result of a keystroke. A window receives this message through its WindowProc function. 
            /// </summary>
            IME_COMPOSITION = 0x010F,
            IME_KEYLAST = 0x010F,
            /// <summary>
            /// The WM_INITDIALOG message is sent to the dialog box procedure immediately before a dialog box is displayed. Dialog box procedures typically use this message to initialize controls and carry out any other initialization tasks that affect the appearance of the dialog box. 
            /// </summary>
            INITDIALOG = 0x0110,
            /// <summary>
            /// The WM_COMMAND message is sent when the user selects a command item from a menu, when a control sends a notification message to its parent window, or when an accelerator keystroke is translated. 
            /// </summary>
            COMMAND = 0x0111,
            /// <summary>
            /// A window receives this message when the user chooses a command from the Window menu, clicks the maximize button, minimize button, restore button, close button, or moves the form. You can stop the form from moving by filtering this out.
            /// </summary>
            SYSCOMMAND = 0x0112,
            /// <summary>
            /// The WM_TIMER message is posted to the installing thread's message queue when a timer expires. The message is posted by the GetMessage or PeekMessage function. 
            /// </summary>
            TIMER = 0x0113,
            /// <summary>
            /// The WM_HSCROLL message is sent to a window when a scroll event occurs in the window's standard horizontal scroll bar. This message is also sent to the owner of a horizontal scroll bar control when a scroll event occurs in the control. 
            /// </summary>
            HSCROLL = 0x0114,
            /// <summary>
            /// The WM_VSCROLL message is sent to a window when a scroll event occurs in the window's standard vertical scroll bar. This message is also sent to the owner of a vertical scroll bar control when a scroll event occurs in the control. 
            /// </summary>
            VSCROLL = 0x0115,
            /// <summary>
            /// The WM_INITMENU message is sent when a menu is about to become active. It occurs when the user clicks an item on the menu bar or presses a menu key. This allows the application to modify the menu before it is displayed. 
            /// </summary>
            INITMENU = 0x0116,
            /// <summary>
            /// The WM_INITMENUPOPUP message is sent when a drop-down menu or submenu is about to become active. This allows an application to modify the menu before it is displayed, without changing the entire menu. 
            /// </summary>
            INITMENUPOPUP = 0x0117,
            /// <summary>
            /// The WM_MENUSELECT message is sent to a menu's owner window when the user selects a menu item. 
            /// </summary>
            MENUSELECT = 0x011F,
            /// <summary>
            /// The WM_MENUCHAR message is sent when a menu is active and the user presses a key that does not correspond to any mnemonic or accelerator key. This message is sent to the window that owns the menu. 
            /// </summary>
            MENUCHAR = 0x0120,
            /// <summary>
            /// The WM_ENTERIDLE message is sent to the owner window of a modal dialog box or menu that is entering an idle state. A modal dialog box or menu enters an idle state when no messages are waiting in its queue after it has processed one or more previous messages. 
            /// </summary>
            ENTERIDLE = 0x0121,
            /// <summary>
            /// The WM_MENURBUTTONUP message is sent when the user releases the right mouse button while the cursor is on a menu item. 
            /// </summary>
            MENURBUTTONUP = 0x0122,
            /// <summary>
            /// The WM_MENUDRAG message is sent to the owner of a drag-and-drop menu when the user drags a menu item.
            /// </summary>
            MENUDRAG = 0x0123,
            /// <summary>
            /// The WM_MENUGETOBJECT message is sent to the owner of a drag-and-drop menu when the mouse cursor enters a menu item or moves from the center of the item to the top or bottom of the item. 
            /// </summary>
            MENUGETOBJECT = 0x0124,
            /// <summary>
            /// The WM_UNINITMENUPOPUP message is sent when a drop-down menu or submenu has been destroyed. 
            /// </summary>
            UNINITMENUPOPUP = 0x0125,
            /// <summary>
            /// The WM_MENUCOMMAND message is sent when the user makes a selection from a menu. 
            /// </summary>
            MENUCOMMAND = 0x0126,
            /// <summary>
            /// An application sends the WM_CHANGEUISTATE message to indicate that the user interface (UI) state should be changed.
            /// </summary>
            CHANGEUISTATE = 0x0127,
            /// <summary>
            /// An application sends the WM_UPDATEUISTATE message to change the user interface (UI) state for the specified window and all its child windows.
            /// </summary>
            UPDATEUISTATE = 0x0128,
            /// <summary>
            /// An application sends the WM_QUERYUISTATE message to retrieve the user interface (UI) state for a window.
            /// </summary>
            QUERYUISTATE = 0x0129,
            /// <summary>
            /// The WM_CTLCOLORMSGBOX message is sent to the owner window of a message box before Windows draws the message box. By responding to this message, the owner window can set the text and background colors of the message box by using the given display device context handle. 
            /// </summary>
            CTLCOLORMSGBOX = 0x0132,
            /// <summary>
            /// An edit control that is not read-only or disabled sends the WM_CTLCOLOREDIT message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the edit control. 
            /// </summary>
            CTLCOLOREDIT = 0x0133,
            /// <summary>
            /// Sent to the parent window of a list box before the system draws the list box. By responding to this message, the parent window can set the text and background colors of the list box by using the specified display device context handle. 
            /// </summary>
            CTLCOLORLISTBOX = 0x0134,
            /// <summary>
            /// The WM_CTLCOLORBTN message is sent to the parent window of a button before drawing the button. The parent window can change the button's text and background colors. However, only owner-drawn buttons respond to the parent window processing this message. 
            /// </summary>
            CTLCOLORBTN = 0x0135,
            /// <summary>
            /// The WM_CTLCOLORDLG message is sent to a dialog box before the system draws the dialog box. By responding to this message, the dialog box can set its text and background colors using the specified display device context handle. 
            /// </summary>
            CTLCOLORDLG = 0x0136,
            /// <summary>
            /// The WM_CTLCOLORSCROLLBAR message is sent to the parent window of a scroll bar control when the control is about to be drawn. By responding to this message, the parent window can use the display context handle to set the background color of the scroll bar control. 
            /// </summary>
            CTLCOLORSCROLLBAR = 0x0137,
            /// <summary>
            /// A static control, or an edit control that is read-only or disabled, sends the WM_CTLCOLORSTATIC message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the static control. 
            /// </summary>
            CTLCOLORSTATIC = 0x0138,
            /// <summary>
            /// Use WM_MOUSEFIRST to specify the first mouse message. Use the PeekMessage() Function.
            /// </summary>
            MOUSEFIRST = 0x0200,
            /// <summary>
            /// The WM_MOUSEMOVE message is posted to a window when the cursor moves. If the mouse is not captured, the message is posted to the window that contains the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            MOUSEMOVE = 0x0200,
            /// <summary>
            /// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            LBUTTONDOWN = 0x0201,
            /// <summary>
            /// The WM_LBUTTONUP message is posted when the user releases the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            LBUTTONUP = 0x0202,
            /// <summary>
            /// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            LBUTTONDBLCLK = 0x0203,
            /// <summary>
            /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            RBUTTONDOWN = 0x0204,
            /// <summary>
            /// The WM_RBUTTONUP message is posted when the user releases the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            RBUTTONUP = 0x0205,
            /// <summary>
            /// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            RBUTTONDBLCLK = 0x0206,
            /// <summary>
            /// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            MBUTTONDOWN = 0x0207,
            /// <summary>
            /// The WM_MBUTTONUP message is posted when the user releases the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            MBUTTONUP = 0x0208,
            /// <summary>
            /// The WM_MBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            MBUTTONDBLCLK = 0x0209,
            /// <summary>
            /// The WM_MOUSEWHEEL message is sent to the focus window when the mouse wheel is rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.
            /// </summary>
            MOUSEWHEEL = 0x020A,
            /// <summary>
            /// The WM_XBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse. 
            /// </summary>
            XBUTTONDOWN = 0x020B,
            /// <summary>
            /// The WM_XBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            XBUTTONUP = 0x020C,
            /// <summary>
            /// The WM_XBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            XBUTTONDBLCLK = 0x020D,
            /// <summary>
            /// The WM_MOUSEHWHEEL message is sent to the focus window when the mouse's horizontal scroll wheel is tilted or rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.
            /// </summary>
            MOUSEHWHEEL = 0x020E,
            /// <summary>
            /// Use WM_MOUSELAST to specify the last mouse message. Used with PeekMessage() Function.
            /// </summary>
            MOUSELAST = 0x020E,
            /// <summary>
            /// The WM_PARENTNOTIFY message is sent to the parent of a child window when the child window is created or destroyed, or when the user clicks a mouse button while the cursor is over the child window. When the child window is being created, the system sends WM_PARENTNOTIFY just before the CreateWindow or CreateWindowEx function that creates the window returns. When the child window is being destroyed, the system sends the message before any processing to destroy the window takes place.
            /// </summary>
            PARENTNOTIFY = 0x0210,
            /// <summary>
            /// The WM_ENTERMENULOOP message informs an application's main window procedure that a menu modal loop has been entered. 
            /// </summary>
            ENTERMENULOOP = 0x0211,
            /// <summary>
            /// The WM_EXITMENULOOP message informs an application's main window procedure that a menu modal loop has been exited. 
            /// </summary>
            EXITMENULOOP = 0x0212,
            /// <summary>
            /// The WM_NEXTMENU message is sent to an application when the right or left arrow key is used to switch between the menu bar and the system menu. 
            /// </summary>
            NEXTMENU = 0x0213,
            /// <summary>
            /// The WM_SIZING message is sent to a window that the user is resizing. By processing this message, an application can monitor the size and position of the drag rectangle and, if needed, change its size or position. 
            /// </summary>
            SIZING = 0x0214,
            /// <summary>
            /// The WM_CAPTURECHANGED message is sent to the window that is losing the mouse capture.
            /// </summary>
            CAPTURECHANGED = 0x0215,
            /// <summary>
            /// The WM_MOVING message is sent to a window that the user is moving. By processing this message, an application can monitor the position of the drag rectangle and, if needed, change its position.
            /// </summary>
            MOVING = 0x0216,
            /// <summary>
            /// Notifies applications that a power-management event has occurred.
            /// </summary>
            POWERBROADCAST = 0x0218,
            /// <summary>
            /// Notifies an application of a change to the hardware configuration of a device or the computer.
            /// </summary>
            DEVICECHANGE = 0x0219,
            /// <summary>
            /// An application sends the WM_MDICREATE message to a multiple-document interface (MDI) client window to create an MDI child window. 
            /// </summary>
            MDICREATE = 0x0220,
            /// <summary>
            /// An application sends the WM_MDIDESTROY message to a multiple-document interface (MDI) client window to close an MDI child window. 
            /// </summary>
            MDIDESTROY = 0x0221,
            /// <summary>
            /// An application sends the WM_MDIACTIVATE message to a multiple-document interface (MDI) client window to instruct the client window to activate a different MDI child window. 
            /// </summary>
            MDIACTIVATE = 0x0222,
            /// <summary>
            /// An application sends the WM_MDIRESTORE message to a multiple-document interface (MDI) client window to restore an MDI child window from maximized or minimized size. 
            /// </summary>
            MDIRESTORE = 0x0223,
            /// <summary>
            /// An application sends the WM_MDINEXT message to a multiple-document interface (MDI) client window to activate the next or previous child window. 
            /// </summary>
            MDINEXT = 0x0224,
            /// <summary>
            /// An application sends the WM_MDIMAXIMIZE message to a multiple-document interface (MDI) client window to maximize an MDI child window. The system resizes the child window to make its client area fill the client window. The system places the child window's window menu icon in the rightmost position of the frame window's menu bar, and places the child window's restore icon in the leftmost position. The system also appends the title bar text of the child window to that of the frame window. 
            /// </summary>
            MDIMAXIMIZE = 0x0225,
            /// <summary>
            /// An application sends the WM_MDITILE message to a multiple-document interface (MDI) client window to arrange all of its MDI child windows in a tile format. 
            /// </summary>
            MDITILE = 0x0226,
            /// <summary>
            /// An application sends the WM_MDICASCADE message to a multiple-document interface (MDI) client window to arrange all its child windows in a cascade format. 
            /// </summary>
            MDICASCADE = 0x0227,
            /// <summary>
            /// An application sends the WM_MDIICONARRANGE message to a multiple-document interface (MDI) client window to arrange all minimized MDI child windows. It does not affect child windows that are not minimized. 
            /// </summary>
            MDIICONARRANGE = 0x0228,
            /// <summary>
            /// An application sends the WM_MDIGETACTIVE message to a multiple-document interface (MDI) client window to retrieve the handle to the active MDI child window. 
            /// </summary>
            MDIGETACTIVE = 0x0229,
            /// <summary>
            /// An application sends the WM_MDISETMENU message to a multiple-document interface (MDI) client window to replace the entire menu of an MDI frame window, to replace the window menu of the frame window, or both. 
            /// </summary>
            MDISETMENU = 0x0230,
            /// <summary>
            /// The WM_ENTERSIZEMOVE message is sent one time to a window after it enters the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns. 
            /// The system sends the WM_ENTERSIZEMOVE message regardless of whether the dragging of full windows is enabled.
            /// </summary>
            ENTERSIZEMOVE = 0x0231,
            /// <summary>
            /// The WM_EXITSIZEMOVE message is sent one time to a window, after it has exited the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns. 
            /// </summary>
            EXITSIZEMOVE = 0x0232,
            /// <summary>
            /// Sent when the user drops a file on the window of an application that has registered itself as a recipient of dropped files.
            /// </summary>
            DROPFILES = 0x0233,
            /// <summary>
            /// An application sends the WM_MDIREFRESHMENU message to a multiple-document interface (MDI) client window to refresh the window menu of the MDI frame window. 
            /// </summary>
            MDIREFRESHMENU = 0x0234,
            /// <summary>
            /// Sent to an application when a window is activated. A window receives this message through its WindowProc function. 
            /// </summary>
            IME_SETCONTEXT = 0x0281,
            /// <summary>
            /// Sent to an application to notify it of changes to the IME window. A window receives this message through its WindowProc function. 
            /// </summary>
            IME_NOTIFY = 0x0282,
            /// <summary>
            /// Sent by an application to direct the IME window to carry out the requested command. The application uses this message to control the IME window that it has created. To send this message, the application calls the SendMessage function with the following parameters.
            /// </summary>
            IME_CONTROL = 0x0283,
            /// <summary>
            /// Sent to an application when the IME window finds no space to extend the area for the composition window. A window receives this message through its WindowProc function. 
            /// </summary>
            IME_COMPOSITIONFULL = 0x0284,
            /// <summary>
            /// Sent to an application when the operating system is about to change the current IME. A window receives this message through its WindowProc function. 
            /// </summary>
            IME_SELECT = 0x0285,
            /// <summary>
            /// Sent to an application when the IME gets a character of the conversion result. A window receives this message through its WindowProc function. 
            /// </summary>
            IME_CHAR = 0x0286,
            /// <summary>
            /// Sent to an application to provide commands and request information. A window receives this message through its WindowProc function. 
            /// </summary>
            IME_REQUEST = 0x0288,
            /// <summary>
            /// Sent to an application by the IME to notify the application of a key press and to keep message order. A window receives this message through its WindowProc function. 
            /// </summary>
            IME_KEYDOWN = 0x0290,
            /// <summary>
            /// Sent to an application by the IME to notify the application of a key release and to keep message order. A window receives this message through its WindowProc function. 
            /// </summary>
            IME_KEYUP = 0x0291,
            /// <summary>
            /// The WM_MOUSEHOVER message is posted to a window when the cursor hovers over the client area of the window for the period of time specified in a prior call to TrackMouseEvent.
            /// </summary>
            MOUSEHOVER = 0x02A1,
            /// <summary>
            /// The WM_MOUSELEAVE message is posted to a window when the cursor leaves the client area of the window specified in a prior call to TrackMouseEvent.
            /// </summary>
            MOUSELEAVE = 0x02A3,
            /// <summary>
            /// The WM_NCMOUSEHOVER message is posted to a window when the cursor hovers over the nonclient area of the window for the period of time specified in a prior call to TrackMouseEvent.
            /// </summary>
            NCMOUSEHOVER = 0x02A0,
            /// <summary>
            /// The WM_NCMOUSELEAVE message is posted to a window when the cursor leaves the nonclient area of the window specified in a prior call to TrackMouseEvent.
            /// </summary>
            NCMOUSELEAVE = 0x02A2,
            /// <summary>
            /// The WM_WTSSESSION_CHANGE message notifies applications of changes in session state.
            /// </summary>
            WTSSESSION_CHANGE = 0x02B1,
            TABLET_FIRST = 0x02c0,
            TABLET_LAST = 0x02df,
            /// <summary>
            /// An application sends a WM_CUT message to an edit control or combo box to delete (cut) the current selection, if any, in the edit control and copy the deleted text to the clipboard in CF_TEXT format. 
            /// </summary>
            CUT = 0x0300,
            /// <summary>
            /// An application sends the WM_COPY message to an edit control or combo box to copy the current selection to the clipboard in CF_TEXT format. 
            /// </summary>
            COPY = 0x0301,
            /// <summary>
            /// An application sends a WM_PASTE message to an edit control or combo box to copy the current content of the clipboard to the edit control at the current caret position. Data is inserted only if the clipboard contains data in CF_TEXT format. 
            /// </summary>
            PASTE = 0x0302,
            /// <summary>
            /// An application sends a WM_CLEAR message to an edit control or combo box to delete (clear) the current selection, if any, from the edit control. 
            /// </summary>
            CLEAR = 0x0303,
            /// <summary>
            /// An application sends a WM_UNDO message to an edit control to undo the last operation. When this message is sent to an edit control, the previously deleted text is restored or the previously added text is deleted.
            /// </summary>
            UNDO = 0x0304,
            /// <summary>
            /// The WM_RENDERFORMAT message is sent to the clipboard owner if it has delayed rendering a specific clipboard format and if an application has requested data in that format. The clipboard owner must render data in the specified format and place it on the clipboard by calling the SetClipboardData function. 
            /// </summary>
            RENDERFORMAT = 0x0305,
            /// <summary>
            /// The WM_RENDERALLFORMATS message is sent to the clipboard owner before it is destroyed, if the clipboard owner has delayed rendering one or more clipboard formats. For the content of the clipboard to remain available to other applications, the clipboard owner must render data in all the formats it is capable of generating, and place the data on the clipboard by calling the SetClipboardData function. 
            /// </summary>
            RENDERALLFORMATS = 0x0306,
            /// <summary>
            /// The WM_DESTROYCLIPBOARD message is sent to the clipboard owner when a call to the EmptyClipboard function empties the clipboard. 
            /// </summary>
            DESTROYCLIPBOARD = 0x0307,
            /// <summary>
            /// The WM_DRAWCLIPBOARD message is sent to the first window in the clipboard viewer chain when the content of the clipboard changes. This enables a clipboard viewer window to display the new content of the clipboard. 
            /// </summary>
            DRAWCLIPBOARD = 0x0308,
            /// <summary>
            /// The WM_PAINTCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area needs repainting. 
            /// </summary>
            PAINTCLIPBOARD = 0x0309,
            /// <summary>
            /// The WM_VSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's vertical scroll bar. The owner should scroll the clipboard image and update the scroll bar values. 
            /// </summary>
            VSCROLLCLIPBOARD = 0x030A,
            /// <summary>
            /// The WM_SIZECLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area has changed size. 
            /// </summary>
            SIZECLIPBOARD = 0x030B,
            /// <summary>
            /// The WM_ASKCBFORMATNAME message is sent to the clipboard owner by a clipboard viewer window to request the name of a CF_OWNERDISPLAY clipboard format.
            /// </summary>
            ASKCBFORMATNAME = 0x030C,
            /// <summary>
            /// The WM_CHANGECBCHAIN message is sent to the first window in the clipboard viewer chain when a window is being removed from the chain. 
            /// </summary>
            CHANGECBCHAIN = 0x030D,
            /// <summary>
            /// The WM_HSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window. This occurs when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's horizontal scroll bar. The owner should scroll the clipboard image and update the scroll bar values.
            /// </summary>
            HSCROLLCLIPBOARD = 0x030E,
            /// <summary>
            /// This message informs a window that it is about to receive the keyboard focus, giving the window the opportunity to realize its logical palette when it receives the focus. 
            /// </summary>
            QUERYNEWPALETTE = 0x030F,
            /// <summary>
            /// The WM_PALETTEISCHANGING message informs applications that an application is going to realize its logical palette. 
            /// </summary>
            PALETTEISCHANGING = 0x0310,
            /// <summary>
            /// This message is sent by the OS to all top-level and overlapped windows after the window with the keyboard focus realizes its logical palette. 
            /// This message enables windows that do not have the keyboard focus to realize their logical palettes and update their client areas.
            /// </summary>
            PALETTECHANGED = 0x0311,
            /// <summary>
            /// The WM_HOTKEY message is posted when the user presses a hot key registered by the RegisterHotKey function. The message is placed at the top of the message queue associated with the thread that registered the hot key. 
            /// </summary>
            HOTKEY = 0x0312,
            /// <summary>
            /// The WM_PRINT message is sent to a window to request that it draw itself in the specified device context, most commonly in a printer device context.
            /// </summary>
            PRINT = 0x0317,
            /// <summary>
            /// The WM_PRINTCLIENT message is sent to a window to request that it draw its client area in the specified device context, most commonly in a printer device context.
            /// </summary>
            PRINTCLIENT = 0x0318,
            /// <summary>
            /// The WM_APPCOMMAND message notifies a window that the user generated an application command event, for example, by clicking an application command button using the mouse or typing an application command key on the keyboard.
            /// </summary>
            APPCOMMAND = 0x0319,
            /// <summary>
            /// The WM_THEMECHANGED message is broadcast to every window following a theme change event. Examples of theme change events are the activation of a theme, the deactivation of a theme, or a transition from one theme to another.
            /// </summary>
            THEMECHANGED = 0x031A,
            /// <summary>
            /// Sent when the contents of the clipboard have changed.
            /// </summary>
            CLIPBOARDUPDATE = 0x031D,
            /// <summary>
            /// The system will send a window the WM_DWMCOMPOSITIONCHANGED message to indicate that the availability of desktop composition has changed.
            /// </summary>
            DWMCOMPOSITIONCHANGED = 0x031E,
            /// <summary>
            /// WM_DWMNCRENDERINGCHANGED is called when the non-client area rendering status of a window has changed. Only windows that have set the flag DWM_BLURBEHIND.fTransitionOnMaximized to true will get this message. 
            /// </summary>
            DWMNCRENDERINGCHANGED = 0x031F,
            /// <summary>
            /// Sent to all top-level windows when the colorization color has changed. 
            /// </summary>
            DWMCOLORIZATIONCOLORCHANGED = 0x0320,
            /// <summary>
            /// WM_DWMWINDOWMAXIMIZEDCHANGE will let you know when a DWM composed window is maximized. You also have to register for this message as well. You'd have other windowd go opaque when this message is sent.
            /// </summary>
            DWMWINDOWMAXIMIZEDCHANGE = 0x0321,
            /// <summary>
            /// Sent to request extended title bar information. A window receives this message through its WindowProc function.
            /// </summary>
            GETTITLEBARINFOEX = 0x033F,
            HANDHELDFIRST = 0x0358,
            HANDHELDLAST = 0x035F,
            AFXFIRST = 0x0360,
            AFXLAST = 0x037F,
            PENWINFIRST = 0x0380,
            PENWINLAST = 0x038F,
            /// <summary>
            /// The WM_APP constant is used by applications to help define private messages, usually of the form WM_APP+X, where X is an integer value. 
            /// </summary>
            APP = 0x8000,
            /// <summary>
            /// The WM_USER constant is used by applications to help define private messages for use by private window classes, usually of the form WM_USER+X, where X is an integer value. 
            /// </summary>
            USER = 0x0400,

            /// <summary>
            /// An application sends the WM_CPL_LAUNCH message to Windows Control Panel to request that a Control Panel application be started. 
            /// </summary>
            CPL_LAUNCH = USER + 0x1000,
            /// <summary>
            /// The WM_CPL_LAUNCHED message is sent when a Control Panel application, started by the WM_CPL_LAUNCH message, has closed. The WM_CPL_LAUNCHED message is sent to the window identified by the wParam parameter of the WM_CPL_LAUNCH message that started the application. 
            /// </summary>
            CPL_LAUNCHED = USER + 0x1001,
            /// <summary>
            /// WM_SYSTIMER is a well-known yet still undocumented message. Windows uses WM_SYSTIMER for internal actions like scrolling.
            /// </summary>
            SYSTIMER = 0x118,

            /// <summary>
            /// The accessibility state has changed.
            /// </summary>
            HSHELL_ACCESSIBILITYSTATE = 11,
            /// <summary>
            /// The shell should activate its main window.
            /// </summary>
            HSHELL_ACTIVATESHELLWINDOW = 3,
            /// <summary>
            /// The user completed an input event (for example, pressed an application command button on the mouse or an application command key on the keyboard), and the application did not handle the WM_APPCOMMAND message generated by that input.
            /// If the Shell procedure handles the WM_COMMAND message, it should not call CallNextHookEx. See the Return Value section for more information.
            /// </summary>
            HSHELL_APPCOMMAND = 12,
            /// <summary>
            /// A window is being minimized or maximized. The system needs the coordinates of the minimized rectangle for the window.
            /// </summary>
            HSHELL_GETMINRECT = 5,
            /// <summary>
            /// Keyboard language was changed or a new keyboard layout was loaded.
            /// </summary>
            HSHELL_LANGUAGE = 8,
            /// <summary>
            /// The title of a window in the task bar has been redrawn.
            /// </summary>
            HSHELL_REDRAW = 6,
            /// <summary>
            /// The user has selected the task list. A shell application that provides a task list should return TRUE to prevent Windows from starting its task list.
            /// </summary>
            HSHELL_TASKMAN = 7,
            /// <summary>
            /// A top-level, unowned window has been created. The window exists when the system calls this hook.
            /// </summary>
            HSHELL_WINDOWCREATED = 1,
            /// <summary>
            /// A top-level, unowned window is about to be destroyed. The window still exists when the system calls this hook.
            /// </summary>
            HSHELL_WINDOWDESTROYED = 2,
            /// <summary>
            /// The activation has changed to a different top-level, unowned window.
            /// </summary>
            HSHELL_WINDOWACTIVATED = 4,
            /// <summary>
            /// A top-level window is being replaced. The window exists when the system calls this hook.
            /// </summary>
            HSHELL_WINDOWREPLACED = 13
        }

        internal enum DLGC
        {
            /// <summary> Control is a button (of any kind). </summary>
            DLGC_BUTTON = 0x2000,
            /// <summary> Control is a default push button. </summary>
            DLGC_DEFPUSHBUTTON = 0x0010,
            /// <summary> Windows will send an EM_SETSEL message to the control to select its contents. </summary>
            DLGC_HASSETSEL = 0x0008,
            /// <summary> Control is an option (radio) button. </summary>
            DLGC_RADIOBUTTON = 0x0040,
            /// <summary> Control is a static control. </summary>
            DLGC_STATIC = 0x0100,
            /// <summary> Control is a push button but not the default push button. </summary>
            DLGC_UNDEFPUSHBUTTON = 0x0020,
            /// <summary> Control processes all keyboard input. </summary>
            DLGC_WANTALLKEYS = 0x0004,
            /// <summary> Control processes arrow keys. </summary>
            DLGC_WANTARROWS = 0x0001,
            /// <summary> Control processes WM_CHAR messages. </summary>
            DLGC_WANTCHARS = 0x0080,
            /// <summary> Control processes the message in the MSG structure that lParam points to. </summary>
            DLGC_WANTMESSAGE = 0x0004,
            /// <summary> Control processes the TAB key. </summary>
            DLGC_WANTTAB = 0x0002,
        }

        // Button message 
        internal const int BM_GETCHECK = 0x00F0;
        internal const int BM_GETSTATE = 0x00F2;
        internal const int BM_SETSTATE = 0x00F3;
        //internal const int BM_CLICK = 0xF0F5;
        internal const int BM_CLICK = 0x00F5;

        // Combobox 
        internal const int CB_GETCURSEL = 0x0147;
        internal const int CB_GETLBTEXT = 0x0148;
        internal const int CB_GETLBTEXTLEN = 0x0149;
        internal const int CB_SHOWDROPDOWN = 0x014F;
        internal const int CB_GETDROPPEDSTATE = 0x0157;

        // Date/Time picker
        internal const int DTM_GETSYSTEMTIME = 0x1001;
        internal const int DTM_SETSYSTEMTIME = 0x1002;
        internal const int DTM_GETMONTHCAL = 0x1008;

        // Editbox messages 
        internal const int EM_GETSEL = 0x00B0;
        internal const int EM_SETSEL = 0x00B1;
        internal const int EM_GETRECT = 0x00B2;
        internal const int EM_LINESCROLL = 0x00B6;
        internal const int EM_GETLINECOUNT = 0x00BA;
        internal const int EM_LINEINDEX = 0x00BB;
        internal const int EM_LINEFROMCHAR = 0x00C9;
        internal const int EM_GETFIRSTVISIBLELINE = 0x00CE;
        internal const int EM_GETLIMITTEXT = 0x00D5;
        internal const int EM_POSFROMCHAR = 0x00D6;
        internal const int EM_CHARFROMPOS = 0x00D7;

        // SysHeader
        internal const int HDM_FIRST = 0x1200;
        internal const int HDM_GETITEMCOUNT = HDM_FIRST + 0;
        internal const int HDM_HITTEST = HDM_FIRST + 6;
        internal const int HDM_GETITEMRECT = HDM_FIRST + 7;
        internal const int HDM_GETITEMW = HDM_FIRST + 11;
        internal const int HDM_ORDERTOINDEX = HDM_FIRST + 15;
        internal const int HDM_GETITEMDROPDOWNRECT = HDM_FIRST + 25;
        internal const int HDM_GETFOCUSEDITEM = HDM_FIRST + 27;

        // Listbox messages
        internal const int LB_ERR = -1;
        internal const int LB_SETSEL = 0x0185;
        internal const int LB_SETCURSEL = 0x0186;
        internal const int LB_GETSEL = 0x0187;
        internal const int LB_GETCURSEL = 0x0188;
        internal const int LB_GETTEXT = 0x0189;
        internal const int LB_GETTEXTLEN = 0x018A;
        internal const int LB_GETCOUNT = 0x018B;
        internal const int LB_GETSELCOUNT = 0x0190;
        internal const int LB_SETTOPINDEX = 0x0197;
        internal const int LB_GETITEMRECT = 0x0198;
        internal const int LB_GETITEMDATA = 0x0199;
        internal const int LB_SETCARETINDEX = 0x019E;
        internal const int LB_GETCARETINDEX = 0x019F;
        internal const int LB_ITEMFROMPOINT = 0x01A9;

        // Listbox notification message
        internal const int LBN_SELCHANGE = 1;

        // List-view messages
        internal const int LVM_FIRST = 0x1000;
        internal const int LVM_GETITEMCOUNT = LVM_FIRST + 4;
        internal const int LVM_GETNEXTITEM = LVM_FIRST + 12;
        internal const int LVM_GETITEMRECT = LVM_FIRST + 14;
        internal const int LVM_GETITEMPOSITION = LVM_FIRST + 16;
        internal const int LVM_HITTEST = (LVM_FIRST + 18);
        internal const int LVM_ENSUREVISIBLE = LVM_FIRST + 19;
        internal const int LVM_SCROLL = LVM_FIRST + 20;
        internal const int LVM_GETHEADER = LVM_FIRST + 31;
        internal const int LVM_GETITEMSTATE = LVM_FIRST + 44;
        internal const int LVM_SETITEMSTATE = LVM_FIRST + 43;
        internal const int LVM_GETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 55;
        internal const int LVM_GETSUBITEMRECT = LVM_FIRST + 56;
        internal const int LVM_SUBITEMHITTEST = LVM_FIRST + 57;
        internal const int LVM_APPROXIMATEVIEWRECT = LVM_FIRST + 64;
        internal const int LVM_GETITEMW = LVM_FIRST + 75;
        internal const int LVM_GETTOOLTIPS = LVM_FIRST + 78;
        internal const int LVM_GETFOCUSEDGROUP = LVM_FIRST + 93;
        internal const int LVM_GETGROUPRECT = LVM_FIRST + 98;
        internal const int LVM_EDITLABEL = LVM_FIRST + 118;
        internal const int LVM_GETVIEW = LVM_FIRST + 143;
        internal const int LVM_SETVIEW = LVM_FIRST + 142;
        internal const int LVM_SETGROUPINFO = LVM_FIRST + 147;
        internal const int LVM_GETGROUPINFO = LVM_FIRST + 149;
        internal const int LVM_GETGROUPINFOBYINDEX = LVM_FIRST + 153;
        internal const int LVM_GETGROUPMETRICS = LVM_FIRST + 156;
        internal const int LVM_HASGROUP = LVM_FIRST + 161;
        internal const int LVM_ISGROUPVIEWENABLED = LVM_FIRST + 175;
        internal const int LVM_GETFOCUSEDCOLUMN = LVM_FIRST + 186;

        internal const int LVM_GETEMPTYTEXT = LVM_FIRST + 204;
        internal const int LVM_GETFOOTERRECT = LVM_FIRST + 205;
        internal const int LVM_GETFOOTERINFO = LVM_FIRST + 206;
        internal const int LVM_GETFOOTERITEMRECT = LVM_FIRST + 207;
        internal const int LVM_GETFOOTERITEM = LVM_FIRST + 208;
        internal const int LVM_GETITEMINDEXRECT = LVM_FIRST + 209;
        internal const int LVM_SETITEMINDEXSTATE = LVM_FIRST + 210;
        internal const int LVM_GETNEXTITEMINDEX = LVM_FIRST + 211;

        // calendar control specific constants taken from commctrl.h 
        // commctrl MONTHCAL CONTROL win messages
        internal const int MCM_FIRST = 0x1000;
        internal const int MCM_GETCURSEL = (MCM_FIRST + 1);
        internal const int MCM_SETCURSEL = (MCM_FIRST + 2);
        internal const int MCM_GETMAXSELCOUNT = (MCM_FIRST + 3);
        internal const int MCM_GETSELRANGE = (MCM_FIRST + 5);
        internal const int MCM_SETSELRANGE = (MCM_FIRST + 6);
        internal const int MCM_GETMONTHRANGE = (MCM_FIRST + 7);
        internal const int MCM_GETMINREQRECT = (MCM_FIRST + 9);
        internal const int MCM_GETTODAY = (MCM_FIRST + 13);
        internal const int MCM_HITTEST = (MCM_FIRST + 14);
        internal const int MCM_GETFIRSTDAYOFWEEK = (MCM_FIRST + 16);
        internal const int MCM_GETRANGE = (MCM_FIRST + 17);
        internal const int MCM_SETMONTHDELTA = (MCM_FIRST + 20);
        internal const int MCM_GETMAXTODAYWIDTH = (MCM_FIRST + 21);
        internal const int MCM_GETCURRENTVIEW = (MCM_FIRST + 22);
        internal const int MCM_GETCALENDARCOUNT = (MCM_FIRST + 23);
        internal const int MCM_GETCALENDARGRIDINFO = (MCM_FIRST + 24);

        // PAGER CONTROL from commctrl.h
        internal const int PGM_FIRST = 0x1400;
        internal const int PGM_SETCHILD = (PGM_FIRST + 1);
        internal const int PGM_RECALCSIZE = (PGM_FIRST + 2);
        internal const int PGM_FORWARDMOUSE = (PGM_FIRST + 3);
        internal const int PGM_SETBKCOLOR = (PGM_FIRST + 4);
        internal const int PGM_GETBKCOLOR = (PGM_FIRST + 5);
        internal const int PGM_SETBORDER = (PGM_FIRST + 6);
        internal const int PGM_GETBORDER = (PGM_FIRST + 7);
        internal const int PGM_SETPOS = (PGM_FIRST + 8);
        internal const int PGM_GETPOS = (PGM_FIRST + 9);
        internal const int PGM_SETBUTTONSIZE = (PGM_FIRST + 10);
        internal const int PGM_GETBUTTONSIZE = (PGM_FIRST + 11);
        internal const int PGM_GETBUTTONSTATE = (PGM_FIRST + 12);

        // SysTabControl32
        internal const int TCM_FIRST = 0x1300;
        internal const int TCM_GETITEMCOUNT = TCM_FIRST + 4;
        internal const int TCM_GETITEMRECT = TCM_FIRST + 10;
        internal const int TCM_GETCURSEL = TCM_FIRST + 11;
        internal const int TCM_SETCURSEL = TCM_FIRST + 12;
        internal const int TCM_HITTEST = TCM_FIRST + 13;
        internal const int TCM_GETTOOLTIPS = TCM_FIRST + 45;
        internal const int TCM_GETCURFOCUS = TCM_FIRST + 47;
        internal const int TCM_SETCURFOCUS = TCM_FIRST + 48;
        internal const int TCM_DESELECTALL = TCM_FIRST + 50;
        internal const int TCM_GETITEMW = TCM_FIRST + 60;

        // TreeView
        internal const int TV_FIRST = 0x1100;
        internal const int TVM_EXPAND = (TV_FIRST + 2);
        internal const int TVM_GETITEMRECT = (TV_FIRST + 4);
        internal const int TVM_GETCOUNT = (TV_FIRST + 5);
        internal const int TVM_GETNEXTITEM = (TV_FIRST + 10);
        internal const int TVM_SELECTITEM = (TV_FIRST + 11);
        internal const int TVM_HITTEST = (TV_FIRST + 17);
        internal const int TVM_ENSUREVISIBLE = (TV_FIRST + 20);
        internal const int TVM_ENDEDITLABELNOW = (TV_FIRST + 22);
        internal const int TVM_GETTOOLTIPS = (TV_FIRST + 25);
        internal const int TVM_GETITEMSTATE = (TV_FIRST + 39);
        internal const int TVM_MAPACCIDTOHTREEITEM = (TV_FIRST + 42);
        internal const int TVM_MAPHTREEITEMTOACCID = (TV_FIRST + 43);
        internal const int TVM_GETITEMW = (TV_FIRST + 62);
        internal const int TVM_SETITEMW = (TV_FIRST + 63);
        internal const int TVM_EDITLABELW = (TV_FIRST + 65);

        // Window
        internal const int WM_SETTEXT = 0x000C;
        internal const int WM_GETTEXT = 0x000D;
        internal const int WM_GETTEXTLENGTH = 0x000E;
        internal const int WM_QUIT = 0x0012;
        internal const int WM_GETFONT = 0x0031;
        internal const int WM_GETOBJECT = 0x003D;
        internal const int WM_NCHITTEST = 0x0084;
        internal const int WM_KEYDOWN = 0x0100;
        internal const int WM_KEYUP = 0x0101;
        internal const int WM_COMMAND = 0x0111;
        internal const int WM_SYSCOMMAND = 0x0112;
        internal const int WM_HSCROLL = 0x0114;
        internal const int WM_VSCROLL = 0x0115;
        internal const int WM_LBUTTONDOWN = 0x0201;
        internal const int WM_LBUTTONUP = 0x0202;
        internal const int WM_RBUTTONDOWN = 0x0204;
        internal const int WM_RBUTTONUP = 0x0205;
        internal const int WM_MDITILE = 0x0226;
        internal const int WM_MDICASCADE = 0x0227;
        internal const int WM_HOTKEY = 0x0312;
        internal const int WM_GETTITLEBARINFOEX = 0x033F;
        internal const int WM_USER = 0x0400;

        // Dialog Codes 
        internal const int WM_GETDLGCODE = 0x0087;
        internal const int DLGC_STATIC = 0x0100;

        // Slider 
        internal const int TBM_GETPOS = WM_USER;
        internal const int TBM_GETRANGEMIN = WM_USER + 1;
        internal const int TBM_GETRANGEMAX = WM_USER + 2;
        internal const int TBM_SETPOS = WM_USER + 5;
        internal const int TBM_GETPAGESIZE = WM_USER + 22;
        internal const int TBM_GETLINESIZE = WM_USER + 24;
        internal const int TBM_GETTHUMBRECT = WM_USER + 25;
        internal const int TBM_GETCHANNELRECT = WM_USER + 26;
        internal const int TBM_GETTOOLTIPS = WM_USER + 30;

        // Progress Bar 
        internal const int PBM_GETRANGE = (WM_USER + 7);
        internal const int PBM_GETPOS = (WM_USER + 8);

        // Status Bar
        internal const int SB_GETPARTS = (WM_USER + 6);
        internal const int SB_GETRECT = (WM_USER + 10);
        internal const int SB_GETTEXTLENGTHW = (WM_USER + 12);
        internal const int SB_GETTEXTW = (WM_USER + 13);

        // Rebar 
        internal const int RB_HITTEST = WM_USER + 8;
        internal const int RB_GETRECT = WM_USER + 9;
        internal const int RB_GETBANDCOUNT = WM_USER + 12;
        internal const int RB_GETTOOLTIPS = WM_USER + 17;
        internal const int RB_GETBANDINFOA = WM_USER + 29;
        internal const int RB_PUSHCHEVRON = WM_USER + 43;

        // ToolBar 
        internal const int TB_PRESSBUTTON = WM_USER + 3;
        internal const int TB_ISBUTTONENABLED = WM_USER + 9;
        internal const int TB_ISBUTTONCHECKED = WM_USER + 10;
        internal const int TB_ISBUTTONHIDDEN = WM_USER + 12;
        internal const int TB_GETBUTTON = WM_USER + 23;
        internal const int TB_BUTTONCOUNT = WM_USER + 24;
        internal const int TB_GETITEMRECT = WM_USER + 29;
        internal const int TB_GETTOOLTIPS = WM_USER + 35;
        internal const int TB_GETIMAGELIST = WM_USER + 49;
        internal const int TB_GETHOTITEM = WM_USER + 71;
        internal const int TB_SETHOTITEM = WM_USER + 72;
        internal const int TB_GETBUTTONTEXT = WM_USER + 75;
        internal const int TB_GETEXTENDEDSTYLE = WM_USER + 85;

        // Tooltip
        internal const int TTM_GETTOOLINFO = (WM_USER + 53);
        internal const int TTM_HITTEST = (WM_USER + 55);
        internal const int TTM_GETTEXT = (WM_USER + 56);
        internal const int TTM_GETCURRENTTOOL = (WM_USER + 59);

        // IPAddress 
        internal const int IPM_SETADDRESS = (WM_USER + 101);

        //  SpinControl
        internal const int UDM_GETRANGE = (WM_USER + 102);
        internal const int UDM_SETPOS = (WM_USER + 103);
        internal const int UDM_GETPOS = (WM_USER + 104);
        internal const int UDM_GETBUDDY = (WM_USER + 106);

        // Hyperlink 
        internal const int LM_FIRST = (WM_USER + 0x300);
        internal const int LM_HITTEST = LM_FIRST;
        internal const int LM_GETIDEALHEIGHT = (LM_FIRST + 1);
        internal const int LM_SETITEM = (LM_FIRST + 2);
        internal const int LM_GETITEM = (LM_FIRST + 3);


        // Button styles
        internal const int BS_PUSHBUTTON = 0x00000000;
        internal const int BS_DEFPUSHBUTTON = 0x00000001;
        internal const int BS_CHECKBOX = 0x00000002;
        internal const int BS_AUTOCHECKBOX = 0x00000003;
        internal const int BS_RADIOBUTTON = 0x00000004;
        internal const int BS_3STATE = 0x00000005;
        internal const int BS_AUTO3STATE = 0x00000006;
        internal const int BS_GROUPBOX = 0x00000007;
        internal const int BS_USERBUTTON = 0x00000008;
        internal const int BS_AUTORADIOBUTTON = 0x00000009;
        internal const int BS_PUSHBOX = 0x0000000A;
        internal const int BS_OWNERDRAW = 0x0000000B;
        internal const int BS_SPLITBUTTON = 0x0000000C;
        internal const int BS_TYPEMASK = 0x0000000F;

        // Date/Time picker styles 
        internal const int DTS_UPDOWN = 0x0001;
        internal const int DTS_SHOWNONE = 0x0002;
        // DTS_TIMEFORMAT is wrongly defined in the common control include file with a value of 9 
        // TIME_FORMAT + DTS_UPDOWN.
        internal const int DTS_TIMEFORMAT = 0x0009;
        // Removes the UPDOWN bit. Use this const to check for TIMEFORMAT
        internal const int DTS_TIMEFORMATONLY = DTS_TIMEFORMAT & ~DTS_UPDOWN;

        // Dialogbox Styles 
        internal const int DS_CONTROL = 0x00000400;

        // Editbox styles 
        internal const int ES_LEFT = 0x0000;
        internal const int ES_CENTER = 0x0001;
        internal const int ES_RIGHT = 0x0002;
        internal const int ES_MULTILINE = 0x0004;
        internal const int ES_UPPERCASE = 0x0008;
        internal const int ES_LOWERCASE = 0x0010;
        internal const int ES_PASSWORD = 0x0020;
        internal const int ES_AUTOHSCROLL = 0x0080;
        internal const int ES_READONLY = 0x0800;
        internal const int ES_NUMBER = 0x2000;

        // Listbox styles
        internal const int LBS_NOTIFY = 0x0001;
        internal const int LBS_SORT = 0x0002;
        internal const int LBS_MULTIPLESEL = 0x0008;
        internal const int LBS_OWNERDRAWFIXED = 0x0010;
        internal const int LBS_WANTKEYBOARDINPUT = 0x0400;
        internal const int LBS_EXTENDEDSEL = 0x0800;
        internal const int LBS_COMBOBOX = 0x8000;

        // Listview styles
        internal const int LVS_REPORT = 0x0001;
        internal const int LVS_LIST = 0x0003;
        internal const int LVS_TYPEMASK = 0x0003;
        internal const int LVS_SINGLESEL = 0x0004;
        internal const int LVS_AUTOARRANGE = 0x0100;
        internal const int LVS_EDITLABELS = 0x0200;
        internal const int LVS_NOSCROLL = 0x2000;
        internal const int LVS_NOCOLUMNHEADER = 0x4000;

        // Listview extended styles 
        internal const int LVS_EX_CHECKBOXES = 0x4;
        internal const int LVS_EX_FULLROWSELECT = 0x00000020;
        internal const int LVS_EX_ONECLICKACTIVATE = 0x00000040;
        internal const int LVS_EX_TWOCLICKACTIVATE = 0x00000080;
        internal const int LVS_EX_UNDERLINEHOT = 0x00000800;
        internal const int LVS_EX_UNDERLINECOLD = 0x00001000;
        internal const int LVS_EX_JUSTIFYCOLUMNS = 0x00200000; // Icons are lined up in columns that use up the whole view area

        // Listview item states 
        internal const int LVIS_FOCUSED = 0x0001;
        internal const int LVIS_SELECTED = 0x0002;
        internal const int LVIS_STATEIMAGEMASK = 0xFFFF;

        // commctrl MONTHCAL CONTROL style constants 
        internal const int MCS_DAYSTATE = 0x0001;
        internal const int MCS_MULTISELECT = 0x0002;
        internal const int MCS_WEEKNUMBERS = 0x0004;
        internal const int MCS_NOTODAYCIRCLE = 0x0008;
        internal const int MCS_NOTODAY = 0x0010;

        // PAGER CONTROL styles from commctrl.h 
        internal const int PGS_VERT = 0x00000000;
        internal const int PGS_HORZ = 0x00000001;

        // Scrollbar style
        internal const int SBS_HORZ = 0x0000;
        internal const int SBS_VERT = 0x0001;

        // Slider style 
        internal const int TBS_VERT = 0x0002;

        // Static styles 
        internal const int SS_LEFT = 0x00000000;
        internal const int SS_CENTER = 0x00000001;
        internal const int SS_RIGHT = 0x00000002;
        internal const int SS_ICON = 0x00000003;
        internal const int SS_BLACKRECT = 0x00000004;
        internal const int SS_GRAYRECT = 0x00000005;
        internal const int SS_WHITERECT = 0x00000006;
        internal const int SS_BLACKFRAME = 0x00000007;
        internal const int SS_GRAYFRAME = 0x00000008;
        internal const int SS_WHITEFRAME = 0x00000009;
        internal const int SS_USERITEM = 0x0000000A;
        internal const int SS_SIMPLE = 0x0000000B;
        internal const int SS_LEFTNOWORDWRAP = 0x0000000C;
        internal const int SS_OWNERDRAW = 0x0000000D;
        internal const int SS_BITMAP = 0x0000000E;
        internal const int SS_ENHMETAFILE = 0x0000000F;
        internal const int SS_ETCHEDHORZ = 0x00000010;
        internal const int SS_ETCHEDVERT = 0x00000011;
        internal const int SS_ETCHEDFRAME = 0x00000012;
        internal const int SS_TYPEMASK = 0x0000001F;

        // SysHeader32 styles 
        //internal const int HDS_HORZ = 0x0000;
        internal const int HDS_VERT = 0x0001;

        // Toolbar styles
        internal const int TBSTYLE_EX_DRAWDDARROWS = 0x00000001;

        // Toolbar button styles
        internal const byte BTNS_SEP = 0x0001;
        internal const byte BTNS_CHECK = 0x0002;
        internal const byte BTNS_GROUP = 0x0004;
        internal const byte BTNS_DROPDOWN = 0x0008;

        // Image list constants
        internal const int I_IMAGENONE = -2;

        // Window styles
        internal const int WS_OVERLAPPED = 0x00000000;
        internal const int WS_TABSTOP = 0x00010000;
        internal const int WS_MAXIMIZEBOX = 0x00010000;
        internal const int WS_GROUP = 0x00020000;
        internal const int WS_MINIMIZEBOX = 0x00020000;
        internal const int WS_SYSMENU = 0x00080000;
        internal const int WS_HSCROLL = 0x00100000;
        internal const int WS_VSCROLL = 0x00200000;
        internal const int WS_BORDER = 0x00800000;
        internal const int WS_CAPTION = 0x00C00000;
        internal const int WS_MAXIMIZE = 0x01000000;
        internal const int WS_DISABLED = 0x08000000;
        internal const int WS_VISIBLE = 0x10000000;
        internal const int WS_MINIMIZE = 0x20000000;
        internal const int WS_CHILD = 0x40000000;
        internal const int WS_POPUP = unchecked((int)0x80000000);

        // Button states
        internal const int BST_UNCHECKED = 0x0000;
        internal const int BST_CHECKED = 0x0001;
        internal const int BST_INDETERMINATE = 0x0002;
        internal const int BST_PUSHED = 0x0004;
        internal const int BST_FOCUS = 0x0008;

        //GetDeviceCaps() 
        internal const int LOGPIXELSX = 88;
        internal const int LOGPIXELSY = 90;

        // GetWindow() 
        internal const int GW_HWNDFIRST = 0;
        internal const int GW_HWNDLAST = 1;
        internal const int GW_HWNDNEXT = 2;
        internal const int GW_HWNDPREV = 3;
        internal const int GW_OWNER = 4;
        internal const int GW_CHILD = 5;

        // GetWindowLong()
        internal const int GWL_EXSTYLE = (-20);
        internal const int GWL_STYLE = (-16);
        internal const int GWL_ID = (-12);
        internal const int GWL_HWNDPARENT = (-8);
        internal const int GWL_WNDPROC = (-4);

        // GetSysColor()
        internal const int COLOR_WINDOW = 5;
        internal const int COLOR_WINDOWTEXT = 8;

        // Mouse Key
        internal const int MK_LBUTTON = 0x0001;
        internal const int MK_RBUTTON = 0x0002;

        // Scrollbar 
        internal const int SB_HORZ = 0;
        internal const int SB_VERT = 1;
        internal const int SB_CTL = 2;
        internal const int SB_LINEUP = 0;
        internal const int SB_LINELEFT = 0;
        internal const int SB_LINEDOWN = 1;
        internal const int SB_LINERIGHT = 1;
        internal const int SB_PAGEUP = 2;
        internal const int SB_PAGELEFT = 2;
        internal const int SB_PAGEDOWN = 3;
        internal const int SB_PAGERIGHT = 3;
        internal const int SB_THUMBPOSITION = 4;
        internal const int SB_THUMBTRACK = 5;
        internal const int SB_LEFT = 6;
        internal const int SB_RIGHT = 7;
        internal const int SB_ENDSCROLL = 8;
        internal const int SB_TOP = 6;
        internal const int SB_BOTTOM = 7;

        internal const int SORT_DEFAULT = 0x0;
        internal const int SUBLANG_DEFAULT = 0x01;

        internal const int SC_TASKLIST = 0xF130;

        // ShowWindow() 
        internal const int SW_HIDE = 0;
        internal const int SW_NORMAL = 1;
        internal const int SW_SHOWMINIMIZED = 2;
        internal const int SW_SHOWMAXIMIZED = 3;
        internal const int SW_MAXIMIZE = 3;
        internal const int SW_SHOWNOACTIVATE = 4;
        internal const int SW_SHOW = 5;
        internal const int SW_MINIMIZE = 6;
        internal const int SW_SHOWMINNOACTIVE = 7;
        internal const int SW_SHOWNA = 8;
        internal const int SW_RESTORE = 9;
        internal const int SW_MAX = 10;

        internal const int SWP_NOSIZE = 0x0001;
        internal const int SWP_NOMOVE = 0x0002;
        internal const int SWP_NOZORDER = 0x0004;
        internal const int SWP_NOACTIVATE = 0x0010;
        internal const int SWP_SHOWWINDOW = 0x0040;
        internal const int SWP_HIDEWINDOW = 0x0080;
        internal const int SWP_DRAWFRAME = 0x0020;

        // System Metrics
        internal const int SM_CXSCREEN = 0;
        internal const int SM_CYSCREEN = 1;
        internal const int SM_CXVSCROLL = 2;
        internal const int SM_CYHSCROLL = 3;
        internal const int SM_CYCAPTION = 4;
        internal const int SM_CXBORDER = 5;
        internal const int SM_CYBORDER = 6;
        internal const int SM_CYVTHUMB = 9;
        internal const int SM_CXHTHUMB = 10;
        internal const int SM_CXICON = 11;
        internal const int SM_CYICON = 12;
        internal const int SM_CXCURSOR = 13;
        internal const int SM_CYCURSOR = 14;
        internal const int SM_CYMENU = 15;
        internal const int SM_CYKANJIWINDOW = 18;
        internal const int SM_MOUSEPRESENT = 19;
        internal const int SM_CYVSCROLL = 20;
        internal const int SM_CXHSCROLL = 21;
        internal const int SM_DEBUG = 22;
        internal const int SM_SWAPBUTTON = 23;
        internal const int SM_CXMIN = 28;
        internal const int SM_CYMIN = 29;
        internal const int SM_CXSIZE = 30;
        internal const int SM_CYSIZE = 31;
        internal const int SM_CXFRAME = 32;
        internal const int SM_CYFRAME = 33;
        internal const int SM_CXMINTRACK = 34;
        internal const int SM_CYMINTRACK = 35;
        internal const int SM_CXDOUBLECLK = 36;
        internal const int SM_CYDOUBLECLK = 37;
        internal const int SM_CXICONSPACING = 38;
        internal const int SM_CYICONSPACING = 39;
        internal const int SM_MENUDROPALIGNMENT = 40;
        internal const int SM_PENWINDOWS = 41;
        internal const int SM_DBCSENABLED = 42;
        internal const int SM_CMOUSEBUTTONS = 43;
        internal const int SM_CXFIXEDFRAME = 7;
        internal const int SM_CYFIXEDFRAME = 8;
        internal const int SM_SECURE = 44;
        internal const int SM_CXEDGE = 45;
        internal const int SM_CYEDGE = 46;
        internal const int SM_CXMINSPACING = 47;
        internal const int SM_CYMINSPACING = 48;
        internal const int SM_CXSMICON = 49;
        internal const int SM_CYSMICON = 50;
        internal const int SM_CYSMCAPTION = 51;
        internal const int SM_CXSMSIZE = 52;
        internal const int SM_CYSMSIZE = 53;
        internal const int SM_CXMENUSIZE = 54;
        internal const int SM_CYMENUSIZE = 55;
        internal const int SM_ARRANGE = 56;
        internal const int SM_CXMINIMIZED = 57;
        internal const int SM_CYMINIMIZED = 58;
        internal const int SM_CXMAXTRACK = 59;
        internal const int SM_CYMAXTRACK = 60;
        internal const int SM_CXMAXIMIZED = 61;
        internal const int SM_CYMAXIMIZED = 62;
        internal const int SM_NETWORK = 63;
        internal const int SM_CLEANBOOT = 67;
        internal const int SM_CXDRAG = 68;
        internal const int SM_CYDRAG = 69;
        internal const int SM_SHOWSOUNDS = 70;
        internal const int SM_CXMENUCHECK = 71;
        internal const int SM_CYMENUCHECK = 72;
        internal const int SM_MIDEASTENABLED = 74;
        internal const int SM_MOUSEWHEELPRESENT = 75;
        internal const int SM_XVIRTUALSCREEN = 76;
        // Stock Logical Objects 
        internal const int SYSTEM_FONT = 13;

        internal const int SM_YVIRTUALSCREEN = 77;
        internal const int SM_CXVIRTUALSCREEN = 78;
        internal const int SM_CYVIRTUALSCREEN = 79;

        // Virtal Keys
        internal const int VK_TAB = 0x09;
        internal const int VK_RETURN = 0x0D;
        internal const int VK_ESCAPE = 0x1B;
        internal const int VK_PRIOR = 0x21;
        internal const int VK_NEXT = 0x22;
        internal const int VK_F4 = 0x73;

        internal const int MAX_PATH = 260;

        internal const int MDITILE_VERTICAL = 0x0000;
        internal const int MDITILE_HORIZONTAL = 0x0001;
        internal const int MDITILE_SKIPDISABLED = 0x0002;

        internal const int S_OK = 0x00000000;
        internal const int S_FALSE = 0x00000001;

        //internal unsafe delegate bool EnumChildrenCallbackVoid(IntPtr hwnd, void* lParam);

        [StructLayout(LayoutKind.Sequential)]
        internal struct MSG
        {
            internal IntPtr hwnd;
            internal int message;
            internal IntPtr wParam;
            internal IntPtr lParam;
            internal int time;

            // pt was a by-value POINT structure 
            internal int pt_x;
            internal int pt_y;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct LOGFONT
        {
            internal int lfHeight;
            internal int lfWidth;
            internal int lfEscapement;
            internal int lfOrientation;
            internal int lfWeight;
            internal byte lfItalic;
            internal byte lfUnderline;
            internal byte lfStrikeOut;
            internal byte lfCharSet;
            internal byte lfOutPrecision;
            internal byte lfClipPrecision;
            internal byte lfQuality;
            internal byte lfPitchAndFamily;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            internal string lfFaceName;
        }

        // struct for unmanaged SYSTEMTIME struct
        [StructLayout(LayoutKind.Sequential)]
        internal struct SYSTEMTIME
        {
            internal ushort wYear;
            internal ushort wMonth;
            internal ushort wDayOfWeek;
            internal ushort wDay;
            internal ushort wHour;
            internal ushort wMinute;
            internal ushort wSecond;
            internal ushort wMilliseconds;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct NMHDR
        {
            internal IntPtr hwndFrom;
            internal int idFrom;
            internal int code;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TBBUTTON
        {
            internal int iBitmap;
            internal int idCommand;
            internal byte fsState;
            internal byte fsStyle;
            internal byte bReserved0;
            internal byte bReserved1;
            internal int dwData;
            internal IntPtr iString;
        }

        // 
        // ListView constants and strucs
        // 

        // ListView item relation flags
        //      internal const int LVNI_ALL = 0x0000; 
        internal const int LVNI_FOCUSED = 0x0001;
        internal const int LVNI_SELECTED = 0x0002;
        internal const int LVNI_BELOW = 0x0200;
        internal const int LVNI_TORIGHT = 0x0800;

        internal const int LVNI_VISIBLEORDER = 0x0010;
        internal const int LVNI_PREVIOUS = 0x0020;
        internal const int LVNI_VISIBLEONLY = 0x0040;
        internal const int LVNI_SAMEGROUPONLY = 0x0080;

        // Listview's VIEW. v5 and up
        internal const int LV_VIEW_ICON = 0x0000;
        internal const int LV_VIEW_DETAILS = 0x0001;
        internal const int LV_VIEW_SMALLICON = 0x0002;
        internal const int LV_VIEW_LIST = 0x0003;
        internal const int LV_VIEW_TILE = 0x0004;

        // ListView rectangle related constants 
        internal const int LVIR_BOUNDS = 0;
        internal const int LVIR_ICON = 1;
        internal const int LVIR_LABEL = 2;
        internal const int LVIR_SELECTBOUNDS = 3;

        // ListView hit test defines 
        internal const int LVHT_NOWHERE = 0x0001;
        internal const int LVHT_ONITEMICON = 0x0002;
        internal const int LVHT_ONITEMLABEL = 0x0004;
        internal const int LVHT_ONITEMSTATEICON = 0x0008;
        internal const int LVHT_ONITEM = (LVHT_ONITEMICON | LVHT_ONITEMLABEL | LVHT_ONITEMSTATEICON);

        internal const int LVHT_EX_GROUP_HEADER = 0x10000000;
        internal const int LVHT_EX_GROUP_FOOTER = 0x20000000;
        internal const int LVHT_EX_GROUP_COLLAPSE = 0x40000000;
        internal const int LVHT_EX_GROUP_BACKGROUND = unchecked((int)0x80000000);
        internal const int LVHT_EX_GROUP_STATEICON = 0x01000000;
        internal const int LVHT_EX_GROUP_SUBSETLINK = 0x02000000;
        internal const int LVHT_EX_GROUP = (LVHT_EX_GROUP_BACKGROUND | LVHT_EX_GROUP_COLLAPSE | LVHT_EX_GROUP_FOOTER | LVHT_EX_GROUP_HEADER | LVHT_EX_GROUP_STATEICON | LVHT_EX_GROUP_SUBSETLINK);
        internal const int LVHT_EX_ONCONTENTS = 0x04000000;
        internal const int LVHT_EX_FOOTER = 0x08000000;

        // ListView  item flag
        internal const int LVIF_TEXT = 0x0001;
        internal const int LVIF_STATE = 0x0008;
        internal const int LVIF_GROUPID = 0x0100;

        // This used internally and not passed to the listview the other two
        // struct will be passed to the listview depending on what version the list is.
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto)]
        internal struct LVHITTESTINFO_INTERNAL
        {
            internal Win32Point pt;
            internal uint flags;
            internal int iItem;
            internal int iSubItem;    // this is was NOT in win95.  valid only for LVM_SUBITEMHITTEST 
            internal int iGroup;    // version 6 common control
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto)]
        internal struct LVHITTESTINFO
        {
            internal Win32Point pt;
            internal uint flags;
            internal int iItem;
            internal int iSubItem;    // this is was NOT in win95.  valid only for LVM_SUBITEMHITTEST

            internal LVHITTESTINFO(LVHITTESTINFO_INTERNAL htinfo)
            {
                pt = htinfo.pt;
                flags = htinfo.flags;
                iItem = htinfo.iItem;
                iSubItem = htinfo.iSubItem;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto)]
        internal struct LVHITTESTINFO_V6
        {
            internal Win32Point pt;
            internal uint flags;
            internal int iItem;
            internal int iSubItem;    // this is was NOT in win95.  valid only for LVM_SUBITEMHITTEST 
            internal int iGroup;    // version 6 common control

            internal LVHITTESTINFO_V6(LVHITTESTINFO_INTERNAL htinfo)
            {
                pt = htinfo.pt;
                flags = htinfo.flags;
                iItem = htinfo.iItem;
                iSubItem = htinfo.iSubItem;
                iGroup = htinfo.iGroup;
            }
        }

        // Should be class so we can use it with our XSendMessage.XSend 
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct LVITEM
        {
            internal int mask;
            internal int iItem;
            internal int iSubItem;
            internal int state;
            internal int stateMask;
            internal IntPtr pszText;
            internal int cchTextMax;
            internal int iImage;
            internal IntPtr lParam;
            internal int iIndent;
        }

        // new LVITEM structure
        [StructLayout(LayoutKind.Sequential)]
        internal struct LVITEM_V6
        {
            internal uint mask;
            internal int iItem;
            internal int iSubItem;
            internal int state;
            internal int stateMask;
            internal IntPtr pszText;
            internal int cchTextMax;
            internal int iImage;
            internal IntPtr lParam;
            internal int iIndent;
            internal int iGroupID;
            internal int cColumns;
            internal IntPtr puColumns;
        }

        // Listview group specific flags
        internal const int LVGF_HEADER = 0x00000001;
        internal const int LVGF_FOOTER = 0x00000002;
        internal const int LVGF_STATE = 0x00000004;
        internal const int LVGF_ALIGN = 0x00000008;
        internal const int LVGF_GROUPID = 0x00000010;

        internal const int LVGF_SUBTITLE = 0x00000100;
        internal const int LVGF_TASK = 0x00000200;
        internal const int LVGF_DESCRIPTIONTOP = 0x00000400;
        internal const int LVGF_DESCRIPTIONBOTTOM = 0x00000800;
        internal const int LVGF_TITLEIMAGE = 0x00001000;
        internal const int LVGF_EXTENDEDIMAGE = 0x00002000;
        internal const int LVGF_ITEMS = 0x00004000;
        internal const int LVGF_SUBSET = 0x00008000;
        internal const int LVGF_SUBSETITEMS = 0x00010000;

        // Listview group styles
        internal const int LVGS_NORMAL = 0x00000000;
        internal const int LVGS_COLLAPSED = 0x00000001;
        internal const int LVGS_HIDDEN = 0x00000002;
        internal const int LVGS_NOHEADER = 0x00000004;
        internal const int LVGS_COLLAPSIBLE = 0x00000008;
        internal const int LVGS_FOCUSED = 0x00000010;
        internal const int LVGS_SELECTED = 0x00000020;
        internal const int LVGS_SUBSETED = 0x00000040;
        internal const int LVGS_SUBSETLINKFOCUSED = 0x00000080;
        internal const int LVGGR_GROUP = 0;
        internal const int LVGGR_HEADER = 1;
        internal const int LVGGR_LABEL = 2;
        internal const int LVGGR_SUBSETLINK = 3;

        // Should be class so we can use it with our XSendMessage.XSend
        [StructLayout(LayoutKind.Sequential)]
        internal struct LVGROUP
        {
            internal int cbSize;
            internal int mask;
            internal IntPtr pszHeader;
            internal int cchHeader;
            internal IntPtr pszFooter;
            internal int cchFooter;
            internal int iGroupID;
            internal int stateMask;
            internal int state;
            internal int align;

            internal void Init(int size)
            {
                cbSize = size;
                mask = 0;
                pszHeader = pszFooter = IntPtr.Zero;
                cchFooter = cchHeader = 0;
                iGroupID = -1;
                stateMask = state = align = 0;
            }
        }

        // Should be class so we can use it with our XSendMessage.XSend 
        [StructLayout(LayoutKind.Sequential)]
        internal struct LVGROUP_V6
        {
            internal int cbSize;
            internal int mask;
            internal IntPtr pszHeader;
            internal int cchHeader;
            internal IntPtr pszFooter;
            internal int cchFooter;
            internal int iGroupID;
            internal int stateMask;
            internal int state;
            internal int align;

            // new stuff for v6 
            internal IntPtr pszSubtitle;
            internal int cchSubtitle;
            internal IntPtr pszTask;
            internal int cchTask;
            internal IntPtr pszDescriptionTop;
            internal int cchDescriptionTop;
            internal IntPtr pszDescriptionBottom;
            internal int cchDescriptionBottom;
            internal int iTitleImage;
            internal int iExtendedImage;
            internal int iFirstItem;         // Read only 
            internal int cItems;             // Read only 
            internal IntPtr pszSubsetTitle;     // NULL if group is not subset
            internal int cchSubsetTitle;


            internal void Init(int size)
            {
                cbSize = size;
                mask = 0;
                pszHeader = pszFooter = IntPtr.Zero;
                cchFooter = cchHeader = 0;
                iGroupID = -1;
                stateMask = state = align = 0;

                //new stuff for v6
                pszSubtitle = IntPtr.Zero;
                cchSubtitle = 0;
                pszTask = IntPtr.Zero;
                cchTask = 0;
                pszDescriptionTop = IntPtr.Zero;
                cchDescriptionTop = 0;
                pszDescriptionBottom = IntPtr.Zero;
                cchDescriptionBottom = 0;
                iTitleImage = 0;
                iExtendedImage = 0;
                iFirstItem = 0;         // Read only
                cItems = 0;             // Read only 
                pszSubsetTitle = IntPtr.Zero; // NULL if group is not subset 
                cchSubsetTitle = 0;
            }
        }

        internal const int LVGMF_BORDERSIZE = 0x00000001;

        [StructLayout(LayoutKind.Sequential)]
        internal struct LVGROUPMETRICS
        {
            internal int cbSize;
            internal int mask;
            internal int Left;
            internal int Top;
            internal int Right;
            internal int Bottom;
            internal int crLeft;
            internal int crTop;
            internal int crBottom;
            internal int crRightHeader;
            internal int crFooter;

            internal LVGROUPMETRICS(int size, int flag)
            {
                cbSize = size;
                mask = flag;
                Left = Top = Bottom = Right = 0;
                crLeft = crTop = crBottom = crLeft = crFooter = crRightHeader = 0;
            }
        }


        // supports a single item in multiple groups.
        [StructLayout(LayoutKind.Sequential)]
        internal struct LVITEMINDEX
        {
            internal int iItem;          // listview item index 
            internal int iGroup;         // group index (must be -1 if group view is not enabled)

            internal LVITEMINDEX(int item, int group)
            {
                iItem = item;
                iGroup = group;
            }
        }


        // 
        // Getting the version of the common controls
        //

        internal const int CCM_FIRST = 0x2000;
        internal const int CCM_GETVERSION = CCM_FIRST + 0x8;


        //
        // PAGER CONTROL consts and structs from commctrl.h 
        //

        internal const int PGB_TOPORLEFT = 0;
        internal const int PGB_BOTTOMORRIGHT = 1;

        // height and width values 
        internal const int PGF_CALCWIDTH = 1;
        internal const int PGF_CALCHEIGHT = 2;

        //The scroll can be in one of the following control State
        internal const int PGF_INVISIBLE = 0;      // Scroll button is not visible
        internal const int PGF_NORMAL = 1;      // Scroll button is in normal state
        internal const int PGF_GRAYED = 2;      // Scroll button is in grayed state 
        internal const int PGF_DEPRESSED = 4;      // Scroll button is in depressed state
        internal const int PGF_HOT = 8;      // Scroll button is in hot state 

        [StructLayout(LayoutKind.Sequential)]
        private struct NMPGSCROLL
        {
            internal NMHDR hdr;
            internal bool fwKeys;
            internal Rect rcParent;
            internal int iDir;
            internal int iXpos;
            internal int iYpos;
            internal int iScroll;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NMPGCALCSIZE
        {
            internal NMHDR hdr;
            internal uint dwFlag;
            internal int iWidth;
            internal int iHeight;
        }

        //CASRemoval:[System.Security.Permissions.SecurityPermissionAttribute (System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
        static internal class Util
        {
            internal static int MAKELONG(int low, int high)
            {
                return (high << 16) | (low & 0xffff);
            }

            internal static IntPtr MAKELPARAM(int low, int high)
            {
                return (IntPtr)((high << 16) | (low & 0xffff));
            }

            internal static int HIWORD(int n)
            {
                return (n >> 16) & 0xffff;
            }
            internal static int HIDWORD(long n)
            {
                return unchecked((int)((n >> 32) & 0xffffffff));
            }

            internal static int LOWORD(int n)
            {
                return n & 0xffff;
            }
            internal static int LODWORD(long n)
            {
                return unchecked((int)(n & 0xffffffff));
            }
        }

        //Win32 additions 
        internal const int EventSystemSound = 0x0001;
        internal const int EventSystemAlert = 0x0002;
        internal const int EventSystemForeground = 0x0003;
        internal const int EventSystemMenuStart = 0x0004;
        internal const int EventSystemMenuEnd = 0x0005;
        internal const int EventSystemMenuPopupStart = 0x0006;
        internal const int EventSystemMenuPopupEnd = 0x0007;
        internal const int EventSystemCaptureStart = 0x0008;
        internal const int EventSystemCaptureEnd = 0x0009;
        internal const int EventSystemMoveSizeStart = 0x000a;
        internal const int EventSystemMoveSizeEnd = 0x000b;
        internal const int EventSystemContextHelpStart = 0x000c;
        internal const int EventSystemContextHelpEnd = 0x000d;
        internal const int EventSystemDragDropStart = 0x000e;
        internal const int EventSystemDragDropEnd = 0x000f;
        internal const int EventSystemDialogStart = 0x0010;
        internal const int EventSystemDialogEnd = 0x0011;
        internal const int EventSystemScrollingStart = 0x0012;
        internal const int EventSystemScrollingEnd = 0x0013;
        internal const int EventSystemSwitchEnd = 0x0015;
        internal const int EventSystemMinimizeStart = 0x0016;
        internal const int EventSystemMinimizeEnd = 0x0017;
        internal const int EventSystemPaint = 0x0019;

        internal const int EventConsoleCaret = 0x4001;
        internal const int EventConsoleUpdateRegion = 0x4002;
        internal const int EventConsoleUpdateSimple = 0x4003;
        internal const int EventConsoleUpdateScroll = 0x4004;
        internal const int EventConsoleLayout = 0x4005;
        internal const int EventConsoleStartApplication = 0x4006;
        internal const int EventConsoleEndApplication = 0x4007;

        internal const int EventObjectCreate = 0x8000;
        internal const int EventObjectDestroy = 0x8001;
        internal const int EventObjectShow = 0x8002;
        internal const int EventObjectHide = 0x8003;
        internal const int EventObjectReorder = 0x8004;
        internal const int EventObjectFocus = 0x8005;
        internal const int EventObjectSelection = 0x8006;
        internal const int EventObjectSelectionAdd = 0x8007;
        internal const int EventObjectSelectionRemove = 0x8008;
        internal const int EventObjectSelectionWithin = 0x8009;
        internal const int EventObjectStateChange = 0x800A;
        internal const int EventObjectLocationChange = 0x800B;
        internal const int EventObjectNameChange = 0x800C;
        internal const int EventObjectDescriptionChange = 0x800D;
        internal const int EventObjectValueChange = 0x800E;
        internal const int EventObjectParentChange = 0x800F;
        internal const int EventObjectHelpChange = 0x8010;
        internal const int EventObjectDefactionChange = 0x8011;
        internal const int EventObjectAcceleratorChange = 0x8012;
        internal const int EventObjectInvoke = 0x8013;
        internal const int EventObjectTextSelectionChanged = 0x8014;

        #region Oleacc

        internal const int OBJID_CLIENT = unchecked((int)0xFFFFFFFC);
        internal const int OBJID_WINDOW = 0x00000000;
        internal const int OBJID_VSCROLL = unchecked((int)0xFFFFFFFB);
        internal const int OBJID_HSCROLL = unchecked((int)0xFFFFFFFA);
        internal const int OBJID_MENU = unchecked((int)0xFFFFFFFD);
        internal const int OBJID_SYSMENU = unchecked((int)0xFFFFFFFF);
        internal const int OBJID_NATIVEOM = unchecked((int)0xFFFFFFF0);
        internal const int OBJID_CARET = unchecked((int)0xFFFFFFF8);

        #endregion

        internal const int SELFLAG_TAKEFOCUS = 0x1;
        internal const int SELFLAG_TAKESELECTION = 0x2;
        internal const int SELFLAG_ADDSELECTION = 0x8;
        internal const int SELFLAG_REMOVESELECTION = 0x10;

        internal const int E_ACCESSDENIED = unchecked((int)0x80070005);
        internal const int E_FAIL = unchecked((int)0x80004005);
        internal const int E_UNEXPECTED = unchecked((int)0x8000FFFF);
        internal const int E_INVALIDARG = unchecked((int)0x80070057);
        internal const int E_MEMBERNOTFOUND = unchecked((int)0x80020003);
        internal const int E_NOTIMPL = unchecked((int)0x80004001);
        internal const int E_OUTOFMEMORY = unchecked((int)0x8007000E);

        // Thrown during stress (Win32 call failing in COM) 
        internal const int RPC_E_SYS_CALL_FAILED = unchecked((int)0x80010100);

        internal const int RPC_E_SERVERFAULT = unchecked((int)0x80010105);
        internal const int RPC_E_DISCONNECTED = unchecked((int)0x80010108);

        internal const int DISP_E_BADINDEX = unchecked((int)0x8002000B);

        // Thrown by Word and possibly others 
        // The RPC server is unavailable 
        internal const int RPC_E_UNAVAILABLE = unchecked((int)0x800706BA);
        // The interface is unknown 
        internal const int E_INTERFACEUNKNOWN = unchecked((int)0x800706B5);
        // An unknown Error code thrown by Word being closed while a search is running
        internal const int E_UNKNOWNWORDERROR = unchecked((int)0x800A01A8);



        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Rect
        {
            internal int left;
            internal int top;
            internal int right;
            internal int bottom;

            internal Win32Rect(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            internal Win32Rect(Rect rc)
            {
                this.left = (int)rc.Left;
                this.top = (int)rc.Top;
                this.right = (int)rc.Right;
                this.bottom = (int)rc.Bottom;
            }

            internal bool IsEmpty
            {
                get
                {
                    return left >= right || top >= bottom;
                }
            }

            static internal Win32Rect Empty
            {
                get
                {
                    return new Win32Rect(0, 0, 0, 0);
                }
            }

            static public explicit operator Rect(Win32Rect rc)
            {
                // Convert to Windows.Rect (x, y, witdh, heigh) 

                // Note we need special case Win32Rect.Empty since Rect with widht/height of 0 
                // does not consider to be Empty (see Rect in Base\System\Windows\Rect.cs) 

                // This test is necessary to prevent throwing an exception in new Rect() 
                if (rc.IsEmpty)
                {
                    return Rect.Empty;
                }
                return new Rect(rc.left, rc.top, rc.right - rc.left, rc.bottom - rc.top);
            }

            internal Rect ToRect(bool isRtoL)
            {
                Normalize(isRtoL);
                return (Rect)this;
            }

            internal void Normalize(bool isRtoL)
            {
                // Invert the left and right values for right-to-left windows 
                if (isRtoL)
                {
                    int temp = this.left;
                    this.left = this.right;
                    this.right = temp;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            internal int x;
            internal int y;

            internal Win32Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            static public explicit operator Win32Point(Point pt)
            {
                return checked(new Win32Point((int)pt.X, (int)pt.Y));
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SIZE
        {
            internal int cx;
            internal int cy;

            internal SIZE(int cx, int cy)
            {
                this.cx = cx;
                this.cy = cy;
            }
        }

        internal const int PROCESSOR_ARCHITECTURE_INTEL = 0;
        internal const int PROCESSOR_ARCHITECTURE_MIPS = 1;
        internal const int PROCESSOR_ARCHITECTURE_ALPHA = 2;
        internal const int PROCESSOR_ARCHITECTURE_PPC = 3;
        internal const int PROCESSOR_ARCHITECTURE_SHX = 4;
        internal const int PROCESSOR_ARCHITECTURE_ARM = 5;
        internal const int PROCESSOR_ARCHITECTURE_IA64 = 6;
        internal const int PROCESSOR_ARCHITECTURE_ALPHA64 = 7;
        internal const int PROCESSOR_ARCHITECTURE_MSIL = 8;
        internal const int PROCESSOR_ARCHITECTURE_AMD64 = 9;
        internal const int PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFFFF;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct SYSTEM_INFO
        {
            internal ushort wProcessorArchitecture;
            internal ushort wReserved;
            internal uint dwPageSize;
            internal IntPtr lpMinimumApplicationAddress;
            internal IntPtr lpMaximumApplicationAddress;
            internal IntPtr dwActiveProcessorMask;
            internal uint dwNumberOfProcessors;
            internal uint dwProcessorType;
            internal uint dwAllocationGranularity;
            internal ushort wProcessorLevel;
            internal ushort wProcessorRevision;
        }

        // 
        // ScrollInfo consts and struct
        // 

        internal const int SIF_RANGE = 0x0001;
        internal const int SIF_PAGE = 0x0002;
        internal const int SIF_POS = 0x0004;
        internal const int SIF_TRACKPOS = 0x0010;
        internal const int SIF_ALL = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS);

        [StructLayout(LayoutKind.Sequential)]
        internal struct ScrollInfo
        {
            internal int cbSize;
            internal int fMask;
            internal int nMin;
            internal int nMax;
            internal int nPage;
            internal int nPos;
            internal int nTrackPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct ScrollBarInfo
        {
            internal int cbSize;
            internal Win32Rect rcScrollBar;
            internal int dxyLineButton;
            internal int xyThumbTop;
            internal int xyThumbBottom;
            internal int reserved;
            internal int scrollBarInfo;
            internal int upArrowInfo;
            internal int largeDecrementInfo;
            internal int thumbnfo;
            internal int largeIncrementInfo;
            internal int downArrowInfo;
        }

        internal const int QS_KEY = 0x0001;
        internal const int QS_MOUSEMOVE = 0x0002;
        internal const int QS_MOUSEBUTTON = 0x0004;
        internal const int QS_POSTMESSAGE = 0x0008;
        internal const int QS_TIMER = 0x0010;
        internal const int QS_PAINT = 0x0020;
        internal const int QS_SENDMESSAGE = 0x0040;
        internal const int QS_HOTKEY = 0x0080;
        internal const int QS_ALLPOSTMESSAGE = 0x0100;
        internal const int QS_MOUSE = QS_MOUSEMOVE | QS_MOUSEBUTTON;
        internal const int QS_INPUT = QS_MOUSE | QS_KEY;
        internal const int QS_ALLEVENTS = QS_INPUT | QS_POSTMESSAGE | QS_TIMER | QS_PAINT | QS_HOTKEY;
        internal const int QS_ALLINPUT = QS_INPUT | QS_POSTMESSAGE | QS_TIMER | QS_PAINT | QS_HOTKEY | QS_SENDMESSAGE;

        internal const int INFINITE = unchecked((int)0xFFFFFFFF);

        internal const int WAIT_FAILED = unchecked((int)0xFFFFFFFF);
        internal const int WAIT_TIMEOUT = 0x00000102;

        internal const int SMTO_BLOCK = 0x0001;

        //
        // INPUT consts and structs
        //

        internal const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        internal const int KEYEVENTF_KEYUP = 0x0002;
        internal const int KEYEVENTF_SCANCODE = 0x0008;
        internal const int MOUSEEVENTF_VIRTUALDESK = 0x4000;

        internal const int INPUT_MOUSE = 0;
        internal const int INPUT_KEYBOARD = 1;

        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT
        {
            internal int type;
            internal INPUTUNION union;
        };

        [StructLayout(LayoutKind.Explicit)]
        internal struct INPUTUNION
        {
            [FieldOffset(0)]
            internal MOUSEINPUT mouseInput;
            [FieldOffset(0)]
            internal KEYBDINPUT keyboardInput;
        };

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            internal int dx;
            internal int dy;
            internal int mouseData;
            internal int dwFlags;
            internal int time;
            internal IntPtr dwExtraInfo;
        };

        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            internal short wVk;
            internal short wScan;
            internal int dwFlags;
            internal int time;
            internal IntPtr dwExtraInfo;
        };

        internal const int GA_PARENT = 1;

        internal const int PM_REMOVE = 0x0001;

        internal const int HEAP_SHARED = 0x04000000;      // Win95 only 

        internal const int PROCESS_VM_OPERATION = 0x0008;
        internal const int PROCESS_VM_READ = 0x0010;
        internal const int PROCESS_VM_WRITE = 0x0020;
        internal const int PROCESS_QUERY_INFORMATION = 0x0400;
        internal const int STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        internal const int SYNCHRONIZE = 0x00100000;
        internal const int PROCESS_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF;

        internal const int CHILD_SELF = 0x0;

        internal const int ROLE_SYSTEM_MENUBAR = 0x2;
        internal const int ROLE_SYSTEM_TOOLBAR = 0x16;
        internal const int ROLE_SYSTEM_CLIENT = 0xa;
        internal const int ROLE_SYSTEM_MENUPOPUP = 0xb;
        internal const int ROLE_SYSTEM_LINK = 0x1e;
        internal const int ROLE_SYSTEM_TEXT = 0x0000002A;
        internal const int ROLE_SYSTEM_BUTTONDROPDOWN = 0x00000038;
        internal const int ROLE_SYSTEM_BUTTONMENU = 0x39;
        internal const int ROLE_SYSTEM_MENUITEM = 0x0000000C;
        internal const int ROLE_SYSTEM_GROUPING = 0x14;
        internal const int ROLE_SYSTEM_BUTTONDROPDOWNGRID = 0x0000003A;
        internal const int ROLE_SYSTEM_DROPLIST = 0x0000002F;
        internal const int ROLE_SYSTEM_LISTITEM = 0x22;

        internal const int ROLE_SYSTEM_PUSHBUTTON = 0x2b;
        internal const int ROLE_SYSTEM_CHECKBUTTON = 0x2c;
        internal const int ROLE_SYSTEM_RADIOBUTTON = 0x2d;
        internal const int ROLE_SYSTEM_COMBOBOX = 0x2e;
        internal const int ROLE_SYSTEM_SPINBUTTON = 0x34;

        internal const int STATE_SYSTEM_FLOATING = 0x00001000;
        internal const int STATE_SYSTEM_FOCUSED = 0x4;
        internal const int STATE_SYSTEM_MOVEABLE = 0x00040000;
        internal const int STATE_SYSTEM_CHECKED = 0x10;
        internal const int STATE_SYSTEM_MIXED = 0x20;
        internal const int STATE_SYSTEM_UNAVAILABLE = 0x0001;
        internal const int STATE_SYSTEM_INVISIBLE = 0x8000;
        internal const int STATE_SYSTEM_OFFSCREEN = 0x010000;
        internal const int STATE_SYSTEM_PRESSED = 0x8;
        internal const int STATE_SYSTEM_SIZEABLE = 0x00020000;
        internal const int STATE_SYSTEM_HOTTRACKED = 0x00000080;

        internal const int CBS_SIMPLE = 0x0001;
        internal const int CBS_DROPDOWN = 0x0002;
        internal const int CBS_DROPDOWNLIST = 0x0003;
        internal const int CBS_COMBOTYPEMASK = 0x0003;

        internal const int CBN_EDITUPDATE = 6;
        internal const int CBN_DROPDOWN = 7;

        [StructLayout(LayoutKind.Sequential)]
        internal struct COMBOBOXINFO
        {
            internal int cbSize;
            internal Win32Rect rcItem;
            internal Win32Rect rcButton;
            internal int stateButton;
            internal IntPtr hwndCombo;
            internal IntPtr hwndItem;
            internal IntPtr hwndList;

            internal COMBOBOXINFO(int size)
            {
                cbSize = size;
                rcItem = Win32Rect.Empty;
                rcButton = Win32Rect.Empty;
                stateButton = 0;
                hwndCombo = IntPtr.Zero;
                hwndItem = IntPtr.Zero;
                hwndList = IntPtr.Zero;
            }
        };
        internal static int comboboxInfoSize = Marshal.SizeOf(typeof(NativeMethods.COMBOBOXINFO));

        [StructLayout(LayoutKind.Sequential)]
        internal struct MENUBARINFO
        {
            internal int cbSize;
            internal Win32Rect rcBar;
            internal IntPtr hMenu;
            internal IntPtr hwndMenu;
            internal int focusFlags;
        }

        internal const int GUI_CARETBLINKING = 0x00000001;
        internal const int GUI_INMOVESIZE = 0x00000002;
        internal const int GUI_INMENUMODE = 0x00000004;
        internal const int GUI_SYSTEMMENUMODE = 0x00000008;
        internal const int GUI_POPUPMENUMODE = 0x00000010;

        [StructLayout(LayoutKind.Sequential)]
        internal struct GUITHREADINFO
        {
            internal int cbSize;
            internal int dwFlags;
            internal IntPtr hwndActive;
            internal IntPtr hwndFocus;
            internal IntPtr hwndCapture;
            internal IntPtr hwndMenuOwner;
            internal IntPtr hwndMoveSize;
            internal IntPtr hwndCaret;
            internal Win32Rect rc;
        }

        // 
        // Menu consts and structs
        // 

        internal const int MF_BYCOMMAND = 0x00000000;
        internal const int MF_GRAYED = 0x00000001;
        internal const int MF_DISABLED = 0x00000002;
        internal const int MF_BITMAP = 0x00000004;
        internal const int MF_CHECKED = 0x00000008;
        internal const int MF_MENUBARBREAK = 0x00000020;
        internal const int MF_MENUBREAK = 0x00000040;
        internal const int MF_HILITE = 0x00000080;
        internal const int MF_OWNERDRAW = 0x00000100;
        internal const int MF_BYPOSITION = 0x00000400;
        internal const int MF_SEPARATOR = 0x00000800;

        internal const int MFT_RADIOCHECK = 0x00000200;

        internal const int MIIM_STATE = 0x00000001;
        internal const int MIIM_ID = 0x00000002;
        internal const int MIIM_SUBMENU = 0x00000004;
        internal const int MIIM_CHECKMARKS = 0x00000008;
        internal const int MIIM_TYPE = 0x00000010;
        internal const int MIIM_DATA = 0x00000020;
        internal const int MIIM_FTYPE = 0x00000100;

        // obtain the HMENU from the hwnd 
        internal const int MN_GETHMENU = 0x01E1;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct MENUITEMINFO
        {
            internal int cbSize;
            internal int fMask;
            internal int fType;
            internal int fState;
            internal int wID;
            internal IntPtr hSubMenu;
            internal IntPtr hbmpChecked;
            internal IntPtr hbmpUnchecked;
            internal IntPtr dwItemData;
            internal IntPtr dwTypeData;
            internal int cch;
            internal IntPtr hbmpItem;
        }

        #region REBAR Constants and Structs

        [StructLayout(LayoutKind.Sequential)]
        internal struct RB_HITTESTINFO
        {
            internal Win32Point pt;
            internal uint uFlags;
            internal int iBand;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct REBARBANDINFO
        {
            internal uint cbSize;
            internal uint fMask;
            internal uint fStyle;
            internal int clrFore;
            internal int clrBack;
            internal IntPtr lpText;
            internal uint cch;
            internal int iImage;
            internal IntPtr hwndChild;
            internal uint cxMinChild;
            internal uint cyMinChild;
            internal uint cx;
            internal IntPtr hbmBack;
            internal uint wID;
            internal uint cyChild;
            internal uint cyMaxChild;
            internal uint cyIntegral;
            internal uint cxIdeal;
            internal IntPtr lParam;
            internal uint cxHeader;
        }

        // 
        // TreeView constants and strucs
        // 

        internal const int TVIF_TEXT = 0x0001;
        internal const int TVIF_IMAGE = 0x0002;
        internal const int TVIF_STATE = 0x0008;
        internal const int TVIF_CHILDREN = 0x0040;

        internal const int TVIS_SELECTED = 0x0002;
        internal const int TVIS_EXPANDED = 0x0020;
        internal const int TVIS_STATEIMAGEMASK = 0xF000;

        internal const int TVGN_ROOT = 0x0000;
        internal const int TVGN_NEXT = 0x0001;
        internal const int TVGN_PREVIOUS = 0x0002;
        internal const int TVGN_PARENT = 0x0003;
        internal const int TVGN_CHILD = 0x0004;
        internal const int TVGN_CARET = 0x0009;

        // note: this flag has effect only on WinXP and up 
        internal const int TVSI_NOSINGLEEXPAND = 0x8000;

        internal const int TVE_COLLAPSE = 0x0001;
        internal const int TVE_EXPAND = 0x0002;

        // style 
        internal const int TVS_EDITLABELS = 0x0008;
        internal const int TVS_CHECKBOXES = 0x0100;

        [StructLayout(LayoutKind.Sequential)]
        internal struct TVITEM
        {
            internal uint mask;
            internal IntPtr hItem;
            internal uint state;
            internal uint stateMask;
            internal IntPtr pszText;
            internal int cchTextMax;
            internal int iImage;
            internal int iSelectedImage;
            internal int cChildren;
            internal IntPtr lParam;

            internal void Init(IntPtr item)
            {
                mask = 0;
                hItem = item;
                state = 0;
                stateMask = 0;
                pszText = IntPtr.Zero;
                cchTextMax = 0;
                iImage = 0;
                iSelectedImage = 0;
                cChildren = 0;
                lParam = IntPtr.Zero;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct TVHITTESTINFO
        {
            internal Win32Point pt;
            internal uint flags;
            internal IntPtr hItem;

            internal TVHITTESTINFO(int x, int y, uint flg)
            {
                pt.x = x;
                pt.y = y;
                flags = flg;
                hItem = IntPtr.Zero;
            }
        }

        #endregion

        internal const int INDEX_TITLEBAR_SELF = 0;
        internal const int INDEX_TITLEBAR_IMEBUTTON = 1;
        internal const int INDEX_TITLEBAR_MINBUTTON = 2;
        internal const int INDEX_TITLEBAR_MAXBUTTON = 3;
        internal const int INDEX_TITLEBAR_HELPBUTTON = 4;
        internal const int INDEX_TITLEBAR_CLOSEBUTTON = 5;

        internal const int INDEX_TITLEBAR_MIC = 1;
        internal const int INDEX_TITLEBAR_MAC = 5;
        internal const int CCHILDREN_TITLEBAR = 5;

        // Hit Test areas
        internal const int HTTRANSPARENT = -1;
        internal const int HTCAPTION = 2;
        internal const int HTSYSMENU = 3;
        internal const int HTGROWBOX = 4;
        internal const int HTMENU = 5;
        internal const int HTHSCROLL = 6;
        internal const int HTVSCROLL = 7;
        internal const int HTMINBUTTON = 8;
        internal const int HTMAXBUTTON = 9;
        internal const int HTLEFT = 10;
        internal const int HTRIGHT = 11;
        internal const int HTTOP = 12;
        internal const int HTTOPLEFT = 13;
        internal const int HTTOPRIGHT = 14;
        internal const int HTBOTTOM = 15;
        internal const int HTBOTTOMLEFT = 16;
        internal const int HTBOTTOMRIGHT = 17;
        internal const int HTBORDER = 18;
        internal const int HTCLOSE = 20;
        internal const int HTHELP = 21;
        internal const int HTMDIMAXBUTTON = 66;
        internal const int HTMDIMINBUTTON = 67;
        internal const int HTMDICLOSE = 68;

        // System Commands
        internal const int SC_MINIMIZE = 0xF020;
        internal const int SC_MAXIMIZE = 0xF030;
        internal const int SC_CLOSE = 0xF060;
        internal const int SC_KEYMENU = 0xF100;
        internal const int SC_RESTORE = 0xF120;
        internal const int SC_CONTEXTHELP = 0xF180;

        // WinEvent specific consts and delegates

        internal const int WINEVENT_OUTOFCONTEXT = 0x0000;

        internal const int EVENT_MIN = 0x00000001;
        internal const int EVENT_MAX = 0x7FFFFFFF;

        internal const int EVENT_SYSTEM_SOUND = 0x0001;
        internal const int EVENT_SYSTEM_ALERT = 0x0002;
        internal const int EVENT_SYSTEM_FOREGROUND = 0x0003;
        internal const int EVENT_SYSTEM_MENUSTART = 0x0004;
        internal const int EVENT_SYSTEM_MENUEND = 0x0005;
        internal const int EVENT_SYSTEM_MENUPOPUPSTART = 0x0006;
        internal const int EVENT_SYSTEM_MENUPOPUPEND = 0x0007;
        internal const int EVENT_SYSTEM_CAPTURESTART = 0x0008;
        internal const int EVENT_SYSTEM_CAPTUREEND = 0x0009;
        internal const int EVENT_SYSTEM_MOVESIZESTART = 0x000A;
        internal const int EVENT_SYSTEM_MOVESIZEEND = 0x000B;
        internal const int EVENT_SYSTEM_CONTEXTHELPSTART = 0x000C;
        internal const int EVENT_SYSTEM_CONTEXTHELPEND = 0x000D;
        internal const int EVENT_SYSTEM_DRAGDROPSTART = 0x000E;
        internal const int EVENT_SYSTEM_DRAGDROPEND = 0x000F;
        internal const int EVENT_SYSTEM_DIALOGSTART = 0x0010;
        internal const int EVENT_SYSTEM_DIALOGEND = 0x0011;
        internal const int EVENT_SYSTEM_SCROLLINGSTART = 0x0012;
        internal const int EVENT_SYSTEM_SCROLLINGEND = 0x0013;
        internal const int EVENT_SYSTEM_SWITCHEND = 0x0015;
        internal const int EVENT_SYSTEM_MINIMIZESTART = 0x0016;
        internal const int EVENT_SYSTEM_MINIMIZEEND = 0x0017;
        internal const int EVENT_SYSTEM_PAINT = 0x0019;
        internal const int EVENT_CONSOLE_CARET = 0x4001;
        internal const int EVENT_CONSOLE_UPDATE_REGION = 0x4002;
        internal const int EVENT_CONSOLE_UPDATE_SIMPLE = 0x4003;
        internal const int EVENT_CONSOLE_UPDATE_SCROLL = 0x4004;
        internal const int EVENT_CONSOLE_LAYOUT = 0x4005;
        internal const int EVENT_CONSOLE_START_APPLICATION = 0x4006;
        internal const int EVENT_CONSOLE_END_APPLICATION = 0x4007;
        internal const int EVENT_OBJECT_CREATE = 0x8000;
        internal const int EVENT_OBJECT_DESTROY = 0x8001;
        internal const int EVENT_OBJECT_SHOW = 0x8002;
        internal const int EVENT_OBJECT_HIDE = 0x8003;
        internal const int EVENT_OBJECT_REORDER = 0x8004;
        internal const int EVENT_OBJECT_FOCUS = 0x8005;
        internal const int EVENT_OBJECT_SELECTION = 0x8006;
        internal const int EVENT_OBJECT_SELECTIONADD = 0x8007;
        internal const int EVENT_OBJECT_SELECTIONREMOVE = 0x8008;
        internal const int EVENT_OBJECT_SELECTIONWITHIN = 0x8009;
        internal const int EVENT_OBJECT_STATECHANGE = 0x800A;
        internal const int EVENT_OBJECT_LOCATIONCHANGE = 0x800B;
        internal const int EVENT_OBJECT_NAMECHANGE = 0x800C;
        internal const int EVENT_OBJECT_DESCRIPTIONCHANGE = 0x800D;
        internal const int EVENT_OBJECT_VALUECHANGE = 0x800E;
        internal const int EVENT_OBJECT_PARENTCHANGE = 0x800F;
        internal const int EVENT_OBJECT_HELPCHANGE = 0x8010;
        internal const int EVENT_OBJECT_DEFACTIONCHANGE = 0x8011;
        internal const int EVENT_OBJECT_ACCELERATORCHANGE = 0x8012;

        // WinEvent fired when new Avalon UI is created
        public const int EventObjectUIFragmentCreate = 0x6FFFFFFF;

        // the delegate passed to USER for receiving a WinEvent
        public delegate void WinEventProcDef(int winEventHook, int eventId, IntPtr hwnd, int idObject, int idChild, int eventThread, uint eventTime);

        //
        // SysTabControl32 constants and strucs
        // 

        internal const int TCIF_TEXT = 0x0001;
        internal const int TCIF_STATE = 0x0010;

        internal const int TCIS_BUTTONPRESSED = 0x0001;

        internal const int TCS_RIGHT = 0x0002;
        internal const int TCS_MULTISELECT = 0x0004;
        internal const int TCS_VERTICAL = 0x0080;
        internal const int TCS_BUTTONS = 0x0100;
        internal const int TCS_MULTILINE = 0x0200;
        internal const int TCS_FOCUSNEVER = 0x8000;

        [StructLayout(LayoutKind.Sequential)]
        internal struct TCITEM
        {
            internal int mask;
            internal int dwState;
            internal int dwStateMask;
            internal IntPtr pszText;
            internal int cchTextMax;
            internal int iImage;
            internal IntPtr lParam;

            internal void Init()
            {
                mask = 0;
                dwState = 0;
                dwStateMask = 0;
                pszText = IntPtr.Zero;
                cchTextMax = 0;
                iImage = 0;
                lParam = IntPtr.Zero;
            }

            internal void Init(int m)
            {
                mask = m;
                dwState = 0;
                dwStateMask = 0;
                pszText = IntPtr.Zero;
                cchTextMax = 0;
                iImage = 0;
                lParam = IntPtr.Zero;
            }
        }

        // 
        // SysHeader constants and strucs
        // 

        internal const uint HDI_TEXT = 0x0002;
        internal const uint HDI_FORMAT = 0x0004;
        internal const uint HDI_ORDER = 0x0080;

        internal const int HDS_BUTTONS = 0x0002;
        internal const int HDS_HIDDEN = 0x0008;
        internal const int HDS_FILTERBAR = 0x0100;

        internal const int HDF_SORTUP = 0x0400;
        internal const int HDF_SORTDOWN = 0x0200;
        internal const int HDF_SPLITBUTTON = 0x1000000;

        internal const int HHT_ONHEADER = 0x0002;

        [StructLayout(LayoutKind.Sequential)]
        internal struct HDITEM
        {
            internal uint mask;
            internal int cxy;
            internal IntPtr pszText;
            internal IntPtr hbm;
            internal int cchTextMax;
            internal int fmt;
            internal IntPtr lParam;
            internal int iImage;
            internal int iOrder;
            internal uint type;
            internal IntPtr pvFilter;

            internal void Init()
            {
                mask = 0;
                cxy = 0;
                pszText = IntPtr.Zero;
                hbm = IntPtr.Zero;
                cchTextMax = 0;
                fmt = 0;
                lParam = IntPtr.Zero;
                iImage = 0;
                iOrder = 0;
                type = 0;
                pvFilter = IntPtr.Zero;
            }

            // return an empty HDITEM
            internal static readonly HDITEM Empty = new HDITEM();
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct HDHITTESTINFO
        {
            internal NativeMethods.Win32Point pt;
            internal uint flags;
            internal int item;
        }

        // 
        // Win32 Hyperlink constants and strucs
        // 

        internal const int LIF_ITEMINDEX = 0x00000001;
        internal const int LIF_STATE = 0x00000002;
        internal const int LIF_ITEMID = 0x00000004;
        internal const int LIF_URL = 0x00000008;

        internal const int LIS_FOCUSED = 0x00000001;
        internal const int LIS_ENABLED = 0x00000002;
        internal const int LIS_VISITED = 0x00000004;

        internal const int L_MAX_URL_LENGTH = 2048 + 32 + 3;


        //
        //  Win32API SpinControl constants
        // 

        internal const int UDS_HORZ = 0x0040;


        // 
        // Tooltip strucs
        //

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct TOOLINFO
        {
            internal int cbSize;
            internal int uFlags;
            internal IntPtr hwnd;
            internal int uId;
            internal Win32Rect rect;
            internal IntPtr hinst;
            internal IntPtr pszText;
            internal IntPtr lParam;

            internal void Init(int size)
            {
                cbSize = size;
                uFlags = 0;
                hwnd = IntPtr.Zero;
                uId = 0;
                rect = Win32Rect.Empty;
                hinst = IntPtr.Zero;
                pszText = IntPtr.Zero;
                lParam = IntPtr.Zero;
            }
        }

        internal const int TTF_IDISHWND = 0x0001;
    }
}
