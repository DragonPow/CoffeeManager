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
        public BillView_readonly()
        {
            InitializeComponent();
        }

        public long Refund
        {
            get
            {
                if (DataContext != null)
                {
                    Model.BILL b = (Model.BILL)DataContext;
                    return b.MoneyCustomer - b.TotalPrice;
                }
                return 0;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
