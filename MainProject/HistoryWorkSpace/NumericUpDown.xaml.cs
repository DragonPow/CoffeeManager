using System;
using System.Collections.Generic;
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
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public NumericUpDown()
        {
            InitializeComponent();
            txtNumberPage.KeyDown += TxtNumberPage_KeyDown;
            txtNumberPage.TextChanged += TxtNumberPage_TextChanged;
        }

        private void TxtNumberPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BindingExpression binding = txtNumberPage.GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();
            }
            e.Handled = (e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9);
        }

        private void TxtNumberPage_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}
