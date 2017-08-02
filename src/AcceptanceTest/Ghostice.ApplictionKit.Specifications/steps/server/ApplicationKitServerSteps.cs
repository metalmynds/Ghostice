using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using TechTalk.SpecFlow;

namespace Ghostice.ApplictionKit.Specifications.steps.server
{
    [Binding]
    public class ApplicationKitServerSteps
    {
        private readonly FeatureContext _featureContext;
        private readonly ScenarioContext _scenarioContext;

        public const String GUI_SERVER_NAME = "WGhostice";
        public const String CONSOLE_SERVER_NAME = "CGhostice";


        public ApplicationKitServerSteps(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
        }

        [Given(@"I have the (.*) server installed")]
        public void GivenIHaveTheServerInstalled(String ui)
        {
            var path = String.Empty;

            if (ui == "Gui")
            {
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), GUI_SERVER_NAME + ".exe");
            }
            else if (ui == "Console")
            {
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), CONSOLE_SERVER_NAME + ".exe");
            }

            Assert.IsTrue(File.Exists(path), $"Ghostice Server {ui} at Location {path} Not Found!");

            _scenarioContext.Set<String>(path, "serverPath");

        }

        [Given(@"the endpoint (.*) is clear")]
        public void GivenTheEndpointIsClear(String endPoint)
        {
            var address = new Uri(endPoint);

            using (System.Net.Sockets.TcpClient client = new TcpClient())
            {

                client.Connect(address.Host, address.Port);

                Assert.Fail($"Unexpected connection to {endPoint} established!");

            }

            _scenarioContext.Set<String>(endPoint, "endPoint");

        }

        [When(@"I start the (.*) server")]
        public void WhenIStartTheServer(string ui)
        {
            ProcessStartInfo serverStartupInfo = new ProcessStartInfo(_scenarioContext.Get<String>("serverPath"));

            serverStartupInfo.Arguments = "-e " + _scenarioContext.Get<String>("endPoint");

            _scenarioContext.Set<DateTime>(DateTime.Now, "start");

            var ghostiseServer = Process.Start(serverStartupInfo);

            ghostiseServer.WaitForInputIdle();

            _scenarioContext.Set<DateTime>(DateTime.Now, "finish");

            ghostiseServer.CloseMainWindow();
        }

        [Then(@"the server should start in under (.*) seconds")]
        public void ThenTheServerShouldStartInUnderSeconds(int timeout)
        {
            var duration = _scenarioContext.Get<DateTime>("finish") - _scenarioContext.Get<DateTime>("start");

            Assert.IsTrue(duration.Seconds <= timeout, $"Server Failed to Start Promptly! Actual Time: {duration.Seconds}");
        }
    }
}
