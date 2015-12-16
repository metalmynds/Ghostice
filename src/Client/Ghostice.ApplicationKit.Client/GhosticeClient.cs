using AustinHarris.JsonRpc;
using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Anotar.Log4Net;

namespace Ghostice.ApplicationKit
{
    public class GhosticeClient
    {

        protected JsonRpcClient _client;

        protected ActionDispatcher _dispatcher;

        protected ApplicationControl _application;

        public GhosticeClient()
        {

        }

        public void Connect(String ServerUrl)
        {

            if (!Uri.IsWellFormedUriString(ServerUrl.EndsWith("/") ? ServerUrl : ServerUrl += "/", UriKind.Absolute))
            {
                throw new ArgumentException(String.Format("ServerUrl Parameter value is Not Well Formed!\r\nServerUrl: {0}", ServerUrl));
            }

            _client = new JsonRpcClient(new Uri(ServerUrl));

            _dispatcher = new ActionDispatcher(_client);

            _application = new ApplicationControl(_dispatcher);

        }

        public void Disconnect()
        {
            _dispatcher = null;
        }

        public void Shutdown(ApplicationInfo Application)
        {
            throw new NotImplementedException("Shutdown of SUT is not yet implemented.");
        }

        public ApplicationInfo Start(String ApplicationPath, String Arguments, int TimeoutSeconds)
        {

            Exception error = null;

            JsonResponse<ApplicationInfo> result = null;

            AutoResetEvent completed = new AutoResetEvent(false);

            Stopwatch startWatch = new Stopwatch();

            startWatch.Start();

            var callback = _client.Invoke<ApplicationInfo>("Start", new String[] { ApplicationPath, Arguments }, Scheduler.Default);

            using (callback.Subscribe(
                onNext: (response) =>
                {
                    LogTo.Debug(response.ToString());

                    result = response;
                },
                onError: (exception) =>
                {

                    error = exception;
                    completed.Set();
                },
                onCompleted:
                    () =>
                    {
                        completed.Set();
                    }
                ))
            {


                if (!completed.WaitOne(TimeoutSeconds * 1000))
                {
                    return ApplicationInfo.ReportFailed(ApplicationPath, Arguments, String.Format("Timeout Waiting for Application Start after {0} second(s)", TimeoutSeconds), startWatch.Elapsed);
                }

                if (result.Error != null)
                {
                    //throw new GhosticeClientException(String.Format("Ghostice Start Application Failed!\r\nApplication Path: {0}\r\nArguments: {1}\r\nResponse:\r\n{2}", ApplicationPath, Arguments, result.Error.data));
                    throw new GhosticeClientException(result.Error.data.ToString());
                }

                return ApplicationInfo.ReportStarted(ApplicationPath, Arguments, Process.GetCurrentProcess().Id, startWatch.Elapsed);

            }
        }

        public ApplicationControl Application
        {
            get { return _application; }
        }

    }

    [Serializable]
    public class GhosticeClientException : Exception
    {
        protected GhosticeClientException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }


        public GhosticeClientException(String Message)
            : base(Message)
        {

        }

        public GhosticeClientException(String Message, Exception Inner)
            : base(Message, Inner)
        {

        }

    }
}

