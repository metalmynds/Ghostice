using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ghostice.Framework;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class FrameworkTests
    {
        [TestMethod]
        public void ConstructControls()
        {
            var client = new GhosticeClient();

            var textbbox = new WinFormTextBox(client.Application);

            var button = new WinFormButton(client.Application);

            var comboBox = new WinFormComboBox(client.Application);

            var groupBox = new WinFormGroupBox(client.Application);

            var listView = new WinFormListView(client.Application);

        }
    }
}
