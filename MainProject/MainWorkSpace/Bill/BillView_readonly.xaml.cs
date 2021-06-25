using MainProject.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MainProject.MainWorkSpace.Bill
{
    /// <summary>
    /// Interaction logic for BillView.xaml
    /// </summary>
    public partial class BillView_readonly : Window, INotifyPropertyChanged
    {
        public long TotalPrice { get; set; }
        public long MoneyCustomer { get; set; }
        public long Refund { get => MoneyCustomer - TotalPrice; }
        public DateTime TimeCheckout { get; set; }
        public long BillID { get; set; }
        public BillView_readonly()
        {
            InitializeComponent();
        }
        public BillView_readonly(long billID, DateTime timeCheckout, long totalprice, long moneyCustomer)
        {
            InitializeComponent();
            this.BillID = billID;
            this.TimeCheckout = timeCheckout;
            this.TotalPrice = totalprice;
            this.MoneyCustomer = moneyCustomer;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
