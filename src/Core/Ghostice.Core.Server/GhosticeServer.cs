using Anotar.NLog;
using Ghostice.Core.Server.Rpc;
using Ghostice.Core.Server.Services;
using System;
using System.IO;
using System.Reflection;

namespace Ghostice.Core.Server
{
    public class GhosticeServer
    {

        private WaldoService _service;

        private RpcServer _server;

        private Uri _endPoint;

        private IWaldoListener _status;

        private String _extensions;

        private String _serverBinPath;

        public GhosticeServer(String Extensions) : this(null, Extensions) { }

        public GhosticeServer(IWaldoListener Receiver, String Extensions)
        {

            if (!Directory.Exists(Extensions))
            {
                throw new ArgumentException(String.Format("Extensions Directory Does Not Exist! Path: {0}", Extensions), "Extensions");
            }

            _extensions = Extensions;

            _serverBinPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (Receiver == null)
            {
                Receiver = new WaldoServiceListener();
            }

            _status = Receiver;

            _service = new WaldoService(_status, _serverBinPath, _extensions); // RPC Service Registration is Automatic

        }

        public void Start(Uri EndPoint)
        {
            _endPoint = EndPoint;

            InitialiseRpc(_endPoint);
        }

        protected void InitialiseRpc(Uri EndPoint)
        {

            LogTo.Info("Initialising RPC Server");

            _server = new RpcServer(EndPoint);

            _server.Listen();

            LogTo.Info("RPC Server Started. Listening @ {0}", _server.Url);

        }

        public void Shutdown()
        {
            _server.Shutdown();
        }

        public String EndPoint { get { return _endPoint.ToString(); } }

        public IWaldoListener StatusListener { get { return _status; } }
    }
}
