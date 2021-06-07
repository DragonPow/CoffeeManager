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
    public partial class BillView : Window, INotifyPropertyChanged
    {
        public BillView()
        {
            InitializeComponent();
            txtMoneyCustomer.PreviewTextInput += TxtMoneyCustomer_PreviewTextInput;
        }

        private void TxtMoneyCustomer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Any(c => !char.IsDigit(c))) e.Handled = true;
            else e.Handled = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int ErrorCount { get; set; }
        public bool IsValid { get => ErrorCount < 1; }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                ErrorCount++;
            }
            else
            {
                ErrorCount--;
            }
        }
    }
}
