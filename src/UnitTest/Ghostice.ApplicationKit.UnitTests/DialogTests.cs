using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using Ghostice.Core;
using System.Threading;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class DialogTests
    {
        [TestMethod]
        public void SimpleMessageBoxHandler()
        {
            
            var messageBoxThread = new Thread(() =>
            {
                MessageBox.Show("A Simple MessageBox", "A Simple Title");

            });

            messageBoxThread.Start();

            Application.DoEvents();

            var windows = WindowManager.GetApplicationWindows();

            var messageBox = (MessageBoxControl)windows[0];

            Assert.AreEqual<String>("A Simple Title", messageBox.Text);

            Assert.AreEqual<String>("A Simple MessageBox", messageBox.Description);

            messageBoxThread.Abort();

        }
    }
}
