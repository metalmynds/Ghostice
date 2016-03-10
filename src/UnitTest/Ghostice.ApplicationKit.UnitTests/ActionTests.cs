using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using Ghostice.Core;
using Newtonsoft.Json;
using System.Drawing;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class ActionTests
    {

        [TestMethod]
        public void GetTextFromNestedTextBox()
        {

            using (var form = new FormNestedTabPageControls())
            {

                form.Show();


                var nestedTextBoxLocator = new Locator(new Descriptor(DescriptorType.Window, new Property("Name", "FormNestedTabPageControls")), new Descriptor(DescriptorType.Control, new Property("Name", "tabctrlTabControl")), new Descriptor(DescriptorType.Control, new Property("Name", "tabpgeTabPage1")), new Descriptor(DescriptorType.Control, new Property("Name", "txtboxTextBox")));

                TextBox textbox1 = WindowWalker.Locate(form, nestedTextBoxLocator) as TextBox;

                Assert.IsNotNull(textbox1);

                var getTextRequest = ActionRequest.Get(nestedTextBoxLocator, "Text");

                var appManager = new AutomationAvatar(String.Empty);

                //System.Threading.Thread.Sleep(512);

                var getActionResult = appManager.Perform(getTextRequest);

                Assert.IsTrue(getActionResult.Status == ActionResult.ActionStatus.Successful);

                Assert.AreEqual<String>("AMP Rules", JsonConvert.DeserializeObject<String>(getActionResult.ReturnValue));

            }

        }

        [TestMethod]
        public void EvaluteSimpleExpression()
        {

            using (var form = new FormEvaluation())
            {

                form.Show();

                var simpleExpression = "target.Text == \"Shiney\"";

                var statusLabelLocator = new Locator(new Descriptor(DescriptorType.Control, new Property("Name", "lblStatus")));

                Label statuslabel = WindowWalker.Locate(form, statusLabelLocator) as Label;

                var goodResult = ActionManager.Evaluate(statuslabel, simpleExpression);

                Assert.IsTrue(goodResult);

                statuslabel.Text = "Not to frett!";

                var negativeResult = ActionManager.Evaluate(statuslabel, simpleExpression);

                Assert.IsFalse(negativeResult);
            }

        }

        [TestMethod]
        public void EvaluteComplexExpression()
        {

            using (var form = new FormEvaluation())
            {

                form.Show();
                
                var complexExpression = "target.Text == \"Shiney\" && target.TextAlign == ContentAlignment.TopLeft";

                var statusLabelLocator = new Locator(new Descriptor(DescriptorType.Control, new Property("Name", "lblStatus")));

                Label statuslabel = WindowWalker.Locate(form, statusLabelLocator) as Label;

                var goodResult = ActionManager.Evaluate(statuslabel, complexExpression);

                Assert.IsTrue(goodResult);

                statuslabel.Text = "Not to frett!";

                var negativeResult = ActionManager.Evaluate(statuslabel, complexExpression);

                Assert.IsFalse(negativeResult);
            }

        }

    }
}
