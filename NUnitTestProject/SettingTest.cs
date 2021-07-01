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

            List<PARAMETER> parameter;

            [SetUp]
            public void Setup()
            {
                viewmodel = new SettingViewModel();

                //setup data for PARAMETER 

                parameter = new List<PARAMETER>() {
                    new PARAMETER(){ NAME = "StoreName", Value = "10 Diem" },
                    new PARAMETER(){NAME = "StorePhone", Value ="0348771xxx"},
                     new PARAMETER(){NAME = "StoreAddress", Value ="Dĩ An, Bình Dương"},
                };

                var DataParameter = parameter.AsQueryable();

                //CONFIG PARAMETER
                var mockSetparameter = new Mock<DbSet<PARAMETER>>();
                mockSetparameter.As<IQueryable<PARAMETER>>().Setup(m => m.Provider).Returns(DataParameter.Provider);
                mockSetparameter.As<IQueryable<PARAMETER>>().Setup(m => m.Expression).Returns(DataParameter.Expression);
                mockSetparameter.As<IQueryable<PARAMETER>>().Setup(m => m.ElementType).Returns(DataParameter.ElementType);
                mockSetparameter.As<IQueryable<PARAMETER>>().Setup(m => m.GetEnumerator()).Returns(DataParameter.GetEnumerator());

                //init data
                mockContext = new Mock<mainEntities>();
                mockContext.Setup(m => m.PARAMETERs).Returns(mockSetparameter.Object);
                mockContext.Setup(m => m.SetUnchanged(It.IsAny<object>()));
                viewmodel.context = mockContext.Object;
            }

            [TestCase()] 

            public void TestSaveDataStore(string name, string phonenumber, string address )
            {

            }


        }

    }
}
