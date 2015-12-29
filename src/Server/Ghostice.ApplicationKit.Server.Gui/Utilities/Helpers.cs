using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ghostice.ApplicationKit.Utilities
{
    public static class Helpers
    {

        public static void PositionBottomRightDesktop(Form Target)
        {
            Rectangle desktopDimenions = Screen.PrimaryScreen.WorkingArea;

            Point topLeft = new Point(desktopDimenions.Width - Target.Width - 5, desktopDimenions.Height - Target.Height - 5);

            Target.SetDesktopLocation(topLeft.X, topLeft.Y);
        }

    }
}
