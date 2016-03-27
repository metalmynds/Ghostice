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

        private Dictionary<String, IntPtr> buttons = new Dictionary<string, IntPtr>();

        private NativeWindow nativeMessageBox;

        public MessageBoxControl(IntPtr handle)
            : base()
        {

            this.Text = WindowManager.GetWindowText(handle);

            nativeMessageBox = new NativeWindow();

            nativeMessageBox.AssignHandle(handle);

            var children = NativeMethods.EnumChildWindowHandles(handle);

            // Find Buttons and Get Text

            foreach (var childHandle in children)
            {
                var childClass = WindowManager.GetWindowClassName(childHandle);

                var text = WindowManager.GetWindowText(childHandle);

                if (childClass == "Button")
                {
                    buttons.Add(text, childHandle);

                    // NEED TO CHECK WHICH IS DEFAULT BUTTON !!!

                } else if (childClass == "Static")
                {
                    this.Description = text;
                }

            }
        }

        public String Description { get; protected set; }

        public static Boolean IsMessageBox(IntPtr handle)
        {

            return WindowManager.GetWindowClassName(handle) == "#32770";
        }

    }
}
