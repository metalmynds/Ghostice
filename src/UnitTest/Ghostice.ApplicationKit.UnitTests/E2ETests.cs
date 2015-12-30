using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Threading;
using Ghostice.ApplicationKit.Example.Models;
using Ghostice.Core;
using Ghostice.Core.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class E2ETests
    {
        public const String SERVER_NAME = "WGhostice";

        //private Process ghostiseServer;

        [TestInitialize]
        public void TestInitialize()
        {

            //    foreach (var serverProcess in Process.GetProcessesByName(SERVER_NAME))
            //    {
            //        try {

            //            serverProcess.Kill();

            //        } catch (Exception ex)
            //        {
            //            System.Diagnostics.Debug.WriteLine(ex.ToString());
            //        }
            //    }

            //    ProcessStartInfo serverStartupInfo = new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Path), SERVER_NAME + ".exe"));

            //    serverStartupInfo.Arguments = "-p 21555";

            //    ghostiseServer = Process.Start(serverStartupInfo);

            //    Assert.IsNotNull(ghostiseServer);

            //    ghostiseServer.WaitForInputIdle();


            InterfaceControlFactory.ControlConstructor = new InterfaceControlFactory.ConstructControl((controlType, parent, name) =>
            {

                Object[] parameters = new Object[1];

                parameters[0] = parent;

                if (InterfaceControlFactory.IsWindow(controlType))
                {

                    return InterfaceControlFactory.Create(controlType, parent);

                }
                else
                {
                    return ReflectionHelper.Instantiate(controlType.Assembly, controlType.FullName, parameters);
                }

            });

        }


        [TestMethod]
        public void StartExampleInServerAndCloseWindow()
        {

            Process ghostiseServer = null;

            try
            {

                ProcessStartInfo serverStartupInfo = new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SERVER_NAME + ".exe"));

                serverStartupInfo.Arguments = "-e http://localhost:21555";

                ghostiseServer = Process.Start(serverStartupInfo);

                Assert.IsNotNull(ghostiseServer);

                ghostiseServer.WaitForInputIdle();

                GhosticeClient client = new GhosticeClient();

                Assert.IsNotNull(client);

                client.Connect("http://localhost:21555");

                var applicationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Example.PetShop.WinForms.exe");

                var application = client.Start(applicationPath, String.Empty, 15);

                var mainWindow = InterfaceControlFactory.Create<MainFormWindow>(client.Application);

                Assert.IsNotNull(mainWindow);

                mainWindow.WaitForReady(30);

                mainWindow.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (ghostiseServer != null)
                {
                    ghostiseServer.Kill();
                }
            }
        }

        [TestMethod]
        public void StartExampleInServerSelectMenuFromMainMenuComponentAndCloseWindow()
        {

            Process ghostiseServer = null;

            try
            {

                ProcessStartInfo serverStartupInfo = new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SERVER_NAME + ".exe"));

                serverStartupInfo.Arguments = "-e http://localhost:21555";

                ghostiseServer = Process.Start(serverStartupInfo);

                Assert.IsNotNull(ghostiseServer);

                ghostiseServer.WaitForInputIdle();

                GhosticeClient client = new GhosticeClient();

                Assert.IsNotNull(client);

                client.Connect("http://localhost:21555");

                var applicationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Example.PetShop.WinForms.exe");

                var application = client.Start(applicationPath, String.Empty, 15);

                var mainWindow = InterfaceControlFactory.Create<MainFormWindow>(client.Application);

                Assert.IsNotNull(mainWindow);

                mainWindow.WaitForReady(30);

                mainWindow.MainMenu.Click(@"&File\Accessories");

                mainWindow.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (ghostiseServer != null)
                {
                    ghostiseServer.Kill();
                }
            }
        }

        [TestMethod]
        public void StartExampleInServerAndListWindows()
        {

            Process ghostiseServer = null;

            try
            {

                ProcessStartInfo serverStartupInfo = new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SERVER_NAME + ".exe"));

                serverStartupInfo.Arguments = "-e http://localhost:21555";

                ghostiseServer = Process.Start(serverStartupInfo);

                Assert.IsNotNull(ghostiseServer);

                ghostiseServer.WaitForInputIdle();

                GhosticeClient client = new GhosticeClient();

                Assert.IsNotNull(client);

                client.Connect("http://localhost:21555");

                var applicationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Example.PetShop.WinForms.exe");

                var application = client.Start(applicationPath, String.Empty, 15);

                var mainWindow = InterfaceControlFactory.Create<MainFormWindow>(client.Application);

                Assert.IsNotNull(mainWindow);

                mainWindow.WaitForReady(30);

                var windows = client.Application.GetWindows();

                mainWindow.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (ghostiseServer != null)
                {
                    ghostiseServer.Kill();
                }
            }
        }

        [TestMethod]
        public void StartExampleInServerAndPerformBasicOperationsAndCloseWindow()
        {

            Process ghostiseServer = null;

            try
            {

                // Start Server

                ProcessStartInfo serverStartupInfo = new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SERVER_NAME + ".exe"));

                serverStartupInfo.Arguments = "-e http://localhost:21599";

                ghostiseServer = Process.Start(serverStartupInfo);

                Assert.IsNotNull(ghostiseServer);

                ghostiseServer.WaitForInputIdle();


                // Start Client

                GhosticeClient client = new GhosticeClient();

                Assert.IsNotNull(client);

                client.Connect("http://localhost:21599");

                var applicationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Example.PetShop.WinForms.exe");

                client.Start(applicationPath, String.Empty, 15);

                var mainWindow = InterfaceControlFactory.Create<MainFormWindow>(client.Application);

                Assert.IsNotNull(mainWindow);

                mainWindow.WaitForReady(30);

                mainWindow.RegistrationHistory.SelectTab(1);

                mainWindow.Close();

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (ghostiseServer != null)
                {
                    ghostiseServer.Kill();
                }
            }
        }


        [TestMethod]
        public void StartExampleInServerAndMapMainForm()
        {
            Process ghostiseServer = null;

            try
            {


                // Start Server

                ProcessStartInfo serverStartupInfo = new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SERVER_NAME + ".exe"));

                serverStartupInfo.Arguments = "-e http://localhost:21600";

                ghostiseServer = Process.Start(serverStartupInfo);

                Assert.IsNotNull(ghostiseServer);

                ghostiseServer.WaitForInputIdle();


                // Start Client

                GhosticeClient client = new GhosticeClient();

                Assert.IsNotNull(client);

                client.Connect("http://localhost:21600");

                var applicationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Example.PetShop.WinForms.exe");

                client.Start(applicationPath, String.Empty, 15);

                // Setup Main Window

                var mainWindow = InterfaceControlFactory.Create<MainFormWindow>(client.Application);

                mainWindow.WaitForReady(30);

                // Map Controls on Form

                var mainWindowMap = mainWindow.PrintTree();

                Assert.IsNotNull(mainWindowMap);

                mainWindow.Close();

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (ghostiseServer != null)
                {
                    ghostiseServer.Kill();
                }
            }
        }


        [TestMethod]
        public void StartExampleInServerAndGetColumnsFromListViewViaExtensionMethod()
        {
            Process ghostiseServer = null;

            try
            {


                // Start Server

                ProcessStartInfo serverStartupInfo = new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SERVER_NAME + ".exe"));

                serverStartupInfo.Arguments = "-e http://localhost:21599";

                ghostiseServer = Process.Start(serverStartupInfo);

                Assert.IsNotNull(ghostiseServer);

                ghostiseServer.WaitForInputIdle();


                // Start Client

                GhosticeClient client = new GhosticeClient();

                Assert.IsNotNull(client);

                client.Connect("http://localhost:21599");

                var applicationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Example.PetShop.WinForms.exe");

                client.Start(applicationPath, String.Empty, 15);

                // Setup Main Window

                var mainWindow = InterfaceControlFactory.Create<MainFormWindow>(client.Application);

                mainWindow.WaitForReady(30);

                // GET ROWS OF LIST VIEW VIA EXTENSION METHOD (TESTING AppDomain Assembly Resolution)

                mainWindow.RegistrationHistory.SelectTab(1);

                var summary = mainWindow.RegistrationHistory.HistoryTab.Summary.Text;

                var columns = mainWindow.RegistrationHistory.HistoryTab.HistoryList.Columns;



                mainWindow.Close();

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (ghostiseServer != null)
                {
                    ghostiseServer.Kill();
                }
            }
        }


        [TestMethod]
        public void StartExampleInServerAndPerforMultipleActionsAndCloseWindow()
        {

            Process ghostiseServer = null;

            try
            {


                // Start Server

                ProcessStartInfo serverStartupInfo = new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SERVER_NAME + ".exe"));

                serverStartupInfo.Arguments = "-e http://localhost:21599";

                ghostiseServer = Process.Start(serverStartupInfo);

                Assert.IsNotNull(ghostiseServer);

                ghostiseServer.WaitForInputIdle();

                // Start Client

                GhosticeClient client = new GhosticeClient();

                Assert.IsNotNull(client);

                client.Connect("http://localhost:21599");

                var applicationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Example.PetShop.WinForms.exe");

                client.Start(applicationPath, String.Empty, 15);

                var mainWindow = InterfaceControlFactory.Create<MainFormWindow>(client.Application);

                mainWindow.WaitForReady(30);

                mainWindow.RegistrationHistory.SelectTab(1);

                var summary = mainWindow.RegistrationHistory.HistoryTab.Summary.Text;

                Assert.AreEqual<String>("History so far:", summary);

                mainWindow.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (ghostiseServer != null)
                {
                    ghostiseServer.Kill();
                }
            }
        }

        [TestMethod]
        public void StartExampleInServerAndFillRegistrationFormAndCloseWindow()
        {

            Process ghostiseServer = null;

            try
            {


                // Start Server

                ProcessStartInfo serverStartupInfo = new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SERVER_NAME + ".exe"));

                serverStartupInfo.Arguments = "-e http://localhost:21299";

                ghostiseServer = Process.Start(serverStartupInfo);

                Assert.IsNotNull(ghostiseServer);

                ghostiseServer.WaitForInputIdle();

                // Start Client

                GhosticeClient client = new GhosticeClient();

                Assert.IsNotNull(client);

                client.Connect("http://localhost:21299");

                var applicationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Example.PetShop.WinForms.exe");

                client.Start(applicationPath, String.Empty, 15);

                var mainWindow = InterfaceControlFactory.Create<MainFormWindow>(client.Application);

                mainWindow.WaitForReady(30);

                var summary = mainWindow.RegistrationHistory.HistoryTab.Summary.Text;

                Assert.AreEqual<String>("History so far:", summary);

                mainWindow.RegistrationHistory.RegistrationTab.Name.Text = "Dave";

                mainWindow.RegistrationHistory.RegistrationTab.Name.Text = "";

                mainWindow.RegistrationHistory.RegistrationTab.Name.Text = "Monti";

                mainWindow.RegistrationHistory.RegistrationTab.Type.Text = "Cat";

                mainWindow.RegistrationHistory.RegistrationTab.Eats.SelectItem("Carnivorous");

                mainWindow.RegistrationHistory.RegistrationTab.Eats.SelectItem(2);

                mainWindow.RegistrationHistory.RegistrationTab.Price.Text = "12.56";

                var rules = mainWindow.RegistrationHistory.RegistrationTab.Rules.Items;

                Assert.IsNotNull(rules, "Rules Are Null!");

                Assert.IsTrue(rules.Count == 4, "Rules Incorrect Length!");

                var selectedRules = mainWindow.RegistrationHistory.RegistrationTab.Rules.SelectedItems;

                Assert.IsNotNull(rules, "Selected Rules Is Null!");

                //mainWindow.RegistrationHistory.RegistrationTab.Rules.SelectedItems = new List<String>(new String[] { "Dangerous", "Sell In Pairs" });
                mainWindow.RegistrationHistory.RegistrationTab.Rules.SelectedItems = new List<String>(new String[] { "Dangerous", "Sell In Pairs" });

                mainWindow.RegistrationHistory.RegistrationTab.Save.Press();

                var historySummary = mainWindow.RegistrationHistory.HistoryTab.Summary.Text;

                Assert.AreEqual<String>("History so far:\r\nMonti the Cat registered at a price of £12.56. Food: Omnivorous", historySummary);

                mainWindow.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (ghostiseServer != null)
                {
                    ghostiseServer.Kill();
                }
            }
        }

        [TestMethod]
        public void PetShopWindowWinFormsGuiTest()
        {
            Process ghostiseServer = null;

            try
            {

                // Start Server                

                ProcessStartInfo serverStartupInfo = new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SERVER_NAME + ".exe"));

                serverStartupInfo.Arguments = "-e http://localhost:21299";

                ghostiseServer = Process.Start(serverStartupInfo);

                Assert.IsNotNull(ghostiseServer);

                ghostiseServer.WaitForInputIdle();

                // Start Client

                GhosticeClient client = new GhosticeClient();

                Assert.IsNotNull(client);

                client.Connect("http://localhost:21299");

                var applicationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Example.PetShop.WinForms.exe");

                client.Start(applicationPath, String.Empty, 15);

                var mainWindow = InterfaceControlFactory.Create<MainFormWindow>(client.Application);

                List<String> rules = new List<string>();

                rules.Add("Special Environment");

                mainWindow.RegisterAnimal("Foghorn Leghorn", "Large Bird", "Herbivorous", 69.68, rules.ToArray());

                mainWindow.ShowHistory();

                mainWindow.RegisterAnimal("Chickin Lic'in", "Small Bird", "Herbivorous", 666.99, rules.ToArray());

                mainWindow.ShowHistory();

                rules.Clear();

                rules.Add("Dangerous");

                rules.Add("Sell In Pairs");

                mainWindow.RegisterAnimal("Capistrano", "Cat", "Carnivorous", 9.99, rules.ToArray());

                mainWindow.ShowHistory();

                //mainWindow.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (ghostiseServer != null)
                {
                    ghostiseServer.Kill();
                }
            }

        }

        [TestCleanup]
        public void CleanUp()
        {

            //ghostiseServer.Kill();

        }
    }
}
