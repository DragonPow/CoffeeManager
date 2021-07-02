using MainProject.Model;
using MainProject.ViewModel;
using MainProject;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace NUnitTestProject
{
    [TestFixture]
    public class PayTest
    {
        static TableViewModel tableVM;

        static List<DetailPro> listProduct = new List<DetailPro>()
            {
                new DetailPro() { Quantity = 2, Pro = new PRODUCT() { Name = "Trà sữa", Price = 2000, TYPE_PRODUCT = new TYPE_PRODUCT() { ID = 1, Type = "Trà" }, IsProvided = true } }
            };
        static TABLECUSTOM Currenttable = new TABLECUSTOM() { table = new TABLE() { Name = 1, CurrentStatus = "Already" } };

        static Mock<mainEntities> mockContext;

        static Mock<DbSet<DetailPro>> mockSetPRODUCT;

        [SetUp]
        public void SetUp()
        {

            /*listProduct = new List<DetailPro>()
            {
                new DetailPro() { Quantity = 2, Pro = new PRODUCT() { Name = "Trà sữa", Price = 2000, TYPE_PRODUCT = new TYPE_PRODUCT() { ID = 1, Type = "Trà" }, IsProvided = true } }
            };
            Currenttable = new TABLECUSTOM() { table = new TABLE() { Name = 1, CurrentStatus = "Already" } };*/
            //Config PRODUCT
            // mockSetPRODUCT = new Mock<DbSet<DetailPro>>();
            // mockSetPRODUCT.As<IQueryable<DetailPro>>().Setup(m => m.Provider).Returns(dataPRODUCT.Provider);
            // mockSetPRODUCT.As<IQueryable<DetailPro>>().Setup(m => m.Expression).Returns(dataPRODUCT.Expression);
            // mockSetPRODUCT.As<IQueryable<DetailPro>>().Setup(m => m.ElementType).Returns(dataPRODUCT.ElementType);
            // mockSetPRODUCT.As<IQueryable<DetailPro>>().Setup(m => m.GetEnumerator()).Returns(dataPRODUCT.GetEnumerator());
            tableVM = new TableViewModel(true);


        }
        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void TestPayTable([ValueSource(typeof(TestData), nameof(TestData._testData))] TestData data)
        {     
            
           /* tableVM.Isbringtohome = data.IsBringHome;*/
            if (data.listPro != null) tableVM.Currentlistdetailpro = new ObservableCollection<DetailPro>(data.listPro);
            else
                tableVM.Currentlistdetailpro = new ObservableCollection<DetailPro>();

           /* tableVM.CurrentTable = data.table;*/

            if (data.table == null && !data.IsBringHome)
            {
                Assert.Throws<ArgumentException>(() => tableVM.Pay(data.IsBringHome, data.table), "Chưa chọn bàn!", "TableNULL");
                return;
            }
            if (data.listPro == null || data.listPro.Count == 0)
            {
                Assert.Throws<ArgumentException>(() => tableVM.Pay(data.IsBringHome, data.table), "Chưa chọn món!", "ListProNULL");
                return;
            }

            tableVM.Pay(data.IsBringHome, data.table);

        }

        public class TestData
        {
            public List<DetailPro> listPro ;
            public TABLECUSTOM table;
            public bool IsBringHome; 
            public static TestData[] _testData = new[]
            {
                new TestData() { listPro = listProduct, table = null, IsBringHome = true },
                new TestData() {listPro = null, table = Currenttable, IsBringHome = false  },
                new TestData() {listPro = listProduct, table = Currenttable, IsBringHome = false},
                new TestData() {listPro = listProduct, table = null, IsBringHome = false}
            };
        }
    }
}

