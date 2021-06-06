using MainProject.ViewModel;
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
//using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainProject.AccountWorkSpace
{
    /// <summary>
    /// Interaction logic for AccountWorkSpace.xaml
    /// </summary>
    public partial class AccountView : UserControl
    {
        public AccountView()
        {
            InitializeComponent();
            Console.WriteLine("AccountView is created");
            DataContextChanged += AccountView_DataContextChanged;
            txt_pass.DataContextChanged += Txt_pass_DataContextChanged;
        }


        private void Txt_pass_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null)
            {
                if (DataContext is EmployeeViewModel)
                {
                    PasswordChange();
                }
            }
        }
        private void AccountView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null)
            {
                if (DataContext is EmployeeViewModel)
                {
                    var data = (EmployeeViewModel)DataContext;
                    data.New_Infor_Employee.PropertyChanged += New_Infor_Employee_PropertyChanged;
                }
            }
        }


        private void New_Infor_Employee_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Password")
            {
                PasswordChange();
            }
        }
        public void PasswordChange()
        {
            txt_pass.Password = ((EmployeeViewModel)DataContext).New_Infor_Employee.Password;
        }
    }
}
