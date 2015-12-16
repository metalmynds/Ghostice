using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ghostice.Core;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class CoreTests
    {
        [TestMethod]
        public void UIPropertyCreate()
        {
            String value = "A Property Value";

            var property = UIProperty.Create("APropertyName", value);

            Assert.IsNotNull(property);

            Assert.AreEqual<String>(property.PropertyName, "APropertyName");

            Assert.AreEqual<String>(property.Value, value);

        }
    }
}
