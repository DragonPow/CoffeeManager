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

        [SetUp]
        public void SetUp()
        {

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
            if (data.NumberTable == -1)
            {
                tableVM.ListTable = null;
            }
            else
            {
                tableVM.ListTable = new System.Collections.ObjectModel.ObservableCollection<TABLECUSTOM>();
                for (int i = 0; i < data.NumberTable; i++)
                {
                    tableVM.ListTable.Add(new TABLECUSTOM()
                    {
                        table = new TABLE()
                        {
                            ID = i,
                            CurrentStatus = data.Status
                        }
                    });
                }
            }

            if (data.NumberTable < 1)
            {
                Assert.Throws<ArgumentException>(() => tableVM.Delete(), "Not table to delete");
                return;
            }

            //mockSetTABLE.Setup(m => m.Max(i => i.ID)).Returns(tableVM.ListTable[tableVM.ListTable.Count - 1].table.ID);
            tableVM.CurrentTable = tableVM.ListTable[tableVM.ListTable.Count - 1];
            tableVM.CurrentTable.table.CurrentStatus = data.Status;
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
