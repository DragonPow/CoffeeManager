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

            ////Testing Startup
            BillView view = new BillView();
            BillViewModel viewModel = new BillViewModel();
            ////End testing Startup
            ///

            //Main Startup
            //ApplicationView view = new ApplicationView();
            //ApplicationViewModel viewModel = new ApplicationViewModel();
            //End main Startup

            //LoadInitApp();
            view.DataContext = viewModel;
            view.Show();
            //view.ShowDialog();
        }

        private void LoadInitApp()
        {
            TYPE_PRODUCT.loadListType();
            STATUS_TABLE.loadListStatus();
            POSITION_EMPLOYEE.loadListStatus();
        }
    }
}
