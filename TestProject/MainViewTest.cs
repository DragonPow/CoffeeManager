using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TestProject
{
    [TestClass]
    public class MainViewTest : BaseTest
    {


        [TestMethod]
        public void TestAddSucessProduct_To_Table()
        {
            //Click to choose table
            session.FindElementByAccessibilityId("TableName").Click();
            var chooseTableSession = desktopSession.FindElementByClassName("SelectTableView");

            //Click into first button
            var listTable = chooseTableSession.FindElementsByClassName("ListBoxItem");
            listTable[0].Click();

            //Close window choos Table
            chooseTableSession.FindElementByClassName("Button").Click();

            var productContainer = session.FindElementByAccessibilityId("Main_View");
            var listProduct = productContainer.FindElementsByClassName("ListBoxItem").ToList();
            for (int i = 0; i < 2; i++)
            {
                listProduct[i].Click();
            }
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [TestInitialize]
        public void TestInit()
        {
            base.TestInit();

            session.FindElementByName("MainProject.MainWorkSpace.MainViewModel").Click();
        }
    }
}
