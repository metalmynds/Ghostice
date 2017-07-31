using Anotar.NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ghostice.Core.Server.Rpc
{
    public class HttpServer
    {

        protected delegate void HttpRequestEventHandler(object sender, HttpRequestEventArgs e);

        protected event HttpRequestEventHandler OnHttpRequest;

        protected Thread _requestThread;

        public HttpServer(String endPoint)
            : this(new Uri(endPoint))
        {
        }

        public HttpServer(Uri endPoint)
        {
            var address = endPoint.ToString();

            this.EndPoint = new Uri(address.EndsWith("/") ? address : address += "/");
        }

        public Uri EndPoint { get; protected set; }

        public void Shutdown()
        {
            if (_requestThread != null)
            {
                try
                {
                    _requestThread.Abort();
                }
                catch (Exception ex)
                {
                    LogTo.WarnException("Shutdown of HttpServer Thread Failed!", ex);
                }
            }

        }

        public void Listen()
        {

            // 4.0

            _requestThread = new Thread(new ThreadStart(ServerThread));

            _requestThread.Start();

            // 4.5

            //using (var listener = new HttpListener())
            //{
            //var tcs = new TaskCompletionSource<object>();

            //listener.GetContextAsync().ContinueWith(async t =>
            //{
            //    try
            //    {
            //        while (true)
            //        {
            //            var context = await t;
            //            this.HttpRequestRecieved(new HttpRequestEventArgs(context));
            //            t = listener.GetContextAsync();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        listener.Close();
            //        tcs.TrySetException(e);
            //    }
            //});
            //}
        }

        protected void ServerThread()
        {

            try
            {

                using (var listener = new HttpListener())
                {

                    try
                    {

                        var address = this.EndPoint.ToString();

                        listener.Prefixes.Add(address.EndsWith("/") ? address : address += "/");

                        listener.Start();

                        var callback = new AsyncCallback(ListenerCallback);

                        while (true)
                        {

                            IAsyncResult result = listener.BeginGetContext(callback, listener);

                            result.AsyncWaitHandle.WaitOne();

                        }

                    }
                    catch (Exception ex)
                    {
                        LogTo.ErrorException("HttpServer Thread Failed!", ex);
                        throw;
                    }

                }
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (Exception ex)
            {
                throw new HttpServerStartupFailedException(this.EndPoint.ToString(), ex);
            }
        }


    protected void ListenerCallback(IAsyncResult result)
        {
            var listener = result.AsyncState as HttpListener;

            try
            {

                var context = listener.EndGetContext(result);

                this.HttpRequestRecieved(new HttpRequestEventArgs(context));

            }
            catch (Exception ex)
            {
                LogTo.WarnException("HttpListener Request Call Back Failed!", ex);
            }
        }


        protected virtual void HttpRequestRecieved(HttpRequestEventArgs e)
        {
            var handler = OnHttpRequest;
            if (handler != null)
                handler(this, e);
        }
    }

    [Serializable]
    public class HttpServerStartupFailedException : Exception
    {

        protected HttpServerStartupFailedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }


        public HttpServerStartupFailedException(String Url, Exception InnerException)
            : base(String.Format("RPC Http Server Start-up Failed!\r\nEndPoint Url: {0}\r\nError: {1}", Url, InnerException.Message), InnerException)
        {

        }
    }

}
