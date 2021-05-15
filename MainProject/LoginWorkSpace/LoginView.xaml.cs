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
using System.Windows.Shapes;

namespace MainProject.LoginWorkSpace
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            this.Closing += LoginView_Closing; ;
        }

        private void LoginView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (((LoginViewModel)this.DataContext).isLoginSuccess)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void TextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((LoginViewModel)this.DataContext).CurrentAccount.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
