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
        }

        #region fields

        private String _nameStore;
        private String _numberPhone;
        private String _address;
        private String _numberFloor;

        #endregion

        #region propertities
        public String NameStore
        {
            get => _nameStore;
            set
            {
                _nameStore = value;
            }
        }

        public String NumberPhone
        {
            get => _numberPhone;
            set
            {
                _numberPhone = value;
            }
        }

        public String Adress
        {
            get => _address;
            set
            {
                _address = value;
            }
        }

        public String NumberFloor
        {
            get => _numberFloor;
            set
            {
                _numberFloor = value;
            }
        }

        #endregion

        private void btn_SaveName_Click(object sender, RoutedEventArgs e)
        {
            txb_Name.IsEnabled = true;
            txb_Name.Focusable = true;
        }

        private void btn_SavePhone_Click(object sender, RoutedEventArgs e)
        {
            txb_numberPhone.IsEnabled = true;
            txb_numberPhone.Focusable = true;
        }

        private void btn_SaveAddress_Click(object sender, RoutedEventArgs e)
        {
            txb_address.IsEnabled = true;
            txb_address.Focusable = true;
        }

        private void btn_NumberFloor_Click(object sender, RoutedEventArgs e)
        {
            txb_NumberFloor.IsEnabled = true;
            txb_NumberFloor.Focusable = true;
        }

        private void txb_Name_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _nameStore = txb_Name.Text;
            btn_Save_change.IsEnabled = true;
        }

        private void txb_NumberFloor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _numberFloor = txb_NumberFloor.Text;
            btn_Save_change.IsEnabled = true;
        }

        private void txb_numberPhone_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _numberPhone = txb_numberPhone.Text;
            btn_Save_change.IsEnabled = true;
        }

        private void txb_address_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _address = txb_address.Text;
            btn_Save_change.IsEnabled = true;
        }

        private void btn_Save_change_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Overview_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
