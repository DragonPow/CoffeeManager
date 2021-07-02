using MainProject.Model;
using MainProject.ViewModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using System.Data.Entity;
using System.Linq;

namespace NUnitTestProject
{
    [TestFixture]
    public class TableTest
    {
        static TableViewModel tableVM;
        static Mock<mainEntities> mockContext;
        static Mock<DbSet<TABLE>> mockSetTABLE;
        static Mock<DbSet<STATUS_TABLE>> mockSetSTATUSTABLE;
        static List<STATUS_TABLE> listStatus;
        static List<TABLE> listTable;

        [SetUp]
        public void SetUp()
        {
            //Setup data TABLE
            listTable = new List<TABLE>()
            {
                new TABLE() {ID = 1, Name = 1, ID_Status = 2},
                new TABLE() {ID = 2, Name = 2, ID_Status = 2},
            };
            var dataTABLE = listTable.AsQueryable();

            //Setup data STATUS_TABLE
            listStatus = new List<STATUS_TABLE>()
            {
                new STATUS_TABLE() {ID = 1, Status = "Fix"},
                new STATUS_TABLE() {ID = 2, Status = "Normal"},
                new STATUS_TABLE() {ID = 3, Status = "Already"},
            };
            var dataSTATUSTABLE = listStatus.AsQueryable();

            //Config TABLE
            mockSetTABLE = new Mock<DbSet<TABLE>>();
            mockSetTABLE.As<IQueryable<TABLE>>().Setup(m => m.Provider).Returns(dataTABLE.Provider);
            mockSetTABLE.As<IQueryable<TABLE>>().Setup(m => m.Expression).Returns(dataTABLE.Expression);
            mockSetTABLE.As<IQueryable<TABLE>>().Setup(m => m.ElementType).Returns(dataTABLE.ElementType);
            mockSetTABLE.As<IQueryable<TABLE>>().Setup(m => m.GetEnumerator()).Returns(dataTABLE.GetEnumerator());

            //Config STATUS_TABLE
            mockSetSTATUSTABLE = new Mock<DbSet<STATUS_TABLE>>();
            mockSetSTATUSTABLE.As<IQueryable<STATUS_TABLE>>().Setup(m => m.Provider).Returns(dataSTATUSTABLE.Provider);
            mockSetSTATUSTABLE.As<IQueryable<STATUS_TABLE>>().Setup(m => m.Expression).Returns(dataSTATUSTABLE.Expression);
            mockSetSTATUSTABLE.As<IQueryable<STATUS_TABLE>>().Setup(m => m.ElementType).Returns(dataSTATUSTABLE.ElementType);
            mockSetSTATUSTABLE.As<IQueryable<STATUS_TABLE>>().Setup(m => m.GetEnumerator()).Returns(dataSTATUSTABLE.GetEnumerator());

            //Init data
            mockContext = new Mock<mainEntities>();
            mockContext.Setup(m => m.TABLEs).Returns(mockSetTABLE.Object);
            mockContext.Setup(m => m.STATUS_TABLE).Returns(mockSetSTATUSTABLE.Object);

            tableVM = new TableViewModel(mockContext.Object);
        }

        [Test]
        public void TestDeleteTable([ValueSource("_testData")] TestData data)
        {
            if (data.NumberTable < 1)
            {
                tableVM.ListTable = null;
                Assert.Throws<ArgumentException>(() => tableVM.Delete(), "Not table to delete");
                return;
            }

            tableVM.ListTable = new System.Collections.ObjectModel.ObservableCollection<TABLECUSTOM>();
            for (int i = 0; i < data.NumberTable; i++)
            {
                tableVM.ListTable.Add(new TABLECUSTOM()
                {
                    table = listTable[i]
                });
            }
            tableVM.ListTable[data.NumberTable - 1].table.CurrentStatus = data.Status;

            //mockSetTABLE.Setup(m => m.Max(i => i.ID)).Returns(tableVM.ListTable[tableVM.ListTable.Count - 1].table.ID);
            //tableVM.CurrentTable = tableVM.ListTable[tableVM.ListTable.Count - 1];
            //tableVM.CurrentTable.table.CurrentStatus = data.Status;
            if (data.Status == "Already")
            {
                Assert.Throws<ArgumentException>(() => tableVM.Delete(), "The table is not payment");
                return;
            }

            tableVM.Delete();
            mockContext.Verify(m => m.TABLEs.Remove(It.IsAny<TABLE>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        public class TestData
        {
            public int NumberTable;
            public string Status;
        }
        private static TestData[] _testData = new[]
        {
            new TestData() {NumberTable = -1},
            new TestData() {NumberTable = 0},
            new TestData() {NumberTable = 1, Status = "Fix"},
            new TestData() {NumberTable = 1, Status = "Already"},
            new TestData() {NumberTable = 1, Status = "Normal"},
        };
    }
}
