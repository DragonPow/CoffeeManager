using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.IO;
using System.Threading;

namespace TestProject
{
    [TestClass]
    public class BaseTest
    {
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string WPFAppID = @"D:\Major\CoffeeManager\MainProject\bin\Debug\MainProject.exe";
        private const string apptest = "Microsoft.WindowsAlarms_8wekyb3d8bbwe!App";

        protected static WindowsDriver<WindowsElement> session;
        protected static WindowsDriver<WindowsElement> desktopSession;
        protected static RemoteTouchScreen touchScreen;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            if (desktopSession == null)
            {
                var options = new AppiumOptions();
                options.AddAdditionalCapability("app", "Root");
                //options.AddAdditionalCapability("appWorkingDir", "Root");
                desktopSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), options);
                //desktopSession.Keyboard.PressKey(OpenQA.Selenium.Keys.Command + "a" + OpenQA.Selenium.Keys.Command);
            }

            if (session == null || touchScreen == null)
            {
                TearDown();
                //AppiumOptions option = new AppiumOptions();
                //option.AddAdditionalCapability("platformName", "Windows");
                //option.AddAdditionalCapability("deviceName", "WindowsPC");
                //option.AddAdditionalCapability("app", "Root");


                var options = new AppiumOptions();
                options.AddAdditionalCapability("app", WPFAppID);
                options.AddAdditionalCapability("platformName", "Windows");
                options.AddAdditionalCapability("deviceName", "WindowsPC");
                options.AddAdditionalCapability("appWorkingDir", Path.GetDirectoryName(WPFAppID));

                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), options);
                Assert.IsNotNull(session);
                Assert.IsNotNull(session.SessionId);

                session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);

                touchScreen = new RemoteTouchScreen(session);
                Assert.IsNotNull(touchScreen);
            }
        }

        public static void TearDown()
        {
            touchScreen = null;
            if (session!=null)
            {
                session.Quit();
                session = null;
            }
        }

        [TestInitialize]
        public void TestInit()
        {

        }
    }
}
