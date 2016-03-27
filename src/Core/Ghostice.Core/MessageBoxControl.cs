using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ghostice.Core
{

    // If window found in the enum windows api (window manager) and 
    // class of window has WS_POPUP then return this class 
    public class MessageBoxControl : Control
    {

        private NativeWindow nativeMessageBox;

        public MessageBoxControl(IntPtr handle)
            : base()
        {

            nativeMessageBox = new NativeWindow();

            nativeMessageBox.AssignHandle(handle);

            var children = NativeMethods.EnumChildWindowHandles(handle);
        }

        public static Boolean IsMessageBox(IntPtr handle)
        {

            StringBuilder classNameBuffer = new StringBuilder(260);

            NativeMethods.GetClassName(handle, classNameBuffer, classNameBuffer.Capacity);

            return (classNameBuffer.ToString() == "#32770");

            //return (NativeMethods.GetWindowLong(handle, NativeMethods.WindowLongFlags.GWL_STYLE) & NativeMethods.WS_POPUPWINDOW) != 0;
        }

    }
}
