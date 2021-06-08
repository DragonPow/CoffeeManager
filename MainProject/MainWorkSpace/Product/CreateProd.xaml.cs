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

namespace MainProject.MainWorkSpace.Product
{
    /// <summary>
    /// Interaction logic for CreateProd.xaml
    /// </summary>
    public partial class CreateProd : UserControl, INotifyPropertyChanged
    {
        public CreateProd()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int ErrorCount{ get; set; }
        public bool IsValid { get => ErrorCount < 1; }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action==ValidationErrorEventAction.Added)
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
