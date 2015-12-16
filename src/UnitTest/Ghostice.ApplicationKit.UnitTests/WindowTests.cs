using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ghostice.Core;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class WindowTests
    {
        [TestMethod]
        public void ListOpenWindows()
        {

            using (var windowA = new FormComplex())
            {
                windowA.Show();

                using (var windowB = new FormNestedTabPageControls())
                {

                    windowB.Show();

                    var listRequest = ActionRequest.List();

                    var result = ActionManager.Execute(null, listRequest);

                    Assert.IsNotNull(result);

                }

            }
        }

        [TestMethod]
        public void FindChildControlWindows()
        {

            using (var aWindow = new FormComplex())
            {
                aWindow.Show();

                var children = WindowManager.GetWindowChildControls(aWindow);

                // LOOK AT CHANGING THE LOCATE ROUTINE TO USE THIS METHOD ITS QUICK AND CAN BE DYNAMICALY CALLED.

                // DO WE NEED TO WALK THE PATH THRU THE CONTROLS ????????

            }
        }
    }
}
