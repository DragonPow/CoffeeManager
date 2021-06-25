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
            BILL overviewbill = ((ViewModel.HistoryViewModel)DataContext).CurrentBill;
            BillView_readonly view = new BillView_readonly(overviewbill.ID, overviewbill.CheckoutDay, overviewbill.TotalPrice, overviewbill.MoneyCustomer);
            TABLECUSTOM table = new TABLECUSTOM();
            using (mainEntities db = new mainEntities())
            {
                table.table = overviewbill.TABLE;
                var listpro = (from detail in db.DETAILBILLs.Where(i => i.ID_Bill == overviewbill.ID).DefaultIfEmpty()
                               from pro in db.PRODUCTs.Where(p => p.ID == detail.ID_Product).DefaultIfEmpty()
                               select new
                               {
                                   Name = pro.Name,
                                   Quantity = detail.Quantity,
                                   UnitPrice = detail.UnitPrice
                               }).ToList();
                foreach (var i in listpro)
                {
                    table.ListPro.Add(new DetailPro()
                    {
                        Pro = new PRODUCT()
                        {
                            Name = i.Name,
                            Price = i.UnitPrice,
                        },
                        Quantity = (int)i.Quantity
                    });
                }
            }
            view.DataContext = table;
            view.Show();
        }
    }
}
