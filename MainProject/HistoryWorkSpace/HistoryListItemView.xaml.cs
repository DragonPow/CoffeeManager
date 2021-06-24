using MainProject.MainWorkSpace.Bill;
using MainProject.Model;
using MainProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainProject.HistoryWorkSpace
{
    /// <summary>
    /// Interaction logic for HistoryListItemView.xaml
    /// </summary>
    public partial class HistoryListItemView : UserControl
    {
        public HistoryListItemView()
        {
            InitializeComponent();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //BillView_readonly view = new BillView_readonly();
            //BILL overviewbill = ((ViewModel.HistoryViewModel)DataContext).CurrentBill;
            //BILL bill;
            //using (mainEntities db = new mainEntities())
            //{
            //    bill = db.BILLs.Where(b => b.ID == overviewbill.ID).Include("DETAILBILLs").First();
            //    foreach (DETAILBILL detail in bill.DETAILBILLs)
            //    {
            //        var pro = (from p in db.PRODUCTs.Where(i => i.ID == detail.ID_Product).DefaultIfEmpty()
            //                  select new
            //                  {
            //                      Name = p.Name
            //                  }).FirstOrDefault();
            //        detail.PRODUCT = new PRODUCT();
            //        detail.PRODUCT.Name = pro.Name;
            //    }
            //}
            //view.DataContext = bill;
            //view.Show();
        }
    }
}
