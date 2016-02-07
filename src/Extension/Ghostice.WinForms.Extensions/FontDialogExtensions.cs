using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ghostice.WinForms.Extensions
{
    [ControlExtensionProvider(typeof(FontDialog))]
    public static class FontDialogExtensions
    {

        public static void Show(this FontDialog dialog)
        {
            dialog.ShowDialog();
        }

    }
}
