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
            AutoResetEvent waitForFinish = new AutoResetEvent(false);

            var messageBoxThread = new Thread(() =>
            {
                MessageBox.Show("A Simple MessageBox", "A Simple Title", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);

                Application.DoEvents();

                waitForFinish.Set();

            });

            messageBoxThread.Start();

            Application.DoEvents();

            var windows = WindowManager.GetApplicationWindows();

            while (windows.Count == 0)
            {
                System.Threading.Thread.Sleep(100);
            }

            var messageBox = (MessageBoxWrapper)windows[0];

            Assert.AreEqual<String>("A Simple Title", messageBox.Caption);

            Assert.AreEqual<String>("A Simple MessageBox", messageBox.Text);

            Assert.IsTrue(messageBox.Buttons.Length == 2);

            messageBox.ClickButton("OK");

            waitForFinish.WaitOne(5000);

            messageBoxThread.Abort();

        }

        public void ComplexMessageBoxHandler()
        {

            var messageBoxThread = new Thread(() =>
            {
                MessageBox.Show("A Simple MessageBox", "A Simple Title");

            });

            messageBoxThread.Start();

            Application.DoEvents();

            var windows = WindowManager.GetApplicationWindows();

            var messageBox = (MessageBoxWrapper)windows[0];

            Assert.AreEqual<String>("A Simple Title", messageBox.Caption);

            Assert.AreEqual<String>("A Simple MessageBox", messageBox.Text);

            Assert.IsTrue(messageBox.Buttons.Length == 1);

            messageBox.ClickButton("OK");

            messageBoxThread.Abort();

        }
    }
}
