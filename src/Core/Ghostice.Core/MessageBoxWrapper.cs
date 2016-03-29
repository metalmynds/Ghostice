using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ghostice.Core
{

    public class MessageBoxWrapper : Control
    {

        private Dictionary<String, IntPtr> buttons = new Dictionary<String, IntPtr>();

        private NativeWindow nativeMessageBox;

        protected MessageBoxWrapper(IntPtr handle)
            : base()
        {

            this.Caption = WindowManager.GetWindowText(handle);

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

                    //var nativeButton = new NativeWindow();

                    //nativeButton.AssignHandle(childHandle);

                    //var getDlgItemMessage = new Message();

                    //getDlgItemMessage.HWnd = childHandle;

                    //getDlgItemMessage.Msg = (int)NativeMethods.WindowMessage.GETDLGCODE;

                    //getDlgItemMessage.WParam = IntPtr.Zero;

                    //getDlgItemMessage.LParam = IntPtr.Zero;

                    //nativeButton.DefWndProc(ref getDlgItemMessage);

                    //var isDefault = (((int)getDlgItemMessage.Result) & (int)NativeMethods.DLGC.DLGC_BUTTON & (int)NativeMethods.DLGC.DLGC_DEFPUSHBUTTON) != 0;

                    //nativeButton.ReleaseHandle();

                    //if (isDefault)
                    //{
                    //    DefaultButton = text; 
                    //}
                }
                else if (childClass == "Static")
                {
                    // REFACTOR: NEED TO IMPROVE ChildClass Handling (Get Icon)

                    if (((int)NativeMethods.GetWindowLongPtr(childHandle, (int)NativeMethods.WindowLongFlags.GWL_STYLE) & NativeMethods.SS_ICON) != 0)
                    {

                    }
                    else
                    {
                        // REFACTOR: If its not an image then the control is probably the body text 
                        this.Text = text;
                    }


                }

            }

        }

        public String Caption { get; protected set; }

        //public String DefaultButton { get; protected set; }

        public String[] Buttons
        {
            get { return buttons.Keys.ToArray<String>(); }
        }

        public void PressButton(String buttonText)
        {
            if (!buttons.Keys.Contains<String>(buttonText))
            {
                throw new ArgumentException(String.Format("MessageBox Dialog Press Button Failed! Unrecognised Button [{0}]", buttonText), "buttonText");
            }

            var buttonHandle = buttons[buttonText];

            var buttonWindow = new NativeWindow();

            buttonWindow.AssignHandle(buttonHandle);

            var keydownMessage = new Message()
            {
                HWnd = buttonWindow.Handle,
                Msg = (int)NativeMethods.WindowMessage.KEYDOWN,
                LParam = IntPtr.Zero,
                WParam = new IntPtr((int)Keys.Space)
            };

            var keyupMessage = new Message()
            {
                HWnd = buttonWindow.Handle,
                Msg = (int)NativeMethods.WindowMessage.KEYUP,
                LParam = IntPtr.Zero,
                WParam = new IntPtr((int)Keys.Space)
            };

            buttonWindow.DefWndProc(ref keydownMessage);

            buttonWindow.DefWndProc(ref keyupMessage);

            buttonWindow.ReleaseHandle();
        }

        public void ClickButton(String buttonText)
        {

            if (!buttons.Keys.Contains<String>(buttonText))
            {
                throw new ArgumentException(String.Format("MessageBox Dialog Press Button Failed! Unrecognised Button [{0}]", buttonText), "buttonText");
            }

            var buttonHandle = buttons[buttonText];

            var buttonWindow = new NativeWindow();

            buttonWindow.AssignHandle(buttonHandle);

            var clickMessage = new Message() { HWnd = buttonWindow.Handle, Msg = NativeMethods.BM_CLICK, LParam = IntPtr.Zero, WParam = IntPtr.Zero };

            buttonWindow.DefWndProc(ref clickMessage);

            buttonWindow.ReleaseHandle();

        }

        public static Boolean IsMessageBoxWindow(IntPtr handle)
        {
            return WindowManager.GetWindowClassName(handle) == "#32770";
        }

        public new static MessageBoxWrapper FromHandle(IntPtr handle)
        {
            return new MessageBoxWrapper(handle);
        }
    }
}
