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

        static List<DetailPro> listProduct;
        static TABLECUSTOM Currenttable;

        static Mock<mainEntities> mockContext;
     
        static Mock<DbSet<DetailPro>> mockSetPRODUCT;

        [SetUp]
        public void SetUp()
        {
            tableVM = new TableViewModel();

            //Setup data TYPE_PRODUCT
            var Type = new TYPE_PRODUCT() { ID = 1, Type = "Trà" };

            listProduct = new List<DetailPro>()
            {
                new DetailPro() {Quantity=2, Pro=new PRODUCT() {Name = "Trà sữa", Price = 2000, TYPE_PRODUCT = Type, ID_Type = 1, IsProvided = true} }
            };
           // var dataPRODUCT = listProduct.AsQueryable();

            Currenttable = new TABLECUSTOM() { table = new TABLE() { Name = 1, CurrentStatus = "Already" } };



            //Config PRODUCT
           // mockSetPRODUCT = new Mock<DbSet<DetailPro>>();
           // mockSetPRODUCT.As<IQueryable<DetailPro>>().Setup(m => m.Provider).Returns(dataPRODUCT.Provider);
           // mockSetPRODUCT.As<IQueryable<DetailPro>>().Setup(m => m.Expression).Returns(dataPRODUCT.Expression);
           // mockSetPRODUCT.As<IQueryable<DetailPro>>().Setup(m => m.ElementType).Returns(dataPRODUCT.ElementType);
           // mockSetPRODUCT.As<IQueryable<DetailPro>>().Setup(m => m.GetEnumerator()).Returns(dataPRODUCT.GetEnumerator());


        }
        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void TestPayTable([ValueSource("_testData")] TestData data)
        {
            tableVM.Isbringtohome = false;
            tableVM.CurrentTable = data.table;

            tableVM.Currentlistdetailpro = new System.Collections.ObjectModel.ObservableCollection<DetailPro>();
            foreach (var i in data.listPro)
            {
                tableVM.Currentlistdetailpro.Add(i);
            }

            if (data.table==null)
            {
                Assert.Throws<ArgumentException>(() => tableVM.Pay(), "Chưa chọn bàn!", "TableNULL");
                return;
            }
            if (data.listPro==null)
            {
                Assert.Throws<ArgumentException>(() => tableVM.Pay(), "Chưa chọn món!", "ListProNULL");
                return;
            }
          
            tableVM.Pay();

        }

        public class TestData
        {
            public List<DetailPro> listPro;
            public TABLECUSTOM table;
        }
        private static TestData[] _testData = new[]
        {
            new TestData() { listPro = null, table=null},
            new TestData() {listPro = null, table=Currenttable},
            new TestData() {listPro = listProduct, table=Currenttable},
            
        };
    }
}
}
