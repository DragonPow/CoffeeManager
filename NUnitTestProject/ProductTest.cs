using MainProject.Model;
using MainProject.ViewModel;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace NUnitTestProject
{
    [TestFixture]
    public class ProductTest
    {
        static ProductViewModel productVM;

        static List<PRODUCT> listProduct;
        static List<TYPE_PRODUCT> listType;
        static Mock<mainEntities> mockContext;
        static Mock<DbSet<PRODUCT>> mockSetPRODUCT;
        static Mock<DbSet<TYPE_PRODUCT>> mockSetTYPEPRODUCT;

        [SetUp]
        public void SetUp()
        {
            productVM = new ProductViewModel();

            //Setup data TYPE_PRODUCT
            listType = new List<TYPE_PRODUCT>()
            {
                new TYPE_PRODUCT() {ID = 1, Type = "Trà"},
                new TYPE_PRODUCT() {ID = 2, Type = "Cà phê"}
            };
            var dataTYPEPRODUCT = listType.AsQueryable();

            //Setup data PRODUCT
            listProduct = new List<PRODUCT>()
            {
                new PRODUCT() {ID = 1, Name = "Trà sữa", Price = 2000, TYPE_PRODUCT = listType[0], ID_Type = 1, IsProvided = true}
            };
            var dataPRODUCT = listProduct.AsQueryable();

            //Config TYPE_PRODUCT
            mockSetTYPEPRODUCT = new Mock<DbSet<TYPE_PRODUCT>>();
            mockSetTYPEPRODUCT.As<IQueryable<TYPE_PRODUCT>>().Setup(m => m.Provider).Returns(dataTYPEPRODUCT.Provider);
            mockSetTYPEPRODUCT.As<IQueryable<TYPE_PRODUCT>>().Setup(m => m.Expression).Returns(dataTYPEPRODUCT.Expression);
            mockSetTYPEPRODUCT.As<IQueryable<TYPE_PRODUCT>>().Setup(m => m.ElementType).Returns(dataTYPEPRODUCT.ElementType);
            mockSetTYPEPRODUCT.As<IQueryable<TYPE_PRODUCT>>().Setup(m => m.GetEnumerator()).Returns(dataTYPEPRODUCT.GetEnumerator());

            //Config PRODUCT
            mockSetPRODUCT = new Mock<DbSet<PRODUCT>>();
            mockSetPRODUCT.As<IQueryable<PRODUCT>>().Setup(m => m.Provider).Returns(dataPRODUCT.Provider);
            mockSetPRODUCT.As<IQueryable<PRODUCT>>().Setup(m => m.Expression).Returns(dataPRODUCT.Expression);
            mockSetPRODUCT.As<IQueryable<PRODUCT>>().Setup(m => m.ElementType).Returns(dataPRODUCT.ElementType);
            mockSetPRODUCT.As<IQueryable<PRODUCT>>().Setup(m => m.GetEnumerator()).Returns(dataPRODUCT.GetEnumerator());

            //Init Data
            mockContext = new Mock<mainEntities>();
            mockContext.Setup(m => m.PRODUCTs).Returns(mockSetPRODUCT.Object);
            mockContext.Setup(m => m.TYPE_PRODUCT).Returns(mockSetTYPEPRODUCT.Object);
            productVM.Context = mockContext.Object;
        }
        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void TestAddDetailProToTable([ValueSource("_testData_AddProductToTable")] TestData data)
        {
            productVM.Tableviewmodel = new TableViewModel(mockContext.Object);
            productVM.Tableviewmodel.Currentlistdetailpro = new System.Collections.ObjectModel.ObservableCollection<MainProject.DetailPro>();
            productVM.Tableviewmodel.Currentlistdetailpro.Add(new MainProject.DetailPro(listProduct[0]));

            var product = new PRODUCT() { ID = data.ID, Name = data.Name, Price = data.Price };
            productVM.ListPoduct = new System.Collections.ObjectModel.ObservableCollection<PRODUCT>();
            productVM.ListPoduct.Add(product);
            productVM.Currentproduct = product;

            //Run the method
            productVM.AddDetailProToTable();

            if (data.Name == "Trà sữa")
            {
                Assert.AreEqual(2, productVM.Tableviewmodel.Currentlistdetailpro[0].Quantity);
                Assert.AreEqual(1, productVM.Tableviewmodel.Currentlistdetailpro.Count);
            }
            else
            {
                Assert.AreEqual(1, productVM.Tableviewmodel.Currentlistdetailpro[0].Quantity);
                Assert.AreEqual(1, productVM.Tableviewmodel.Currentlistdetailpro[1].Quantity);
                Assert.AreEqual(2, productVM.Tableviewmodel.Currentlistdetailpro.Count);
            }
        }

        [Test]
        public void TestAddProduct([ValueSource("_testData")] TestData data)
        {
            productVM.Newproduct = new PRODUCT();
            productVM.Newproduct.Name = data.Name;
            productVM.Newproduct.Price = data.Price;
            productVM.Newproduct.TYPE_PRODUCT = listType[0];

            productVM.ListPoduct = new System.Collections.ObjectModel.ObservableCollection<PRODUCT>();
            foreach (var i in listProduct)
            {
                productVM.ListPoduct.Add(i);
            }

            if (string.IsNullOrWhiteSpace(data.Name))
            {
                Assert.Throws<ArgumentException>(()=>productVM.Add(), "Name is empty", "NameNull");
                return;
            }
            if (data.Price < 0)
            {
                Assert.Throws<ArgumentException>(() => productVM.Add(), "Price is negative", "PriceNegative");
                return;
            }
            if (data.Price == 0)
            {
                Assert.Throws<ArgumentException>(() => productVM.Add(), "Price is zero", "Price0");
                return;
            }
            if (data.Name == "Trà sữa")
            {
                Assert.Throws<ArgumentException>(() => productVM.Add(), "Name of price is exsisted", "NameDuplicate");
                return;
            }

            productVM.Add();

            mockContext.Verify(m => m.PRODUCTs.Add(It.IsAny<PRODUCT>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        public class TestData
        {
            public int ID;
            public string Name;
            public int Price;
            //public string Description;
            //public TYPE_PRODUCT Type;
        }
        private static TestData[] _testData = new[]
        {
            new TestData() { Name = ""},
            new TestData() { Name = null},
            new TestData() { Name = "Trà đào", Price = -1},
            new TestData() { Name = "Trà đào", Price = 0},
            new TestData() { Name = "Trà sữa", Price = 1000},
            new TestData() { Name = "Trà đào", Price = 1000},
        };

        private static TestData[] _testData_AddProductToTable = new[]
        {
            new TestData() { ID = 1, Name = "Trà sữa", Price = 2000},
            new TestData() { ID = 2, Name = "Trà đào", Price = 1000},
        };
    }
}
