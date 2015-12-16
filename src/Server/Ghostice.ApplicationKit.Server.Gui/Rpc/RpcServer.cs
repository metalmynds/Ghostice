using Anotar.Log4Net;
using AustinHarris.JsonRpc;
using Ghostice.Core;
using Ghostice.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.ApplicationKit.Rpc
{
    public class RpcServer : HttpServer
    {

        public event EventHandler<RemotePerformEventArgs> Perform;

        public RpcServer(Uri EndPoint)
            : base(EndPoint)
        {

            this.OnHttpRequest += HandleRemoteCommandServerDataReceived;
        }

        private void HandleRemoteCommandServerDataReceived(object sender, HttpRequestEventArgs e)
        {
            var context = e.Context;
            var httpRequest = context.Request;

            var encoding = Encoding.UTF8;

            var rpcResultHandler = new AsyncCallback(
                callback =>
                {
                    var asyncData = ((JsonRpcStateAsync)callback);

                    var result = asyncData.Result;

                    var logOut = LogHelper.EscapeJson(result); 

                    LogTo.Debug(logOut);

                    var data = Encoding.UTF8.GetBytes(result);
                    var rpcRequest = ((RpcRequest)asyncData.AsyncState);

                    rpcRequest.Response.ContentType = "application/json";
                    rpcRequest.Response.ContentEncoding = encoding;

                    rpcRequest.Response.ContentLength64 = data.Length;
                    rpcRequest.Response.OutputStream.Write(data, 0, data.Length);

                });

            using (var reader = new StreamReader(httpRequest.InputStream, encoding))
            {
                var line = reader.ReadToEnd();

                var logOut = LogHelper.EscapeJson(line); 

                LogTo.Debug(logOut);

                var response = context.Response;

                var rpcRequest = new RpcRequest(line, response);

                var async = new JsonRpcStateAsync(rpcResultHandler, rpcRequest) { JsonRpc = line };

                JsonRpcProcessor.Process(async, this);
            }
        }

        public class RemotePerformEventArgs : EventArgs
        {

            public RemotePerformEventArgs(ActionRequest Request)
            {
                Result = new ActionResult();
            }

            public ActionRequest Request
            {
                get;
                protected set;
            }

            public ActionResult Result
            {
                get;
                protected set;
            }
        }

    }

}
