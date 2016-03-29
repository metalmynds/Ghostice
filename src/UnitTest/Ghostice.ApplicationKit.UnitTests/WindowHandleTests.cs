using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ghostice.Core;
using System.Windows.Forms;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class WindowHandleTests
    {

        public FormOwnerWindow _ownerWindow;

        [TestInitialize]
        public void SetupForms()
        {

            _ownerWindow = new FormOwnerWindow();

            _ownerWindow.ShowInTaskbar = true;

            _ownerWindow.Show();

            Application.DoEvents();

        }

        [TestMethod]
        public void LocateTopLevelRelatedOwnedChildControls()
        {

            var processWindowList = WindowManager.GetApplicationWindows();

            var childWindowList = WindowManager.GetWindowsChildren(_ownerWindow);

            var ownedWindowList = WindowManager.GetOwnedWindows(_ownerWindow);

            Assert.IsTrue(childWindowList.Count == 1);

            Assert.IsTrue(ownedWindowList.Count == 1);

            // This will fail if any dialog/message boxes are still on display 
            // e.g. a previous test did not tidy up!
            Assert.IsTrue(processWindowList.Count == 3);

        }


        [TestCleanup]
        public void DestroyForms()
        {
            _ownerWindow.Close();           
        }

    }
}
