using MainProject.MainWorkSpace.Product;
using MainProject.Model;
using MainProject.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace MainProject.MainWorkSpace
{
    public class MainViewModel : BaseViewModel, IMainWorkSpace
    {
        #region Feld
        public string NameWorkSpace => "Trang chủ";
        private const PackIconKind _iconDisplay = PackIconKind.Home;

        private ProductViewModel _Productviewmodel;
        private TableViewModel _Tableviewmodel;

        private string _SearchProduct;
        private static ObservableCollection<TYPE_PRODUCT> _ListType;
        private TYPE_PRODUCT _CurrentTypeInHome;

        /*private ObservableCollection<PRODUCT> _Listpro;
       
        private TYPE_PRODUCT _Type_In_Edit_Pro;
        
        private string _NameNewTypeProduct;
        private string _NewNameEditType;

        private TYPE_PRODUCT _CurrentTypeInProManager;
        private string _EditTypeInEditCatefory;


        

        private ICommand _OpenViewEditCategory;
        private ICommand _CloseEditCategory;

        private ICommand _DeleteTypeEditCategory;
    
        private ICommand _ClickCheckboxSelectedPro;

        //Command cho thêm danh mục
        private ICommand _OpenViewAddCategory;
        private ICommand _AddEditCategory;
        private ICommand _CloseViewAddCategory;

        //Command cho chỉnh sửa tên danh mục
        private ICommand _LoadViewEditNameType;
        private ICommand _UpdateNameType;
        private ICommand _CloseViewEditNameType;

        //Command cho chỉnh sửa danh mục
        private ICommand _LoadViewEditProInType;
        private ICommand _SaveEditCategory;
        private ICommand _CloseViewEditProInType;
*/

        #endregion

        #region  propertities
        public string SearchProduct { get => _SearchProduct; set { if (_SearchProduct != value) { _SearchProduct = value; OnPropertyChanged(); Productviewmodel.SearchProduct = value; } } }
        public ProductViewModel Productviewmodel { get => _Productviewmodel; set { if (_Productviewmodel != value) { _Productviewmodel = value; OnPropertyChanged(); } } }
        public TYPE_PRODUCT CurrentTypeInHome { get => _CurrentTypeInHome; set { if (_CurrentTypeInHome != value) { _CurrentTypeInHome = value; OnPropertyChanged(); Productviewmodel.Type = value; } } }
        public TableViewModel Tableviewmodel { get => _Tableviewmodel; set { if (_Tableviewmodel != value) { _Tableviewmodel = value; OnPropertyChanged(); } } }
        /*public TYPE_PRODUCT CurrentTypeInProManager { get => _CurrentTypeInProManager; set { if (_CurrentTypeInProManager != value) { _CurrentTypeInProManager = value; OnPropertyChanged(); if (value != null) { *//*EditTypeInEditCatefory = value.Type; *//* } } } }
        public string EditTypeInEditCatefory { get => _EditTypeInEditCatefory; set { if (_EditTypeInEditCatefory != value) { _EditTypeInEditCatefory = value; OnPropertyChanged(); } } }
        public string NameNewTypeProduct { get => _NameNewTypeProduct; set { if (_NameNewTypeProduct != value) { _NameNewTypeProduct = value; OnPropertyChanged(); } } }
        public string NewNameEditType { get => _NewNameEditType; set { if (_NewNameEditType != value) { _NewNameEditType = value; OnPropertyChanged(); } } }
*/
        public ObservableCollection<TYPE_PRODUCT> ListType
        {
            get => _ListType;
            set
            {
                if (_ListType != value)
                {
                    _ListType = value;
                    OnPropertyChanged();
                }
            }
        }
        public static TYPE_PRODUCT getType(int index)
        {
            return _ListType[index];
        }

        /*  public ObservableCollection<PRODUCT> Listpro
          {
              get => _Listpro;
              set
              {
                  if (_Listpro != value)
                  {
                      _Listpro = value;
                      OnPropertyChanged();

                  }
              }
          }*/

        public PackIcon IconDisplay
        {
            get
            {
                return new PackIcon() { Kind = _iconDisplay, Width = 30, Height = 30 };
            }
        }

/*        public TYPE_PRODUCT Type_In_Edit_Pro { get => _Type_In_Edit_Pro; set { if (_Type_In_Edit_Pro != value) { _Type_In_Edit_Pro = value; OnPropertyChanged(); CurrentTypeInProManager = value; } } }
*/

        #endregion

        #region Init
        public MainViewModel()
        {
           /* Tableviewmodel = new TableViewModel();
            Productviewmodel = new ProductViewModel() { Tableviewmodel = Tableviewmodel };

            Load_Type();

            CurrentTypeInHome = ListType.ElementAt(0);*/

        }

        public MainViewModel(TableViewModel tabVM)
        {
            Tableviewmodel = tabVM;
            Productviewmodel = new ProductViewModel() { Tableviewmodel = tabVM };

            Load_Type();

            CurrentTypeInHome = ListType.ElementAt(0);

        }

        public MainViewModel(TableViewModel tabVM, ProductViewModel proVM)
        {
            Tableviewmodel = tabVM;

            proVM.Tableviewmodel = tabVM;
            Productviewmodel = proVM;

            Load_Type();

            CurrentTypeInHome = ListType.ElementAt(0);

        }

        #endregion

     /*   #region Command*/

/*

        public ICommand AddEditCategory_Command
        {
            get
            {
                if (_AddEditCategory == null)
                {
                    _AddEditCategory = new RelayingCommand<Object>(a => AddEditCategory());
                }
                return _AddEditCategory;
            }
        }


        public void AddEditCategory()
        {
            if (NameNewTypeProduct == null || NameNewTypeProduct =="")
            {
                WindowService.Instance.OpenMessageBox("Vui lòng nhập tên danh mục!", "Lỗi", System.Windows.MessageBoxImage.Error);
                return;
            }
            using (var db = new mainEntities())
            {

                db.TYPE_PRODUCT.Add(new TYPE_PRODUCT() { Type = NameNewTypeProduct });

                db.SaveChanges();

                TYPE_PRODUCT Type = db.TYPE_PRODUCT.OrderByDescending(p => p.ID).FirstOrDefault();

                ListType.Add(Type);

                Productviewmodel.Type = Type;

                CurrentTypeInHome = ListType.Last();

            }
            CloseViewAddCategory();

        }

        public ICommand OpenViewAddCategory_Command
        {
            get
            {
                if (_OpenViewAddCategory == null)
                {
                    _OpenViewAddCategory = new RelayingCommand<Object>(a => OpenViewAddCategory());
                }
                return _OpenViewAddCategory;
            }
        }


        public void OpenViewAddCategory()
        {
            WindowService.Instance.OpenWindowWithoutBorderControl(this, new NewType());

            Productviewmodel.LoadProductByType(CurrentTypeInHome);

        }

        public ICommand CloseViewAddCategory_Command
        {
            get
            {
                if (_CloseViewAddCategory == null)
                {
                    _CloseViewAddCategory = new RelayingCommand<Object>(a => CloseViewAddCategory());
                }
                return _CloseViewAddCategory;
            }
        }


        public void CloseViewAddCategory()
        {
            Window window = WindowService.Instance.FindWindowbyTag("NewType").First();
            window.Close();
        }

        public ICommand DeleteTypeEditCategory_Command
        {
            get
            {
                if (_DeleteTypeEditCategory == null)
                {
                    _DeleteTypeEditCategory = new RelayingCommand<Object>(a => DeleteTypeEditCategory());
                }
                return _DeleteTypeEditCategory;
            }
        }


        public void DeleteTypeEditCategory()
        {
            if (CurrentTypeInProManager == null || CurrentTypeInProManager.ID == 0) return;

            using (var db = new mainEntities())
            {
                var list = db.PRODUCTs.Where(p => (p.ID_Type == CurrentTypeInProManager.ID)).ToList();
                if (list.Count != 0)
                {
                    foreach (var p in list)
                    {
                        p.TYPE_PRODUCT = null;
                    }
                }
                db.TYPE_PRODUCT.Remove(db.TYPE_PRODUCT.Where(t => t.ID == CurrentTypeInProManager.ID).FirstOrDefault());

                db.SaveChanges();

                int number = ListType.IndexOf(CurrentTypeInProManager);
                ListType.RemoveAt(number);

                CurrentTypeInProManager = ListType[0];          
            }


        }
        public ICommand OpenViewEditCategory_Command
        {
            get
            {
                if (_OpenViewEditCategory == null)
                {
                    _OpenViewEditCategory = new RelayingCommand<Object>(a => OpenViewEditCategory());
                }
                return _OpenViewEditCategory;
            }
        }


        public void OpenViewEditCategory()
        {
            ListType[0] = null;

            EditType view = new EditType();
            view.DataContext = this;

            WindowService.Instance.OpenWindowWithoutBorderControl(this, view);
        }

        public ICommand SaveEditCategory_Command
        {
            get
            {
                if (_SaveEditCategory == null)
                {
                    _SaveEditCategory = new RelayingCommand<Object>(a => SaveEditCategory(a));
                }
                return _SaveEditCategory;

            }
        }


        public void SaveEditCategory(object a)
        {

            if (CurrentTypeInProManager == null) return;         

            using (var db = new mainEntities())
            {              
                *//*for (int i = 1; i < ListType.Count; ++i)
                {
                    if (ListType[i].ID == CurrentTypeInProManager.ID)
                    {
                        ListType[i].Type = EditTypeInEditCatefory;
                        break;
                    }
                }*//*

                var type = db.TYPE_PRODUCT.Where(t => t.ID == CurrentTypeInProManager.ID).FirstOrDefault();
                *//*type.Type = EditTypeInEditCatefory;*//*

                var list = db.PRODUCTs.Where(p => (p.ID_Type == type.ID || p.ID_Type == null)).ToList();
                if (list == null) return;

                int j = 0;
                foreach (var p in list)
                {
                    p.ID_Type = Listpro[j].ID_Type;
                    ++j;
                }
             
                db.SaveChanges();
                CloseViewEditProInType();
            }

        }
        public ICommand ClickCheckboxSelectedPro_Command
        {
            get
            {
                if (_ClickCheckboxSelectedPro == null)
                {
                    _ClickCheckboxSelectedPro = new RelayingCommand<PRODUCT>(a => ClickCheckboxSelectedPro(a));
                }
                return _ClickCheckboxSelectedPro;
            }
        }


        public void ClickCheckboxSelectedPro(PRODUCT pro)
        {
            Productviewmodel.Currentproduct = pro;

            if (Productviewmodel.Currentproduct == null) return;
            if (Productviewmodel.Currentproduct.ID_Type == null)
            {
                Productviewmodel.Currentproduct.ID_Type = CurrentTypeInProManager.ID;
            }
            else
            {
                Productviewmodel.Currentproduct.ID_Type = null;
            }
        }
        public ICommand CloseEditCategory_Command
        {
            get
            {
                if (_CloseEditCategory == null)
                {
                    _CloseEditCategory = new RelayingCommand<Object>(a => CloseEditCategory());
                }
                return _CloseEditCategory;
            }
        }


        public void CloseEditCategory()
        {
            Window window = WindowService.Instance.FindWindowbyTag("Edit category").First();
            window.Close();         
           
            if (CurrentTypeInHome == CurrentTypeInProManager) Productviewmodel.LoadProductByType(CurrentTypeInHome);
            ListType[0] = new TYPE_PRODUCT() { Type = "Tất cả", ID = new long() };

            CurrentTypeInProManager = ListType[0];
            EditTypeInEditCatefory = null;
            Listpro = null;
        }

        public ICommand LoadViewEditNameType_Command
        {
            get
            {
                if (_LoadViewEditNameType == null)
                {
                    _LoadViewEditNameType = new RelayingCommand<Object>(a => LoadViewEditNameType());
                }
                return _LoadViewEditNameType;
            }
        }


        public void LoadViewEditNameType()
        {
            WindowService.Instance.OpenWindowWithoutBorderControl(this, new EditNameType());          
        }

        public ICommand UpdateNameType_Command
        {
            get
            {
                if (_UpdateNameType == null)
                {
                    _UpdateNameType = new RelayingCommand<Object>(a => UpdateNameType());
                }
                return _UpdateNameType;
            }
        }


        public void UpdateNameType()
        {
           using ( var db = new  mainEntities())
            {
                var type = db.TYPE_PRODUCT.Where(t => t.ID == CurrentTypeInProManager.ID).FirstOrDefault();
                type.Type = NewNameEditType;
                db.SaveChanges();
            }

            CloseViewEditNameType();
        }

        public ICommand CloseViewEditNameType_Command
        {
            get
            {
                if (_CloseViewEditNameType == null)
                {
                    _CloseViewEditNameType = new RelayingCommand<Object>(a => CloseViewEditNameType());
                }
                return _CloseViewEditNameType;
            }
        }


        public void CloseViewEditNameType()
        {
            Window window = WindowService.Instance.FindWindowbyTag("Edit Name type").First();
            window.Close();
        }

        public ICommand LoadViewEditProInType_Command
        {
            get
            {
                if (_LoadViewEditProInType == null)
                {
                    _LoadViewEditProInType = new RelayingCommand<Object>(a => LoadViewEditProInType());
                }
                return _LoadViewEditProInType;
            }
        }


        public void LoadViewEditProInType()
        {
            LoadProductBYType_EditType();
            WindowService.Instance.OpenWindowWithoutBorderControl(this, new EditProdInType());        
        }

        public ICommand CloseViewEditProInType_Command
        {
            get
            {
                if (_CloseViewEditProInType == null)
                {
                    _CloseViewEditProInType = new RelayingCommand<Object>(a => CloseViewEditProInType());
                }
                return _CloseViewEditProInType;
            }
        }


        public void CloseViewEditProInType()
        {
            Window window = WindowService.Instance.FindWindowbyTag("Edit pro in category").First();
            window.Close();

            if (CurrentTypeInHome == CurrentTypeInProManager) Productviewmodel.LoadProductByType(CurrentTypeInHome);
            ListType[0] = new TYPE_PRODUCT() { Type = "Tất cả", ID = new long() };

            *//*CurrentTypeInProManager = ListType[0];*//*
            Listpro = null;
        }

        #endregion

        private void LoadProductBYType_EditType()
        {
            using (var db = new mainEntities())
            {
                if (CurrentTypeInProManager == null) return;
                if (CurrentTypeInProManager.Type == "Tất cả")
                {
                    var list = db.PRODUCTs.Where( p=> p.IsProvided).ToList();
                    list.ForEach(p => p.IsChecked = true);
                    Listpro = new ObservableCollection<PRODUCT>(list);
                    return;
                }

                var l = db.PRODUCTs.Where(p => p.ID_Type == null || p.ID_Type == CurrentTypeInProManager    .ID && p.IsProvided).ToList();

                if (l == null) return;

                Listpro = new ObservableCollection<PRODUCT>(l);
            }
        }*/

        public void Load_Type()
        {
            using (var db = new mainEntities())
            {
                var l = new List<TYPE_PRODUCT>() { new TYPE_PRODUCT() { Type = "Tất cả", ID = new long() } };

                l.AddRange(db.TYPE_PRODUCT.Distinct().ToList());

                ListType = new ObservableCollection<TYPE_PRODUCT>(l);
            }
        }

    }
}


