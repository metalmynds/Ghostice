using Ghostice.Core;
using Ghostice.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.ApplicationKit.Example.Models
{
    public class HistoryTabPage : WinFormControlBase
    {

        [AutomationDescriptor("RegistrationName", "Name=txtSummary")]
        private PlaceHolder<WinFormTextBox> historySummary;

        [AutomationDescriptor("HistoryTab", "Name=lstvewHistory")]
        private PlaceHolder<WinFormListView> listHistory;


        public HistoryTabPage(InterfaceControl Parent)
            : base(Parent)
        {

        }

        public WinFormTextBox Summary
        {
            get
            {
                return historySummary.GetControl();
            }
        }

        public WinFormListView HistoryList
        {
            get
            {
                return listHistory.GetControl();
            }
        }
    }
}
