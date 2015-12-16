using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using Ghostice.Core;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class MethodTests
    {
        [TestMethod]
        public void FindStandardMethod()
        {

            Button button = new Button();

            var buttonClickMethod = MethodManager.Resolve(button, "PerformClick", null);

            Assert.IsNotNull(buttonClickMethod);

            
        }

        [TestMethod]
        public void FindExtensionMethodNoParameters()
        {

            ListView listview = new ListView();

            ExtensionManager.AddExtension(typeof(ListView), typeof(TestListViewExtension));

            var getValuesMethod = MethodManager.ResolveExtension(listview, "GetGridValues", null);

            Assert.IsNotNull(getValuesMethod);

        }

        [TestMethod]
        public void FindExtensionMethodWithParameters()
        {

            ListView listview = new ListView();

            ExtensionManager.AddExtension(typeof(ListView), typeof(TestListViewExtension));

            var getValuesMethod = MethodManager.ResolveExtension(listview, "GetGridColumnName", new Object[] { 0 } );

            Assert.IsNotNull(getValuesMethod);

        }


        [TestMethod]
        public void LoadExtensions()
        {

            ExtensionManager.LoadExtensions(".\\Extensions");

            var listViewextensionClass = ExtensionManager.GetExtensions(typeof(ListView));

            Assert.IsNotNull(listViewextensionClass);

            var menuStripExtensionClass = ExtensionManager.GetExtensions(typeof(MenuStrip));

            Assert.IsNotNull(menuStripExtensionClass);

        }

        [TestMethod]
        public void LoadExtensionsFindExtensionMethod()
        {

            ExtensionManager.LoadExtensions(".\\Extensions");

            var listViewextensionClass = ExtensionManager.GetExtensions(typeof(ListView));

            Assert.IsNotNull(listViewextensionClass);

            var menuStripExtensionClass = ExtensionManager.GetExtensions(typeof(MenuStrip));

            Assert.IsNotNull(menuStripExtensionClass);

            using (var form = new FormExtensions())
            {


                var listviewGetValuesMethod = MethodManager.ResolveExtension(form.ListView, "GetColumns", null);

                Assert.IsNotNull(listviewGetValuesMethod);


                var menuStripGetValuesMethod = MethodManager.ResolveExtension(form.MenuStrip, "PerformClickMenu", new Object[] { "&File\\Hello Wurld" });

                Assert.IsNotNull(menuStripGetValuesMethod);

            }

        }

    }
}
