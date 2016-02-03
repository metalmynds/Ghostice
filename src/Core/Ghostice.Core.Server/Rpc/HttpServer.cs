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

        protected HttpListener _listener;

        protected Thread _requestThread;

        public HttpServer(Uri url)
        {
            this.Url = url;

            _listener = new HttpListener();

            if (!HttpListener.IsSupported)

                throw new NotSupportedException("HttpListener not supported. Switch to mono provided one.");

        }

        public Uri Url
        {
            get;
            protected set;
        }

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
                    LogTo.FatalException("Shutdown of Http Request Thread Failed!", ex);
                }
            }

            if (_listener != null)
            {
                try
                {
                    _listener.Close();
                }
                catch (Exception ex)
                {
                    LogTo.FatalException("Shutdown of HttpListener Failed!", ex);
                }

            }
        }

        public void Listen()
        {

            try
            {

                var address = this.Url.ToString();

                _listener.Prefixes.Add(address.EndsWith("/") ? address : address += "/");

                _listener.Start();


            }
            catch (Exception ex)
            {
                throw new HttpServerStartupFailedException(this.Url.ToString(), ex);
            }

            // 4.0

            _requestThread = new Thread(new ThreadStart(ProcessThreadRequest));

            _requestThread.Start();

            // 4.5

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
        }

        protected void ProcessThreadRequest()
        {
            try
            {

                var callback = new AsyncCallback(ListenerCallback);

                while (true)
                {

                    IAsyncResult result = _listener.BeginGetContext(callback, _listener);

                    result.AsyncWaitHandle.WaitOne();

                }

            }
            catch (Exception ex)
            {
                LogTo.ErrorException("ProcessThreadRequest Failed!", ex);
            }
        }

        protected void ListenerCallback(IAsyncResult result)
        {
            var listener = result.AsyncState as HttpListener;

            var context = listener.EndGetContext(result);

            this.HttpRequestRecieved(new HttpRequestEventArgs(context));

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
