using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MainProject.ViewModel
{
    public class LoginFormViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; set; }
        public string UserName { get => _userName; set { _userName = value; NotifyPropertyChanged(); } }
        public string PassWord { get => _passWord; set { _passWord = value; NotifyPropertyChanged(); } }

    private string _userName;
        private string _passWord;
        public LoginFormViewModel()
        {
            UserName = "admin";
            PassWord = "admin";
            LoginCommand = new RelayingCommand<MainWindow>(form => { Login(form); });
        }

        private void Login(MainWindow form)
        {
            
        }
    }
}
