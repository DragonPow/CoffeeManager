using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MainProject.ViewModel;
using Moq;
using MainProject.Model;
using System.Data.Entity;
using System.Linq;
using MainProject.MainWorkSpace;

namespace NUnitTestProject
{
    class ManageProductTest
    {
        [TestFixture]
        class AddEditCategory
        {
            ManageProductviewModel viewmodel;

            Mock<mainEntities> mockcontext;
            Mock<DbSet<TYPE_PRODUCT>> mockSetType_product;
            Mock<DbSet<PRODUCT>> mockSetProduct;

            List<TYPE_PRODUCT> listType;
            List<PRODUCT> listpro;

            [SetUp]

            public void Setup()
            {
                
                //set up data for listType
                listType = new List<TYPE_PRODUCT>()
                {
                    new TYPE_PRODUCT(){Type = "Giải khát", ID = 1}             
                };

                var DataType_Product = listType.AsQueryable();

                listpro = new List<PRODUCT>()
                {
                    new PRODUCT(){Name ="Trà đào", Price =12000, IsProvided = true, ID = 1 ,ID_Type = 1}
                };

                var DataProduct = listpro.AsQueryable();

                //CONFIG TYPE_PRODUCT
                mockSetType_product = new Mock<DbSet<TYPE_PRODUCT>>();
                mockSetType_product.As<IQueryable<TYPE_PRODUCT>>().Setup(m => m.Provider).Returns(DataType_Product.Provider);
                mockSetType_product.As<IQueryable<TYPE_PRODUCT>>().Setup(m => m.Expression).Returns(DataType_Product.Expression);
                mockSetType_product.As<IQueryable<TYPE_PRODUCT>>().Setup(m => m.ElementType).Returns(DataType_Product.ElementType);
                mockSetType_product.As<IQueryable<TYPE_PRODUCT>>().Setup(m => m.GetEnumerator()).Returns(DataType_Product.GetEnumerator());

                //CONFIG PRODUCT
                mockSetProduct = new Mock<DbSet<PRODUCT>>();
                mockSetProduct.As<IQueryable<PRODUCT>>().Setup(m => m.Provider).Returns(DataProduct.Provider);
                mockSetProduct.As<IQueryable<PRODUCT>>().Setup(m => m.Expression).Returns(DataProduct.Expression);
                mockSetProduct.As<IQueryable<PRODUCT>>().Setup(m => m.ElementType).Returns(DataProduct.ElementType);
                mockSetProduct.As<IQueryable<PRODUCT>>().Setup(m => m.GetEnumerator()).Returns(DataProduct.GetEnumerator());

                //init data

                mockcontext = new Mock<mainEntities>();
                mockcontext.Setup(m => m.TYPE_PRODUCT).Returns(mockSetType_product.Object);
                mockcontext.Setup(m => m.PRODUCTs).Returns(mockSetProduct.Object);

                MainViewModel mainvm = new MainViewModel();
                mainvm.ListType = new System.Collections.ObjectModel.ObservableCollection<TYPE_PRODUCT>(listType);
                mainvm.Productviewmodel = new ProductViewModel();
                mainvm.CurrentTypeInHome =  mainvm.Productviewmodel.Type = new TYPE_PRODUCT() { Type = "Tất cả" };

                viewmodel = new ManageProductviewModel(mainvm);
                viewmodel.db = mockcontext.Object;
                viewmodel.MainVM.Productviewmodel.Context = mockcontext.Object;
            }

           
            [TestCase("")]
            [TestCase("Trà")]
            [TestCase("Giải khát")]
            public void TestAddCategory(string Name)
            {
                viewmodel.NameNewTypeProduct = Name;
                viewmodel.AddEditCategory();
                
                if (Name == "")
                {
                    var rs = Assert.Throws<ArgumentNullException>(() => viewmodel.AddEditCategory());
                    Assert.That(rs.Message, Is.EqualTo("Name category is empty"));
                    return;
                }
                if (Name == "Giải khát")
                {
                    var rs = Assert.Throws<ArgumentException>(() => viewmodel.AddEditCategory());
                    Assert.That(rs.Message, Is.EqualTo("Category is existing"));
                    return;
                }

                Assert.Equals(Name, viewmodel.MainVM.ListType.Last().Type);
            }
        }
    }
}
