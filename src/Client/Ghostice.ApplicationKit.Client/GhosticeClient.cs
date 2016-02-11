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
using Anotar.NLog;

namespace Ghostice.ApplicationKit
{
    public class GhosticeClient
    {

        protected JsonRpcClient _client;

        protected ActionDispatcher _dispatcher;

        protected ApplicationUnderTest _application;

        public GhosticeClient()
        {

        }

        public void Connect(String serverUrl)
        {

            if (!Uri.IsWellFormedUriString(serverUrl.EndsWith("/") ? serverUrl : serverUrl += "/", UriKind.Absolute))
            {
                throw new ArgumentException(String.Format("ServerUrl Parameter value is Not a Well Formed Url!\r\nServerUrl: {0}", serverUrl));
            }

            _client = new JsonRpcClient(new Uri(serverUrl));

            _dispatcher = new ActionDispatcher(_client);

            _application = new ApplicationUnderTest(_dispatcher);

        }

        public void Disconnect()
        {
            _dispatcher = null;
        }

        public void Shutdown(ApplicationInfo application)
        {
            throw new NotImplementedException("Shutdown of SUT is not yet implemented.");
        }

        public ApplicationInfo Start(String applicationPath, String arguments, int timeoutSeconds)
        {

            Exception localException = null;

            JsonResponse<ApplicationInfo> result = null;

            AutoResetEvent completed = new AutoResetEvent(false);

            Stopwatch startWatch = new Stopwatch();

            startWatch.Start();

            var callback = _client.Invoke<ApplicationInfo>("Start", new String[] { applicationPath, arguments }, Scheduler.Default);

            using (callback.Subscribe(
                onNext: (response) =>
                {
                    LogTo.Debug(response.ToString());

                    result = response;
                },
                onError: (exception) =>
                {
                    localException = exception;
                    completed.Set();
                },
                onCompleted:
                    () =>
                    {
                        completed.Set();
                    }
                ))

            if (!completed.WaitOne(timeoutSeconds * 1000))
            {
                return ApplicationInfo.ReportFailed(applicationPath, arguments, String.Format("Timeout Waiting for Application {0} to Start after {1} second(s)", System.IO.Path.GetFileNameWithoutExtension(applicationPath), timeoutSeconds), startWatch.Elapsed);
            }
           
            if (result == null)
            {
                throw new ActionResultNullException(String.Format("Starting Application: {0} Aurguments: {1}\r\nServer Returned Null ActionResult!", System.IO.Path.GetFileName(applicationPath)), localException);
            }
            else if (result.Error != null)
            {
                throw new RemoteActionFailedException("Server Retured Request With Errors!", result.Error);
            }
            else
            {
                return ApplicationInfo.ReportStarted(result.Result.InstanceIdentifier, result.Result.ApplicationPath, arguments, Process.GetCurrentProcess().Id, startWatch.Elapsed);
            }

        }

        public ApplicationUnderTest Application
        {
            get { return _application; }
        }

    }

    [Serializable]
    public abstract class GhosticeClientException : Exception
    {
        protected GhosticeClientException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }


        public GhosticeClientException(String Message)
            : base(Message)
        {

        }

        public GhosticeClientException(String Message, Exception Inner)
            : base(Message, Inner)
        {

        }

    }

    [Serializable]
    public class ActionResultNullException : GhosticeClientException
    {
        protected ActionResultNullException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }


        public ActionResultNullException(String Message)
            : base(Message)
        {

        }

        public ActionResultNullException(String Message, Exception Inner)
            : base(Message, Inner)
        {

        }

    }

    [Serializable]
    public class RemoteActionFailedException : GhosticeClientException
    {
        protected RemoteActionFailedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }


        public RemoteActionFailedException(String Message)
            : base(Message)
        {

        }

        public RemoteActionFailedException(String Message, Exception Inner)
            : base(Message, Inner)
        {

        }

    }
}

