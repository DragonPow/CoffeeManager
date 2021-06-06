using MainProject.AccountWorkSpace;
using MainProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MainProject.ViewModel
{
    public enum ModeAccountView
    {
        normal = 1,
        edit = 2,
        add = 3,
    }
    public partial class EmployeeViewModel : BaseViewModel
    {
        #region Field
        ObservableCollection<EMPLOYEE> _ListEmployee;
        long _ID_CurrentEmployee;
        EMPLOYEE _New_Infor_Employee;
        bool _IsPassword;
        bool _IsConfirmPassword;

        ModeAccountView _modeView;
        string _PassWord;
        string _Confirm_Password;

        ICommand _RequestEditEmployeeCommand;
        ICommand _Click_Add_New_Employee;

        ICommand _Cancel;

        ICommand _UpDate_Add_EMployee;

        ICommand _Delete_EMployee;

        ICommand _Load_View_Change_Pass_Employee;
        ICommand _Change_Pass_Employee;


        #endregion

        #region Init

        public EmployeeViewModel()
        {
            using (var db = new mainEntities())
            {
                ListEmployee = new ObservableCollection<EMPLOYEE>(db.EMPLOYEEs.Include("POSITION_EMPLOYEE").Where(e => ((e.DELETED == 0))).ToList());
                ClearCurrentEmployee();
                ModeView = ListEmployee.Count == 0 ? ModeAccountView.add : ModeAccountView.normal;
            }
        }
        #endregion

        #region Property

        public ObservableCollection<EMPLOYEE> ListEmployee { get => _ListEmployee; set { if (value != _ListEmployee) { _ListEmployee = value; OnPropertyChanged(); } } }
        public long ID_CurrentEmployee
        {
            get => _ID_CurrentEmployee;
            set
            {
                if (_ID_CurrentEmployee != value)
                {
                    _ID_CurrentEmployee = value;
                    Console.WriteLine(_ID_CurrentEmployee);
                    OnPropertyChanged();
                }
            }
        }
        public EMPLOYEE New_Infor_Employee
        {
            get => _New_Infor_Employee;
            set
            {
                if (_New_Infor_Employee != value)
                {
                    if (ModeView == ModeAccountView.edit)
                    {
                        Cancel();
                    }
                    _New_Infor_Employee = value;
                    ModeView = ModeAccountView.normal;
                    OnPropertyChanged();
                }
            }
        }
        public string PassWord { get => _PassWord; set { if (_PassWord != value) { _PassWord = value; OnPropertyChanged(); } } }
        public string Confirm_Password
        {
            get => _Confirm_Password;
            set
            {
                if (_Confirm_Password != value)
                {
                    _Confirm_Password = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsPassword
        {
            get => OldPassWord == New_Infor_Employee.Password;
        }
        public bool IsConfirmPassword
        {
            get => PassWord == Confirm_Password;
        }

        public ModeAccountView ModeView
        {
            get => _modeView;
            set
            {
                if (value != _modeView)
                {
                    _modeView = value;
                    OnPropertyChanged();
                }
            }
        }

        public string OldPassWord { get; set; }

        #endregion

        #region Command

        /* public ICommand Check_Account_Command
        {
            get
            {
                if (_Check_Account == null)
                {
                    _Check_Account = new RelayingCommand<LoginWorkSpace.LoginView>(view => Check_Account(view));
                }
                return _Check_Account;
            }
        }


        public void Check_Account(LoginWorkSpace.LoginView view)
        {
            using (var db = new mainEntities())
            {
                EMPLOYEE employee = (EMPLOYEE)db.EMPLOYEEs.Where(e => (e.Name == Login_Employee.Name && e.Password == Login_Employee.Password && e.DELETED == 0));

                if (employee != null)
                {
                    Is_Account = true;
                    view.Close();
                    MainWindow main = new MainWindow();
                    main.Show();
                }
                else
                {
                    Is_Account = false;
                }
            }
        }*/

        public ICommand Click_Add_New_Employee_Command
        {
            get
            {
                if (_Click_Add_New_Employee == null)
                {
                    _Click_Add_New_Employee = new RelayingCommand<Object>(a => Click_Add_New_Employee());
                }
                return _Click_Add_New_Employee;
            }
        }


        private void Click_Add_New_Employee()
        {
            if (ModeView != ModeAccountView.add) New_Infor_Employee = new EMPLOYEE() { POSITION_EMPLOYEE = new POSITION_EMPLOYEE() };

            //ID_CurrentEmployee = New_Infor_Employee.ID;

            //ListEmployee.Add(new EMPLOYEE() { DELETED = 0 });
            //New_Infor_Employee = ListEmployee.Last();

            ModeView = ModeAccountView.add;
            Console.WriteLine("Click btn add employee");
        }

        public ICommand UpDate_Add_EMployee_Command
        {
            get
            {
                if (_UpDate_Add_EMployee == null)
                {
                    _UpDate_Add_EMployee = new RelayingCommand<Object>(a => UpDate_Add_EMployee());
                }
                return _UpDate_Add_EMployee;
            }
        }


        private void UpDate_Add_EMployee()
        {
            using (var db = new mainEntities())
            {
                if (ModeView == ModeAccountView.add)
                {
                    New_Infor_Employee.POSITION_EMPLOYEE = db.POSITION_EMPLOYEE.First(i => i.Position == New_Infor_Employee.POSITION_EMPLOYEE.Position);
                    ListEmployee.Add(New_Infor_Employee);
                }
                else if (ModeView == ModeAccountView.edit)
                {
                    //ID_CurrentEmployee = New_Infor_Employee.ID;

                    EMPLOYEE employee = db.EMPLOYEEs.Where(e => (e.ID == New_Infor_Employee.ID)).FirstOrDefault();
                    employee.DELETED = 1;
                    New_Infor_Employee.POSITION_EMPLOYEE = db.POSITION_EMPLOYEE.First(i => i.Position == New_Infor_Employee.POSITION_EMPLOYEE.Position);
                    //Sai nguyên tắc
                    //ListEmployee[ListEmployee.IndexOf(New_Infor_Employee)] = employee;
                }
                else
                {
                    throw new SettingsPropertyWrongTypeException("Modeview is must have add or edit here");
                }
                db.EMPLOYEEs.Add(New_Infor_Employee);
                db.SaveChanges();
            }

            ModeView = ModeAccountView.normal;
            //ID_CurrentEmployee = new long();
        }

        public ICommand Delete_EMployee_Command
        {
            get
            {
                if (_Delete_EMployee == null)
                {
                    _Delete_EMployee = new RelayingCommand<Object>(a => Delete_EMployee());
                }
                return _Delete_EMployee;
            }
        }


        private void Delete_EMployee()
        {
            using (var db = new mainEntities())
            {
                EMPLOYEE employee = db.EMPLOYEEs.Include("POSITION_EMPLOYEE").Where(e => (e.ID == New_Infor_Employee.ID)).FirstOrDefault();
                employee.DELETED = 1;
                db.SaveChanges();
            }
            ListEmployee.Remove(New_Infor_Employee);
            ModeView = ModeAccountView.normal;
            ClearCurrentEmployee();
        }

        public ICommand Cancel_Command
        {
            get
            {
                if (_Cancel == null)
                {
                    _Cancel = new RelayingCommand<Object>(a => Cancel());
                }
                return _Cancel;
            }
        }


        public void Cancel()
        {
            using (var db = new mainEntities())
            {
                EMPLOYEE employee = db.EMPLOYEEs.Where(e => (e.ID == New_Infor_Employee.ID)).FirstOrDefault();
                New_Infor_Employee.Clone(employee);
                //if (ModeView == ModeAccountView.edit) ListEmployee.RemoveAt(ListEmployee.Count - 1);
            }
        }

        public ICommand Load_ViewChange_Pass_Command
        {
            get
            {
                if (_Load_View_Change_Pass_Employee == null)
                {
                    _Load_View_Change_Pass_Employee = new RelayingCommand<Object>(a => Load_View_Change_Pass_Employee());
                }
                return _Load_View_Change_Pass_Employee;
            }
        }


        public void Load_View_Change_Pass_Employee()
        {
            WindowService.Instance.OpenWindow(this, new ChangedPassword());
        }

        public ICommand Change_Pass_Employee_Command
        {
            get
            {
                if (_Change_Pass_Employee == null)
                {
                    _Change_Pass_Employee = new RelayingCommand<Object>(a => Change_Pass_Employee());
                }
                return _Change_Pass_Employee;
            }
        }

        private void Change_Pass_Employee()
        {
            using (var db = new mainEntities())
            {
                if (!IsPassword)
                {
                    WindowService.Instance.OpenMessageBox("Nhập sai mật khẩu", "Lỗi tài khoản", System.Windows.MessageBoxImage.Error);
                    return;
                }
                else if(!IsConfirmPassword)
                {
                    WindowService.Instance.OpenMessageBox("Mật khẩu xác nhận phải giống với mật khẩu mới", "Lỗi mật khẩu", System.Windows.MessageBoxImage.Error);
                    return;
                }
                else
                {
                    var e = db.EMPLOYEEs.Where(employee => (New_Infor_Employee.ID == employee.ID && New_Infor_Employee.DELETED == 0)).FirstOrDefault();
                    e.Password = PassWord;
                    db.SaveChanges();
                    New_Infor_Employee.Password = PassWord;

                    OldPassWord = PassWord = Confirm_Password = null;

                    //Request close window
                    Window window = WindowService.Instance.FindWindowbyTag("ChangePassword").First();
                    window.Close();
                }
            }
        }

        public ICommand RequestEditEmployeeCommand
        {
            get
            {
                if (_RequestEditEmployeeCommand == null)
                {
                    _RequestEditEmployeeCommand = new RelayingCommand<Object>(a => RequestEditEmployee());
                }
                return _RequestEditEmployeeCommand;
            }
        }

        private void RequestEditEmployee()
        {
            ModeView = ModeAccountView.edit;
        }
        #endregion

        private void ClearCurrentEmployee()
        {
            New_Infor_Employee = ListEmployee.Count == 0 ? CreateNewEmployee() : ListEmployee[0];
        }
        private EMPLOYEE CreateNewEmployee()
        {
            return new EMPLOYEE() { POSITION_EMPLOYEE = new POSITION_EMPLOYEE() };
        }
    }
}
