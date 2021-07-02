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
                    new PRODUCT(){Name ="Trà đào", Price =12000, IsProvided = true, ID = 1 , TYPE_PRODUCT = new TYPE_PRODUCT(){Type = "Giải khát", ID = 1} }
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
               

                viewmodel = new ManageProductviewModel(mainvm);
                viewmodel.db = mockcontext.Object;
                viewmodel.MainVM.Productviewmodel.Context = mockcontext.Object;
            }



            [TestCase("")]
            [TestCase("Giải khát")]
            [TestCase("Trà")]
            public void TestAddCategory(string Name)
            {
                viewmodel.MainVM.CurrentTypeInHome = viewmodel.MainVM.Productviewmodel.Type = new TYPE_PRODUCT() {  Type = "Tất cả" };
                viewmodel.NameNewTypeProduct = Name;

                if (Name == "Giải khát")
                {
                    Assert.Throws<ArgumentException>(() => viewmodel.AddEditCategory(), "Category is existing", "NameExisting");
                    return;
                }
                if (Name == "")
                {
                    Assert.Throws<ArgumentException>(() =>viewmodel.AddEditCategory(), "Name category is empty", "NameEmpty");
                    return;
                }

                viewmodel.AddEditCategory();




                mockcontext.Verify(m => m.TYPE_PRODUCT.Add(It.IsAny<TYPE_PRODUCT>()), Times.Once);
                mockcontext.Verify(m => m.SaveChanges(), Times.Once);
            }
        }
    }
}
