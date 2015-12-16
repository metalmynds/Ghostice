using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace Ghostice.Framework
{
    public class WinFormWindowBase : WinFormControlBase
    {

        public WinFormWindowBase(InterfaceControl Parent)
            : base(Parent)
        {

        }

        public virtual void Close()
        {
            HandleResult(GetDispatcher().Perform(
                ActionRequest.Execute(this.Path, "Close")
                ));
        }

        public Boolean Visible
        {
            get
            {
                return HandleResult<Boolean>(GetDispatcher().Perform(
                    ActionRequest.Get(this.Path, "Visible")
                ));
            }
            //set
            //{
            //    HandleResult(Dispatcher.Execute(
            //        ActionRequest.Set(this.Path, "Visible", value.ToString())
            //    ));
            //} 
        }

        public Boolean Enabled
        {
            get
            {
                return HandleResult<Boolean>(GetDispatcher().Perform(
                    ActionRequest.Get(this.Path, "Enabled")
                ));
            }
            //set
            //{
            //    HandleResult(Dispatcher.Execute(
            //        ActionRequest.Set(this.Path, "Enabled", value.ToString())
            //    ));
            //}
        }

        //public void WaitFor()
        //{

        //    var finished = false;

        //    while (!finished)
        //    {

        //        try
        //        {
        //            finished = this.Visible;
        //        }
        //        catch (Exception ex)
        //        {

        //        }

        //        Thread.Sleep(250);

        //    }

        //}
    }
}
