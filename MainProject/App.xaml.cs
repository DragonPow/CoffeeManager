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
            
            //This is testing Startup
           
            /*TestingView view = new TestingView();
            TestingViewModel viewModel = new TestingViewModel();*/
           
            //End testing Startup
            
            //This is main Startup

            ApplicationView view = new ApplicationView();
            ApplicationViewModel viewModel = new ApplicationViewModel();

            //End main Startup

            //tesing database
            //Để hay xóa dòng này không ảnh hưởng tới luồng chạy chương trình
            using (var main = new mainEntities())
            {
                Console.WriteLine(main.EMPLOYEEs.Count());
            }
            //End testing database

            view.DataContext = viewModel;
            view.ShowDialog();

        }
    }
}
