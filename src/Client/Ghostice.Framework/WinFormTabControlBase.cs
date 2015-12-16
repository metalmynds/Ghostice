using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Framework
{
    public abstract class WinFormTabControlBase : WinFormControlBase
    {

        public WinFormTabControlBase(InterfaceControl Parent)
            : base(Parent)
        {

        }

        public virtual void SelectTab(int Index)
        {
            this.HandleResult(GetDispatcher().Perform(
                                ActionRequest.Execute(this.Path, "SelectTab", new ActionParameter[] { ActionParameter.Create(Index) })));
        }

        public virtual void SelectTab(String Name)
        {
            this.HandleResult(GetDispatcher().Perform(
                ActionRequest.Execute(this.Path, "SelectTab", new ActionParameter[] { ActionParameter.Create(Name) })));
        }

        public virtual int SelectedIndex
        {
            get
            {
                var result = GetDispatcher().Perform(
                    ActionRequest.Get(this.Path, "SelectTab")
                    );

                this.HandleResult(result);

                return int.Parse(result.ReturnValue);
            }
            set
            {

                this.HandleResult(GetDispatcher().Perform(
                    ActionRequest.Set(this.Path, "SelectTab", value.ToString(), typeof(String))
                    ));

            }

        }



    }
}
