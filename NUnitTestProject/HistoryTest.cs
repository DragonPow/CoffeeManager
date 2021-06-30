using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MainProject.ViewModel;
using MainProject;
using Moq;
using MainProject.Model;
using System.Transactions;
using System.Data.Entity;
using System.Linq;

namespace NUnitTestProject
{
    [TestFixture]
    public class HistoryTest
    {
        HistoryViewModel historyVM;

        [SetUp]
        public void Setup()
        {
            historyVM = new HistoryViewModel();

            //Setup data BILLs
            var dataBILL = new List<BILL>
            {
                new BILL() {CheckoutDay = DateTime.Now, ID_Table = 1, TotalPrice = 10000},
                new BILL() {CheckoutDay = DateTime.Now, ID_Table = 2, TotalPrice = 20000},
                new BILL() {CheckoutDay = DateTime.Now.AddDays(-1), ID_Table = 1, TotalPrice = 30000},
                new BILL() {CheckoutDay = DateTime.Now.AddDays(1), ID_Table = 1, TotalPrice = 15000},
                new BILL() {CheckoutDay = DateTime.Now.AddMonths(-1), ID_Table = 0, TotalPrice = 22000},
            }.AsQueryable();

            //Setup data TABLEs
            var dataTABLE = new List<TABLE>
            {
                new TABLE() {ID = 1, Name = 1},
                new TABLE() {ID = 2, Name = 2}
            }.AsQueryable();

            //Config BILL
            var mockSetBILL = new Mock<DbSet<BILL>>();
            mockSetBILL.As<IQueryable<BILL>>().Setup(m => m.Provider).Returns(dataBILL.Provider);
            mockSetBILL.As<IQueryable<BILL>>().Setup(m => m.Expression).Returns(dataBILL.Expression);
            mockSetBILL.As<IQueryable<BILL>>().Setup(m => m.ElementType).Returns(dataBILL.ElementType);
            mockSetBILL.As<IQueryable<BILL>>().Setup(m => m.GetEnumerator()).Returns(dataBILL.GetEnumerator());

            //Config TABLE
            var mockSetTABLE = new Mock<DbSet<TABLE>>();
            mockSetTABLE.As<IQueryable<TABLE>>().Setup(m => m.Provider).Returns(dataTABLE.Provider);
            mockSetTABLE.As<IQueryable<TABLE>>().Setup(m => m.Expression).Returns(dataTABLE.Expression);
            mockSetTABLE.As<IQueryable<TABLE>>().Setup(m => m.ElementType).Returns(dataTABLE.ElementType);
            mockSetTABLE.As<IQueryable<TABLE>>().Setup(m => m.GetEnumerator()).Returns(dataTABLE.GetEnumerator());

            //Init Data
            var mockContext = new Mock<mainEntities>();
            mockContext.Setup(m => m.BILLs).Returns(mockSetBILL.Object);
            mockContext.Setup(m => m.TABLEs).Returns(mockSetTABLE.Object);
            historyVM.db = mockContext.Object;
        }

        [TearDown]
        public void TearDown()
        {
            
        }

        [Test]
        public void TestSearchBill([ValueSource("_testData")]TestData testData)
        {
            historyVM.BeginTime = testData.StartDate;
            historyVM.EndTime = testData.EndDate;
            //if (testData.StartDate == null || testData.EndDate == null)
            //{
            //    var rs = Assert.Throws<ArgumentNullException>(() => historyVM.Search_Bill());
            //    Assert.That(rs.Message, Is.EqualTo("BeginTime or EndTime is null"));
            //    return;
            //}
            if (testData.StartDate.Date > testData.EndDate.Date)
            {
                //Check if start > end, throw exception
                var rs = Assert.Throws<ArgumentException>(() => historyVM.Search_Bill());
                Assert.That(rs.Message, Is.EqualTo("BeginTime is greater than EndTime"));
                return;
            }

            //Run method to check
            historyVM.Search_Bill();
            
            if (historyVM.ListBill == null)
            {
                Assert.Fail("List Bill is null");
            }

            for (int i = 0; i < historyVM.NumberAllPage; i++)
            {
                historyVM.NumberPage = i + 1;
                Console.WriteLine("Number of bill in Page {0} is: {1}", historyVM.NumberPage, historyVM.ListBill.Count);

                Assert.GreaterOrEqual(testData.EndDate.Date, historyVM.ListBill[0].CheckoutDay.Date);
                Assert.LessOrEqual(testData.StartDate.Date, historyVM.ListBill[historyVM.ListBill.Count - 1].CheckoutDay.Date);
            }
        }

        public class TestData
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        private static TestData[] _testData = new[]
        {
            //new TestData() {StartDate = default, EndDate = default},
            //new TestData() {StartDate = default, EndDate = DateTime.Now},
            //new TestData() {StartDate = DateTime.Now, EndDate = default},
            new TestData() {StartDate = DateTime.Now, EndDate = DateTime.Now},
            new TestData() {StartDate = DateTime.Now.AddDays(-1), EndDate = DateTime.Now},
            new TestData() {StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1)},
            new TestData() {StartDate = DateTime.Now.AddDays(1), EndDate = DateTime.Now},
            new TestData() {StartDate = DateTime.Now.AddDays(2), EndDate = DateTime.Now},
            new TestData() {StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(-1)},
            new TestData() {StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(-2)},
            new TestData() {StartDate = DateTime.Now.AddMonths(1), EndDate = DateTime.Now}
        };
    }
}
