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
using System;

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

        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(0, 0, 1, 0, 1)]
        [TestCase(0, 0, 1, 0, 0)]
        [TestCase(0, 0, 0, 0, 1)]
        [TestCase(0, 0, 1, 0, 1)]
        [TestCase(10000, 10000, 1, 20000, 0)]
        [TestCase(20000, 10000, 0, 20000, 1)]
        [TestCase(30000, 10000, 1, 20000, 1)]
        [TestCase(70000, 10000, 3, 20000, 2)]

        public void TestTotalPrice(int total, int price1, int quantity1, int price2, int quantity2)
        {
            table.ListPro.Add(createDetailProduct(price1, quantity1));
            table.ListPro.Add(createDetailProduct(price2, quantity2));

            Assert.AreEqual(total, table.Total);
        }

        private DetailPro createDetailProduct(PRODUCT pro, int quantity)
        {
            var product = new DetailPro();
            product.Pro = pro;
            product.Quantity = quantity;
            return product;
        }
        private DetailPro createDetailProduct(int price, int quantity)
        {
            DetailPro product = new DetailPro();
            product.Pro = new PRODUCT();
            product.Pro.Price = price;
            product.Quantity = quantity;
            return product;
        }

        [TestCase(1, 0, 10000)]
        [TestCase(2, 0, 30000)]
        [TestCase(1, 1, 5000)]
        [TestCase(2, 1, 10000)]
        [TestCase(2, 1, 10000)]
        [TestCase(2, 1, 40000)]
        public void TestPayment(int numberProduct, int tableName, long moneyCustomer)
        {
            billVM.CurrentTable.table = new TABLE();
            billVM.CurrentTable.table.Name = tableName;
            billVM.GiveMoney = moneyCustomer;

            long totalPrice = 0;

            for (int i = 0; i < numberProduct; i++)
            {
                var product = listProduct[i];
                billVM.CurrentTable.ListPro.Add(createDetailProduct(product, i));
                totalPrice += (long)product.Price;
            }

            billVM.Total = totalPrice;

            if (moneyCustomer < totalPrice)
            {
                Assert.Throws<InvalidOperationException>(() => billVM.Payment());
                return;
            }

            billVM.Payment();

            mockSetBILL.Verify(m => m.Add(It.IsAny<BILL>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.AreEqual(moneyCustomer - totalPrice, billVM.Refund);

            //mockContext.Verify(m => m.DETAILBILLs.Add(It.IsAny<DETAILBILL>()), Times.Exactly(numberProduct));
            //mockSetDETAILBILL.Verify(m => m.Add(It.IsAny<DETAILBILL>()), Times.Exactly(numberProduct));
        }
    }
}