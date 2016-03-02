using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Ghostice.Core;
using Infragistics.Win.UltraWinEditors;

namespace Ghostice.WinForms.Infragistics.Extensions
{
    [ControlExtensionProvider(typeof(UltraTextEditor))]
    public static class UltraTextEditorExtensions
    {
        private const int WM_KEYDOWN = 256;
        private const int WM_KEYUP = 257;
        //private const int WM_CHAR = 258;
        private const int VK_DOWN = 38;

        public static void PressDownKey(this UltraTextEditor target)
        {
            var pressDownMessage = new Message()
            {
                HWnd = target.Handle,
                LParam = IntPtr.Zero,
                Msg = WM_KEYDOWN,
                WParam = new IntPtr(VK_DOWN)
            };

            target.WindowTarget.OnMessage(ref pressDownMessage);

            var pressUpMessage = new Message()
            {
                HWnd = target.Handle,
                LParam = IntPtr.Zero,
                Msg = WM_KEYUP,
                WParam = new IntPtr(VK_DOWN)
            };

            target.WindowTarget.OnMessage(ref pressUpMessage);

        }

    }
}
