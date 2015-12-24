using System;
using System.Windows.Forms;
using System.IO;

using Ghostice.Core.Server;
using Ghostice.Core.Server.Services;
using Ghostice.Core.Utilities;

namespace Ghostice.ApplicationKit
{
    public partial class FormAppKit : Form
    {

        private const String APPKIT_APPLICATION_DOMAIN_PREFIX = "AppKit_TestDomain_";

        private delegate void ThreadSafeLogMessage(String Message, String Result);

        private delegate void ThreadSafeDisplaySummary(String executable, String arguments);
        

        private static ulong _rpcRequestIndex;

        private GhosticeServer _server;

        public FormAppKit()
        {
            InitializeComponent();

            this.Text = "Ghostice Application Kit Server - v" + ReflectionHelper.ApplicationVersion;

            var executablePath = Path.GetDirectoryName(Application.ExecutablePath);

            var extensions = Path.Combine(executablePath, "Extensions");

            _server = new GhosticeServer(extensions);

            _server.ServiceStatus.ActionPerformed += Status_ActionPerformed;

            _server.ServiceStatus.SystemUnderTestShutdown += Status_SystemUnderTestShutdown;

            _server.ServiceStatus.SystemUnderTestStarted += Status_SystemUnderTestStarted;

        }

        void Status_SystemUnderTestStarted(object sender, StartupEventArgs e)
        {            
            LogMessage(String.Format("Started: {0} Arguments: {1}", e.Path, String.IsNullOrWhiteSpace(e.Arguments) ? "None" : e.Arguments), String.Empty);
        }

        void Status_SystemUnderTestShutdown(object sender, ShutdownEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Status_ActionPerformed(object sender, ActionEventArgs e)
        {
            LogMessage(String.Format("Target: {0} {1}: {2} Value: {2}", e.Request.Location != null ? e.Request.Location.ToString() : "None", e.Request.Operation.ToString(), e.Request.Name, e.Result.ReturnValue), e.Result.Status.ToString());
        }

        protected void HandleChooseTargetButtonClick(object sender, EventArgs e)
        {
            if (opnfledlgApplication.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtTarget.Text = opnfledlgApplication.FileName;
            }
        }

        private void HandleAppKitFormClosed(object sender, FormClosedEventArgs e)
        {
            Ghostice.ApplicationKit.Properties.Settings.Default.Save();
        }

        private void HandleAppKitFormLoad(object sender, EventArgs e)
        {

            _server.Start(new Uri(Ghostice.ApplicationKit.Properties.Settings.Default.AppKitRpcEndpointAddress));

            var banner = "Waldo On ";

            var listenAddress = banner + _server.EndPoint;

            lblRpcAddress.Text = listenAddress;
           
        }

        protected void LogMessage(String Message, String Result)
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new ThreadSafeLogMessage(this.LogMessage), new Object[] { Message, Result });
            }
            else
            {

                _rpcRequestIndex++;

                var timestamp = String.Format("{0} {1}", DateTime.Now.ToString("HH:mm.ss.fff"), DateTime.Now.ToShortDateString());

                var newItem = lstvewLog.Items.Insert(0, _rpcRequestIndex.ToString());

                newItem.SubItems.Add(timestamp);

                newItem.SubItems.Add(Message);

                newItem.SubItems.Add(Result);

                lstvewLog.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                lstvewLog.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                lstvewLog.Columns[2].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                lstvewLog.Columns[3].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);

            }

        }

        protected void DisplaySummary(String executable, String arguments)
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new ThreadSafeDisplaySummary(this.DisplaySummary), new Object[] { executable, arguments });
            }
            else
            {
                txtTarget.Text = executable;
                txtArguments.Text = arguments;
            }

        }

    }
}
