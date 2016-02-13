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
            var locator = new Locator();

            var button1Descriptor = new Descriptor(DescriptorType.Window);

            locator.Path.Add(button1Descriptor);

            button1Descriptor.Properties.Add(new Property("Name", "butOK"));

            button1Descriptor.Properties.Add(new Property("Text", "OK"));

            var locatorString = locator.ToString();

            Assert.AreEqual("Window[Name=butOK, Text=OK]", locatorString);
        }

        [TestMethod]
        public void LongLocatorToString()
        {
            var locator = new Locator();

            var windowDescriptor = new Descriptor(DescriptorType.Window);

            locator.Path.Add(windowDescriptor);

            windowDescriptor.Properties.Add(new Property("Name", "FormMain"));

            windowDescriptor.Properties.Add(new Property("Title", "Main Form Title"));

            var groupDescriptor = new Descriptor(DescriptorType.Control);

            locator.Path.Add(groupDescriptor);

            groupDescriptor.Properties.Add(new Property("Name", "grpOptions"));

            groupDescriptor.Properties.Add(new Property("Title", "Groupy"));

            var buttonDescriptor = new Descriptor(DescriptorType.Control);

            locator.Path.Add(buttonDescriptor);

            buttonDescriptor.Properties.Add(new Property("Name", "butOK"));

            buttonDescriptor.Properties.Add(new Property("Title", "OK"));

            var locatorString = locator.ToString();

            Assert.AreEqual(@"Window[Name=FormMain, Title=Main Form Title]\Control[Name=grpOptions, Title=Groupy]\Control[Name=butOK, Title=OK]", locatorString);
        }

        [TestMethod]
        public void GetWindowRelativePath()
        {
            var locator = new Locator();

            var windowDescriptor = new Descriptor(DescriptorType.Window);

            locator.Path.Add(windowDescriptor);

            windowDescriptor.Properties.Add(new Property("Name", "FormMain"));

            var groupDescriptor = new Descriptor(DescriptorType.Control);

            locator.Path.Add(groupDescriptor);

            groupDescriptor.Properties.Add(new Property("Name", "grpOptions"));

            var buttonDescriptor = new Descriptor(DescriptorType.Control);

            locator.Path.Add(buttonDescriptor);

            buttonDescriptor.Properties.Add(new Property("Name", "butOK"));

            var locatorString = locator.ToString();

            Assert.AreEqual(@"Window[Name=FormMain]\Control[Name=grpOptions]\Control[Name=butOK]", locatorString);

            var relativelocatorString = locator.GetRelativePath().ToString();

            Assert.AreEqual(@"Control[Name=grpOptions]\Control[Name=butOK]", relativelocatorString);

            //var deepRelativelocatorString = locator.GetRelativePath(groupDescriptor).ToString();

            //Assert.AreEqual(@"Descriptor[Name=grpOptions, Class=GroupBox]\Descriptor[Name=butOK, Class=Button]", deepRelativelocatorString);
        }

    }
}
