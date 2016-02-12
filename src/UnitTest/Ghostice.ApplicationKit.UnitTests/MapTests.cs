using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ghostice.Core;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class MapTests
    {
        [TestMethod]
        public void SimpleFormMap()
        {

            using (var form = new FormSimpleWalk())
            {

                form.Show();

                var locator = new Locator(new Descriptor("Name=FormSimpleWalk"));

                var mapRequest = ActionRequest.Map(locator, new String[] { "Name", "Position"});

                var result = ActionManager.Execute(form, mapRequest);

                Assert.IsNotNull(result);

                
            }


        }

        [TestMethod]
        public void NestedFormMap()
        {

            using (var form = new FormNestedTabPageControls())
            {

                form.Show();

                var locator = new Locator(new Descriptor("Name=FormNestedTabPageControls"));

                var mapRequest = ActionRequest.Map(locator, new String[] { "Name", "Position" });

                var result = ActionManager.Execute(form, mapRequest);

                Assert.IsNotNull(result);



            }


        }


        [TestMethod]
        public void ComplexFormMap()
        {

            using (var form = new FormComplex())
            {

                form.Show();

                var locator = new Locator(new Descriptor("Name=FormComplex"));

                var mapRequest = ActionRequest.Map(locator, new String[] { "Name", "Location", "Size", "TopMost", "Text", "Value", "Selected", "Focused" });

                var result = ActionManager.Execute(form, mapRequest);

                Assert.IsNotNull(result);


            }


        }

        //[TestMethod]
        //public void ComplexFormTell()
        //{

        //    using (var form = new FormComplex())
        //    {

        //        form.Show();

        //        var locator = new Locator(new Descriptor("Name=FormComplex"));

        //        var tellRequest = ActionRequest.Tell(locator);

        //        var result = ActionManager.Execute(form, tellRequest);

        //        Assert.IsNotNull(result);


        //    }


        //}

    }
}
