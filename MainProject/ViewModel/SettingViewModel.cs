using MainProject.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MainProject.ViewModel
{
    public class SettingViewModel : BaseViewModel, IMainWorkSpace
    {
        public enum ModeButton
        {
            edit = 1,
            save = 2,
        }

        public string NameWorkSpace => "Thông tin";
        private const PackIconKind _iconDisplay = PackIconKind.AccountOutline;
        public  mainEntities context = new mainEntities();
        public PackIcon IconDisplay
        {
            get
            {
                return new PackIcon() { Kind = _iconDisplay, Width = 30, Height = 30 };
            }
        }

        #region init 
        public SettingViewModel()
        {
            SetData();
        }

        public SettingViewModel(mainEntities dtcontext)
        {
            context = dtcontext;
            SetData();
        }
        public void SetData()
        {
            /*using (var context = new mainEntities())*/
            {
                var st = context.PARAMETERs.Where(p => p.NAME == "StoreName").FirstOrDefault();
                NameStore = st.Value.ToString();
                st = context.PARAMETERs.Where(p => p.NAME == "StorePhone").FirstOrDefault();
                NumberPhone = st.Value.ToString();
                st = context.PARAMETERs.Where(p => p.NAME == "StoreAddress").FirstOrDefault();
                Address = st.Value.ToString();
            }
        }



        #endregion

        #region fields

        private string _nameStore;
        private string _numberPhone;
        private string _address;

        ModeButton _mode_btn;
        ICommand _Change_data_store;
        ICommand _Save_Data_Store;
        ICommand _Cancel_Change_Data_Store;
        #endregion

        #region propertities
        public string NameStore
        {
            get => _nameStore;
            set
            {
                if (_nameStore != value)
                {
                    _nameStore = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NumberPhone
        {
            get => _numberPhone;
            set
            {
                if (_numberPhone != value)
                {
                    _numberPhone = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged();
                }
            }
        }

        public ModeButton Mode_btn
        {
            get => _mode_btn;
            set
            {
                if (value != _mode_btn)
                {
                    _mode_btn = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Commnand

        public ICommand Save_Data_Store
        {
            get
            {
                if (_Save_Data_Store == null)
                {
                    _Save_Data_Store = new RelayingCommand<object>(a =>
                       {
                           try
                           {
                               Save_data_store();
                           }
                           catch(InvalidOperationException e)
                           {
                               WindowService.Instance.OpenMessageBox("Vui lòng nhập đầy đủ thông tin!", "Lỗi", System.Windows.MessageBoxImage.Error);
                               return;
                           }

                       });
                   
                }
                return _Save_Data_Store;

            }
        }

        public void Save_data_store()
        {
            Mode_btn = ModeButton.save;
            if (NameStore == "" || NumberPhone == "" || Address == "")
            {
                throw new InvalidOperationException("Empty data!");
            }          
            /* using (var context = new mainEntities())*/
            {
                var st = context.PARAMETERs.Where(p => p.NAME == "StoreName").FirstOrDefault();
                st.Value = NameStore;
                st = context.PARAMETERs.Where(p => p.NAME == "StorePhone").FirstOrDefault();
                st.Value = NumberPhone;
                st = context.PARAMETERs.Where(p => p.NAME == "StoreAddress").FirstOrDefault();
                st.Value = Address;
                context.SaveChanges();
            }
        }

        public ICommand Change_Data_Store
        {
            get
            {
                if (_Change_data_store == null)
                {
                    _Change_data_store = new RelayingCommand<object>(a => Change_data_store());
                }
                return _Change_data_store;
            }
        }

        private void Change_data_store()
        {
            Mode_btn = ModeButton.edit;
        }

        public ICommand Cancel_Change_Data_Store
        {
            get
            {
                if (_Cancel_Change_Data_Store == null)
                {
                    _Cancel_Change_Data_Store = new RelayingCommand<object>(a => {
                        SetData();
                        Mode_btn = ModeButton.save;
                    });
                }
                return _Cancel_Change_Data_Store;
            }
        }
        #endregion
    }
}
