using NUnit.Framework;
using MainProject;
using MainProject.ViewModel;
using MainProject.Model;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Data.Entity;
using MainProject.MainWorkSpace.Bill;

namespace NUnitTestProject
{
    [TestFixture]
    public class PaymentTest
    {
        TABLECUSTOM table;
        //TableViewModel tableVM;
        BillViewModel billVM;
        private List<PRODUCT> listProduct;

        Mock<DbSet<BILL>> mockSetBILL;
        Mock<DbSet<DETAILBILL>> mockSetDETAILBILL;
        Mock<mainEntities> mockContext;

        [SetUp]
        public void Setup()
        {
            //tableVM = new TableViewModel();
            table = new TABLECUSTOM();
            //tableVM.CurrentTable = table;
            billVM = new BillViewModel(table);

            //Setup data PRODUCT
            listProduct = new List<PRODUCT>
            {
                new PRODUCT() {ID = 1, Name = "product1", Price = 10000 },
                new PRODUCT() {ID = 2, Name = "product2", Price = 20000 },
                new PRODUCT() {ID = 3, Name = "product3", Price = 15000 }
            };
            var dataPRODUCT = listProduct.AsQueryable();

            //Config PRODUCT
            var mockSetPRODUCT = new Mock<DbSet<PRODUCT>>();
            mockSetPRODUCT.As<IQueryable<PRODUCT>>().Setup(m => m.Provider).Returns(dataPRODUCT.Provider);
            mockSetPRODUCT.As<IQueryable<PRODUCT>>().Setup(m => m.Expression).Returns(dataPRODUCT.Expression);
            mockSetPRODUCT.As<IQueryable<PRODUCT>>().Setup(m => m.ElementType).Returns(dataPRODUCT.ElementType);
            mockSetPRODUCT.As<IQueryable<PRODUCT>>().Setup(m => m.GetEnumerator()).Returns(dataPRODUCT.GetEnumerator());

            mockSetBILL = new Mock<DbSet<BILL>>();
            mockSetDETAILBILL = new Mock<DbSet<DETAILBILL>>();

            //Init Data
            mockContext = new Mock<mainEntities>();
            mockContext.Setup(m => m.PRODUCTs).Returns(mockSetPRODUCT.Object);
            mockContext.Setup(m => m.BILLs).Returns(mockSetBILL.Object);
            mockContext.Setup(m => m.DETAILBILLs).Returns(mockSetDETAILBILL.Object);
            mockContext.Setup(m => m.SetUnchanged(It.IsAny<object>()));
            //tableVM.Context = mockContext.Object;
            billVM.Context = mockContext.Object;
        }

        [TestCase(3, 2)]
        [TestCase(1, 1)]
        [TestCase(1, 1)]
        [TestCase(1, 1)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0, 0)]
        public void TestTotalPrice_of_Bill(int quantity1, int quantity2)
        {
            table.ListPro.Add(createDetailProduct(listProduct[0], quantity1));
            table.ListPro.Add(createDetailProduct(listProduct[1], quantity2));

            Assert.AreEqual(listProduct[0].Price * quantity1 + listProduct[1].Price * quantity2, table.Total);
        }

        private DetailPro createDetailProduct(PRODUCT pro, int quantity)
        {
            var product = new DetailPro();
            product.Pro = pro;
            product.Quantity = quantity;
            return product;
        }

        [TestCase(0, 1)]
        [TestCase(10000, 2)]
        [TestCase(10000, 0)]
        [TestCase(0, 1)]
        public void TestTotalPrice_of_Product(int price, int quantity)
        {
            DetailPro product = new DetailPro();
            product.Pro = new PRODUCT();
            product.Pro.Price = price;
            product.Quantity = quantity;
            Assert.AreEqual(price * quantity, product.Total);
        }

        [TestCase(1, 0)]
        [TestCase(2, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        public void TestPayment(int numberProduct, int tableName)
        {
            billVM.CurrentTable.table = new TABLE();
            billVM.CurrentTable.table.Name = tableName;
            for (int i = 0; i < numberProduct; i++)
            {
                var product = listProduct[i];
                billVM.CurrentTable.ListPro.Add(createDetailProduct(product, i));
            }
            billVM.Payment();

            mockSetBILL.Verify(m => m.Add(It.IsAny<BILL>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);

            //mockContext.Verify(m => m.DETAILBILLs.Add(It.IsAny<DETAILBILL>()), Times.Exactly(numberProduct));
            //mockSetDETAILBILL.Verify(m => m.Add(It.IsAny<DETAILBILL>()), Times.Exactly(numberProduct));
        }
    }
}