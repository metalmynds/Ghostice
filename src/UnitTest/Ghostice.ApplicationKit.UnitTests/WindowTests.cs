using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ghostice.Core;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class WindowTests
    {
        [TestMethod]
        public void ListDesktopWindows()
        {
           List<Control> windows = WindowManager.GetDesktopWindowControls();
        }

        [TestMethod]    
        public void ListOpenWindows()
        {  

            using (var windowA = new FormComplex())
            {
                windowA.Show();

                using (var windowB = new FormNestedTabPageControls())
                {

                    windowB.Show();

                    var listRequest = ActionRequest.List();

                    var result = ActionManager.Execute(null, listRequest);

                    Assert.IsNotNull(result);

                }

            }
        }

        [TestMethod]
        public void ListOpenWindowsRetreivingAdditionalProperties()
        {

            using (var windowA = new FormComplex())
            {
                windowA.Show();

                using (var windowB = new FormNestedTabPageControls())
                {

                    windowB.Show();

                    var listRequest = ActionRequest.List(new String[] { "ForeColor", "BackColor", "WindowState" });

                    var result = ActionManager.Execute(null, listRequest);

                    Assert.IsNotNull(result);

                }

            }
        }

        [TestMethod]
        public void FindChildControlWindows()
        {

            using (var aWindow = new FormComplex())
            {
                aWindow.Show();

                var children = WindowManager.GetChildWindowControls(aWindow);

                Assert.IsNotNull(children);

                Assert.IsTrue(children.Count > 0);

            }
        }

        [TestMethod]
        public void FindChildwindowControlsUnderMDIWindow()
        {

            using (var mdiParent = new FormMdiParent())
            {
                mdiParent.Show();

                Application.DoEvents();

                var childWindows = WindowManager.GetChildWindowControls(mdiParent);

                Assert.IsNotNull(childWindows);

                Assert.IsTrue(childWindows.Count > 0);

                Assert.IsTrue(childWindows[1].Text == "FormMdiChild");
            }
        }


    }
}
