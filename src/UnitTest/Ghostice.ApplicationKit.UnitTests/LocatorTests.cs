using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ghostice.Core;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class LocatorTests
    {
        [TestMethod]
        public void SimpleLocatorToString()
        {
            var locator = new ControlPath();

            var button1Descriptor = new ControlDescription();

            locator.Path.Add(button1Descriptor);

            button1Descriptor.Properties.Add(new Property("Name", "butOK"));

            button1Descriptor.Properties.Add(new Property("Class", "Button"));

            var locatorString = locator.ToString();

            Assert.AreEqual("Descriptor[Name=butOK, Class=Button]", locatorString);
        }

        [TestMethod]
        public void LongLocatorToString()
        {
            var locator = new ControlPath();

            var windowDescriptor = new ControlDescription();

            locator.Path.Add(windowDescriptor);

            windowDescriptor.Properties.Add(new Property("Name", "FormMain"));

            windowDescriptor.Properties.Add(new Property("Class", "Window"));

            var groupDescriptor = new ControlDescription();

            locator.Path.Add(groupDescriptor);

            groupDescriptor.Properties.Add(new Property("Name", "grpOptions"));

            groupDescriptor.Properties.Add(new Property("Class", "GroupBox"));

            var buttonDescriptor = new ControlDescription();

            locator.Path.Add(buttonDescriptor);

            buttonDescriptor.Properties.Add(new Property("Name", "butOK"));

            buttonDescriptor.Properties.Add(new Property("Class", "Button"));

            var locatorString = locator.ToString();

            Assert.AreEqual(@"Descriptor[Name=FormMain, Class=Window]\Descriptor[Name=grpOptions, Class=GroupBox]\Descriptor[Name=butOK, Class=Button]", locatorString);
        }

    }
}
