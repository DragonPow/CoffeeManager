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

namespace MainProject.SettingWorkSpace
{
    /// <summary>
    /// Interaction logic for SettingView.xaml
    /// </summary>
    public partial class SettingView : UserControl
    {
        
        public SettingView()
        {
            InitializeComponent();
            txb_numberPhone.PreviewTextInput += Txb_numberPhone_PreviewTextInput;
        }

        private void Txb_numberPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Any(c => !char.IsDigit(c))) e.Handled = true;
            else e.Handled = false;
        }
    }
}
