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
                MessageBox.Show("A Simple MessageBox");

            });

            messageBoxThread.Start();

            Application.DoEvents();

            var windows = WindowManager.GetApplicationWindows();

            messageBoxThread.Abort();

        }
    }
}
