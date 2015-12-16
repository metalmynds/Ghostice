using System;
using System.Windows.Forms;
using System.IO;

using Ghostice.Core.Server;
using Ghostice.Core.Server.Services;

namespace Ghostice.ApplicationKit
{
    public partial class FormAppKit : Form
    {

        private const String APPKIT_APPLICATION_DOMAIN_PREFIX = "AppKit_TestDomain_";

        delegate void UIThreadSafeLogMessage(String Message, String Result);

        //delegate void UIThreadSafeWaldoStart(object sender, StartupEventArgs e);

        //private RpcServer _rpcServer;

        //private static WaldoService _service;

        private static ulong _rpcRequestIndex;

        //private ApplicationManager _appManager;

        //private ILease _appManagerLease;

        //private ApplicationInfo _sutInformation;

        //private ApplicationManagerSponsor _appManagerSponsor;

        private GhosticeServer _server;

        public FormAppKit()
        {
            InitializeComponent();

            //_appManagerSponsor = new ApplicationManagerSponsor(Ghostice.ApplicationKit.Properties.Settings.Default.AppKitRemoteAppManagerLease);

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


        //private void InitialiseRpc()
        //{

        //    LogTo.Info("Initialising RPC Server");

        //    _rpcRequestIndex = 0;

        //    _service = new WaldoService(); // Service Registration is Automatic

        //    _service.WaldoStartup += HandleWaldoStart;

        //    _service.WaldoPerform += HandleWaldoPerform;

        //    _rpcServer = new RpcServer(new Uri(Ghostice.ApplicationKit.Properties.Settings.Default.AppKitRpcEndpointAddress));

        //    _rpcServer.Listen();

        //    var banner = "Json-RPC On ";

        //    var listenAddress = banner + _rpcServer.Url;

        //    lnklblRpcAddress.Text = listenAddress;

        //    lnklblRpcAddress.LinkArea = new LinkArea(banner.Length, listenAddress.Length - banner.Length);

        //    LogTo.Info("RPC Server Started. Listening to {0}", listenAddress);

        //}

        //private void HandleWaldoStart(object sender, StartupEventArgs e)
        //{

        //    if (this.InvokeRequired)
        //    {
        //        this.Invoke(new UIThreadSafeWaldoStart(this.HandleWaldoStart), new Object[] { sender, e });
        //    }
        //    else
        //    {
        //        String serverExecutableDirectory = Path.GetDirectoryName(Application.ExecutablePath);

        //        String extensionsFullPath = String.Empty;

        //        extensionsFullPath = Path.Combine(serverExecutableDirectory, "ExtensionsPath");

        //        String sutFullPath = String.Empty;

        //        if (!Path.IsPathRooted(e.Path))
        //        {
        //            sutFullPath = Path.Combine(serverExecutableDirectory, e.Path);
        //        }
        //        else
        //        {
        //            sutFullPath = e.Path;
        //        }

        //        txtTarget.Text = sutFullPath;

        //        var args = String.IsNullOrWhiteSpace(e.Arguments) ? String.Empty : e.Arguments;

        //        txtArguments.Text = args;

        //        var appDomainBasePath = Path.GetDirectoryName(sutFullPath);

        //        LogMessage(String.Format("Starting: {0} Arguments: {1}", e.Path, String.IsNullOrWhiteSpace(e.Arguments) ? "None" : e.Arguments), String.Empty);

        //        try
        //        {

        //            _appManager = AppDomainFactory.Create<ApplicationManager>(appDomainBasePath, APPKIT_APPLICATION_DOMAIN_PREFIX, new Object[] { extensionsFullPath }, false);

        //            _appManagerLease = (ILease)RemotingServices.GetLifetimeService(_appManager);

        //            _appManagerLease.Register(_appManagerSponsor);

        //            _sutInformation = _appManager.Start(sutFullPath, args, Ghostice.ApplicationKit.Properties.Settings.Default.AppKitStartupTimeoutSeconds);

        //        }
        //        catch (Exception ex)
        //        {
        //            LogTo.DebugException("Start System Under Test Failed!", ex);
        //        }

        //        LogMessage(String.Format("Started: {0}", e.Path), String.Empty);

        //    }
        //}

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

            //lnklblRpcAddress.Text = listenAddress;

            //lnklblRpcAddress.LinkArea = new LinkArea(banner.Length, listenAddress.Length - banner.Length);
           
        }

        //protected void HandleWaldoPerform(object sender, ActionEventArgs e)
        //{
        //    var displayArgs = e.Request.HasArguments ? String.Join(", ", e.Request.Arguments.ToArray()) : "None";

        //    switch (e.Request.Operation)
        //    {
        //        case ActionRequest.OperationType.Get:

        //            var getResult = _appManager.Execute(e.Request.ToJson());

        //            e.Result = JsonConvert.DeserializeObject<ActionResult>(getResult);

        //            LogMessage(String.Format("Target: {0} Get: {1}", e.Request.Path.ToString(), e.Request.Name), e.Result.ToString());

        //            break;

        //        case ActionRequest.OperationType.Set:

        //            var setResult = _appManager.Execute(e.Request.ToJson());

        //            e.Result = JsonConvert.DeserializeObject<ActionResult>(setResult);

        //            LogMessage(String.Format("Target: {0} Set: {1} Value: {2}", e.Request.Path.ToString(), e.Request.Name, e.Result.ReturnValue), e.Result.ToString());

        //            break;

        //        case ActionRequest.OperationType.Execute:

        //            var executeResult = _appManager.Execute(e.Request.ToJson());

        //            e.Result = JsonConvert.DeserializeObject<ActionResult>(executeResult);

        //            LogMessage(String.Format("Target: {0} Execute: {1} Arguments: {2}", e.Request.Path.ToString(), e.Request.Name, displayArgs), e.Result.ToString());

        //            break;

        //        case ActionRequest.OperationType.Map:

        //            var mapResult = _appManager.Execute(e.Request.ToJson());

        //            e.Result = JsonConvert.DeserializeObject<ActionResult>(mapResult);

        //            LogMessage(String.Format("Target: {0} Map", e.Request.Path.ToString(), e.Request.Name, displayArgs), e.Result.ToString());

        //            break;

        //        case ActionRequest.OperationType.Ready:

        //            var readyResult = _appManager.Execute(e.Request.ToJson());

        //            e.Result = JsonConvert.DeserializeObject<ActionResult>(readyResult);

        //            LogMessage(String.Format("Target: {0} Ready", e.Request.Path.ToString()), e.Result.ToString());

        //            break;


        //        case ActionRequest.OperationType.List:

        //            var result = _appManager.Execute(e.Request.ToJson());

        //            e.Result = JsonConvert.DeserializeObject<ActionResult>(result);

        //            LogMessage(String.Format("Target: List"), e.Result.ToString());

        //            break;

        //        case ActionRequest.OperationType.Unknown:
        //        default:

        //            LogMessage("Ghostice Server Received Unrecognised Operation!", "Failed!");

        //            break;
        //    }

        //}

        protected void LogMessage(String Message, String Result)
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new UIThreadSafeLogMessage(this.LogMessage), new Object[] { Message, Result });
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

    }
}
