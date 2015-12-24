using Anotar.NLog;
using Ghostice.Core.Server.Rpc;
using Ghostice.Core.Server.Services;
using System;
using System.IO;

namespace Ghostice.Core.Server
{
    public class GhosticeServer
    {

        private WaldoService _service;

        private RpcServer _server;

        private Uri _endPoint;

        private IWaldoStatus _status;

        private String _extensions;

        public GhosticeServer(String Extensions) : this(null, Extensions) { }

        public GhosticeServer(IWaldoStatus Receiver, String Extensions)
        {

            if (!Directory.Exists(Extensions))
            {
                throw new ArgumentException(String.Format("Extensions Directory Does Not Exist! Path: {0}", Extensions), "Extensions");
            }

            _extensions = Extensions;

            if (Receiver == null)
            {
                Receiver = new WaldoStatusReceiver();
            }

            _status = Receiver;

            _service = new WaldoService(_status, _extensions); // RPC Service Registration is Automatic

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
            throw new NotImplementedException();
        }

        public String EndPoint { get { return _endPoint.ToString(); } }

        public IWaldoStatus ServiceStatus { get { return _status; } }
    }
}
