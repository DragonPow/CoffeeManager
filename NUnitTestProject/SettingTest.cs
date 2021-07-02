using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MainProject.ViewModel;
using MainProject.Model;
using Moq;
using System.Linq;
using System.Data.Entity;

namespace NUnitTestProject
{
    class SettingTest
    {
        [TestFixture]
        class  Save_data_storeTest
        {
            SettingViewModel viewmodel;

            Mock<mainEntities> mockContext;
            Mock<DbSet<PARAMETER>> mockSetparameter;

           List<PARAMETER> parameter;

            [SetUp]
            public void Setup()
            {
               

                //setup data for PARAMETER 

                parameter = new List<PARAMETER>() {
                    new PARAMETER(){ NAME = "StoreName", Value = "10 Diem" },
                    new PARAMETER(){NAME = "StorePhone", Value ="0348771xxx"},
                    new PARAMETER(){NAME = "StoreAddress", Value ="Dĩ An, Bình Dương"},
                };

                var DataParameter = parameter.AsQueryable();

                //CONFIG PARAMETER
                mockSetparameter = new Mock<DbSet<PARAMETER>>();
                mockSetparameter.As<IQueryable<PARAMETER>>().Setup(m => m.Provider).Returns(DataParameter.Provider);
                mockSetparameter.As<IQueryable<PARAMETER>>().Setup(m => m.Expression).Returns(DataParameter.Expression);
                mockSetparameter.As<IQueryable<PARAMETER>>().Setup(m => m.ElementType).Returns(DataParameter.ElementType);
                mockSetparameter.As<IQueryable<PARAMETER>>().Setup(m => m.GetEnumerator()).Returns(DataParameter.GetEnumerator());

                //init data
                mockContext = new Mock<mainEntities>();
                mockContext.Setup(m => m.PARAMETERs).Returns(mockSetparameter.Object);
                mockContext.Setup(m => m.SetUnchanged(It.IsAny<object>()));

                viewmodel = new SettingViewModel(mockContext.Object);
            }

            [TestCase("", "031458715", "tp Hồ Chí Minh")]
            [TestCase("Ice Coffee", "", "tp Hồ Chí Minh")]
            [TestCase("Ice Coffee", "031458715", "")]
            [TestCase("Ice Coffee", "031458715", "tp Hồ Chí Minh")]

            public void TestSaveDataStore(string name, string phonenumber, string address )
            {
               
                viewmodel.Save_data_store();
                viewmodel.NameStore = name;
                viewmodel.NumberPhone = phonenumber;
                viewmodel.Address = address;

                if (name == "" || phonenumber == "" || address == "")
                {
                    //Check if have emplty value, throw exception
                    var rs = Assert.Throws<InvalidOperationException>(() => viewmodel.Save_data_store());
                    Assert.That(rs.Message, Is.EqualTo("Empty data!"));
                    return;
                }

                /*  mockSetparameter.Verify(m => m.Attach(It.IsAny<PARAMETER>()), Times.Once);*/
                mockContext.Verify(m => m.SaveChanges(), Times.Once);
            }


        }

    }
}
