using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ghostice.Core.Diagnostics;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class DiagnosticTests
    {
        [TestMethod]
        public void DiagnosticScanOfExtensions()
        {

            var extensions = DiagnosticManager.ScanExtensions(".\\Extensions");

            Assert.IsNotNull(extensions);

            Assert.IsTrue(extensions.Count > 0);
        }
    }
}
