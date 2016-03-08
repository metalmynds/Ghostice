using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ghostice.Core;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class WalkerTests
    {
        [TestMethod]
        public void TopLevelFormWalk()
        {

            using (var form = new FormSimpleWalk())
            {

                form.Show();

                // Find TextBox 1

                var textBox1Locator = new Locator(new Descriptor(DescriptorType.Control, new Property("Name", "textBox1")));

                var textBox1 = WindowWalker.Locate(form, textBox1Locator);

                Assert.IsNotNull(textBox1);

                // Find Button 1

                var button1Locator = new Locator(new Descriptor(DescriptorType.Control, new Property("Name", "button1")));

                var button1 = WindowWalker.Locate(form, button1Locator);

                Assert.IsNotNull(button1);


            }

        }

        [TestMethod]
        public void SimpleNestedControlWalk()
        {

            using (var form = new FormNestedControls())
            {

                form.Show();

                // Find TextBox 1 (its nested in groupBox1)

                var textBox1Locator = new Locator(new Descriptor(DescriptorType.Control, new Property("Name", "groupBox1")), new Descriptor(DescriptorType.Control, new Property("Name", "textBox1")));

                var textBox1 = WindowWalker.Locate(form, textBox1Locator);

                Assert.IsNotNull(textBox1);

            }

        }

        [TestMethod]
        public void TrySetAndGetTextBoxText()
        {

            using (var form = new FormSimpleWalk())
            {

                form.Show();

                // Find TextBox 1

                var text1Locator = new Locator(new Descriptor(DescriptorType.Window, new Property("Name", "FormSimpleWalk")), new Descriptor(DescriptorType.Control, new Property("Name", "textbox1")));

                var value = "Dave Woz Here!";

                var setAction = ActionRequest.Set(text1Locator, "Text", value, typeof(String));

                var textbox1 = WindowWalker.Locate(form, setAction.Target) as Control;

                var setResult = ActionManager.Perform(textbox1, setAction);

                var getAction = ActionRequest.Get(text1Locator, "Text");

                Assert.IsTrue(setResult.Status == ActionResult.ActionStatus.Successful);

                var getResult = ActionManager.Perform(textbox1, getAction);

                Assert.IsTrue(getResult.Status == ActionResult.ActionStatus.Successful);

                Assert.AreEqual(value, JsonConvert.DeserializeObject<String>(getResult.ReturnValue));

            }

        }

        [TestMethod]
        public void ComplexNestedControlWalk()
        {
            using (var form = new FormNestedTabPageControls())
            {

                form.Show();
                var nestedTextBoxLocator = new Locator(new Descriptor(DescriptorType.Window, new Property("Name", "tabctrlTabControl")), new Descriptor(DescriptorType.Control, new Property("Name", "tabpgeTabPage1")), new Descriptor(DescriptorType.Control, new Property("Name", "txtboxTextBox")));

                TextBox textbox1 = WindowWalker.Locate(form, nestedTextBoxLocator) as TextBox;

                Assert.IsNotNull(textbox1);



            }
        }

        [TestMethod]
        public void FormWithSimpleUserControl()
        {

            using (var form = new FormSimpleUserControl())
            {

                form.Show();

                // Find User Control 1

                var userControl1Locator = new Locator(new Descriptor(DescriptorType.Control, new Property("Name", "userControlSimple1")));

                var userControl = WindowWalker.Locate(form, userControl1Locator);

                Assert.IsNotNull(userControl);

                // Find Embedded Text Box 1

                var textBox1Locator = new Locator(new Descriptor(DescriptorType.Control, new Property("Name", "userControlSimple1")), new Descriptor(DescriptorType.Control, new Property("Name", "textBox1")));

                var textbox1 = WindowWalker.Locate(form, textBox1Locator);

                Assert.IsNotNull(textbox1);


            }

        }

        [TestMethod]
        public void FindNestedPropertyOnSimpleUserControl()
        {

            using (var form = new FormSimpleUserControl())
            {

                form.Show();

                var nestedValue = WindowManager.GetNestedControlPropertyValue(form, "AnIdentifier");

                Assert.IsNotNull(nestedValue);
            }
        }

        [TestMethod]
        public void LocateComponents()
        {

            using (var form = new FormWithComponents())
            {

                form.Show();

                // Find Component Font Dialog

                var fontDialog1Locator = new Locator(new Descriptor(DescriptorType.Control, new Property("Name", "fontDialog1")));

                var fontDialog = WindowWalker.Locate(form, fontDialog1Locator);

                Assert.IsNotNull(fontDialog);

                // NEED TO CREATE AN E2E TEST TO TEST FINDING A COMPONENT IN 'Real'
            }

        }

       

    }
}

