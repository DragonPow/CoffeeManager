using MainProject.ApplicationWorkSpace;
using MainProject.MainWorkSpace.Bill;
using MainProject.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MainProject.MainWorkSpace;
using MainProject.ViewModel;
using MainProject.MainWorkSpace.Product;
using System.IO;
using MainProject.HelperClass;

namespace MainProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", "../../");

            base.OnStartup(e);

            MessageBoxManager.Yes = "Đồng ý";
            MessageBoxManager.No = "Hủy";
            MessageBoxManager.Register();

            ////Testing Startup
            //Console.WriteLine(File.Exists(PrintPDF.dir_font));
            //using (var db = new mainEntities())
            //{
            //    PrintPDF.Instance.createBill(new BILL()
            //    {
            //        CheckoutDay = DateTime.Now,
            //        MoneyCustomer = 10000,
            //        TotalPrice = 5000,
            //        TABLE = db.TABLEs.Where(i=>i.ID == 2).FirstOrDefault(),
            //        DETAILBILLs = new System.Collections.ObjectModel.ObservableCollection<DETAILBILL>()
            //    {
            //        new DETAILBILL()
            //        {
            //            UnitPrice = 2000,
            //            Quantity = 1,
            //            PRODUCT = db.PRODUCTs.Where(i => i.ID == 1).FirstOrDefault()
            //        },
            //        new DETAILBILL()
            //        {
            //            UnitPrice = 1000,
            //            Quantity = 3,
            //            PRODUCT = db.PRODUCTs.Where(i => i.ID == 2).FirstOrDefault()
            //        },
            //        new DETAILBILL()
            //        {
            //            UnitPrice = 5000,
            //            Quantity = 1,
            //            PRODUCT = db.PRODUCTs.Where(i => i.ID == 3).FirstOrDefault()
            //        }
            //    }
            //    });
            //}
            //End testing Startup


            //Main Startup
            ApplicationView view = new ApplicationView();
            ApplicationViewModel viewModel = new ApplicationViewModel();
            //End main Startup
            /*StatisticWorkSpace.DatabaseController_Statistic db = new StatisticWorkSpace.DatabaseController_Statistic();
            db.createTemplateData();*/
            LoadInitApp();
            view.DataContext = viewModel;
            view.ShowDialog();
        }

        private void LoadInitApp()
        {
            TYPE_PRODUCT.loadListType();
            STATUS_TABLE.loadListStatus();

           /* POSITION_EMPLOYEE.loadListStatus();*/
        }
    }
}
