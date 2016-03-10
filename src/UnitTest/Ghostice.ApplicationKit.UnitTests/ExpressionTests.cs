using Ghostice.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class ExpressionTests
    {

        [TestMethod]
        public void EvaluteSimpleExpression()
        {

            using (var form = new FormEvaluation())
            {

                form.Show();

                var simpleExpression = "target.Text == \"Shiney\"";
               
                var statusLabelLocator = new Locator(new Descriptor(DescriptorType.Control, new Property("Name", "lblStatus")));

                System.Windows.Forms.Label statusLabel = WindowWalker.Locate(form, statusLabelLocator) as System.Windows.Forms.Label;

                var simpleExpressionPrepared = ExpressionManager.Prepare(statusLabel, simpleExpression);

                var goodResult = ExpressionManager.Evaluate(statusLabel, simpleExpressionPrepared);

                Assert.IsTrue(goodResult);

                statusLabel.Text = "Not to frett!";

                var negativeResult = ExpressionManager.Evaluate(statusLabel, simpleExpressionPrepared);

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

                System.Windows.Forms.Label statusLabel = WindowWalker.Locate(form, statusLabelLocator) as System.Windows.Forms.Label;

                var compiledExpression = ExpressionManager.Prepare(statusLabel, complexExpression);

                var goodResult = ExpressionManager.Evaluate(statusLabel, compiledExpression);

                Assert.IsTrue(goodResult);

                statusLabel.Text = "Not to frett!";

                var negativeResult = ExpressionManager.Evaluate(statusLabel, compiledExpression);

                Assert.IsFalse(negativeResult);
            }

        }


    }
}
