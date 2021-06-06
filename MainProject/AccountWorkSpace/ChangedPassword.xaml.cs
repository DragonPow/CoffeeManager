using MainProject.ViewModel;
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

namespace MainProject.AccountWorkSpace
{
    /// <summary>
    /// Interaction logic for ChangedPassword.xaml
    /// </summary>
    public partial class ChangedPassword : UserControl
    {
        public ChangedPassword()
        {
            InitializeComponent();
            txtPass.PasswordChanged += TxtPass_PasswordChanged;
            txtNewPass.PasswordChanged += TxtNewPass_PasswordChanged;
            txtRepass.PasswordChanged += TxtRepass_PasswordChanged;
        }

        private void TxtRepass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext!=null)
            {
                ((EmployeeViewModel)this.DataContext).Confirm_Password = txtRepass.Password;
            }
        }

        private void TxtNewPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((EmployeeViewModel)this.DataContext).PassWord = txtNewPass.Password;
            }
        }

        private void TxtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((EmployeeViewModel)this.DataContext).OldPassWord = txtPass.Password;
                Console.WriteLine(((EmployeeViewModel)this.DataContext).IsPassword);
            }
        }
    }
}
