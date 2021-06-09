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
    class MainViewModel : BaseViewModel, IMainWorkSpace
    {
        #region Feld
        public string NameWorkSpace => "Home";

        private ProductViewModel _Productviewmodel;
        private TableViewModel _Tableviewmodel;
        private ObservableCollection<TYPE_PRODUCT> _ListType;
        private TYPE_PRODUCT _CurrentType;
        private TYPE_PRODUCT _Type_In_Edit_CATEGORY;
        private string _SearchProduct;
        private string _NameNewTypeProduct;

        private TYPE_PRODUCT _TypeInEditCATEGORYCombobox;
        private string _EditTypeInEditCatefory;


        private const PackIconKind _iconDisplay = PackIconKind.Home;

        private ICommand _OpenViewEditCategory;
        private ICommand _AddEditCategory;
        private ICommand _SaveEditCategory;
        private ICommand _DeleteTypeEditCategory;
        private ICommand _CloseEditCategory;
        private ICommand _ClickCheckboxSelectedPro;
        private ICommand _OpenViewAddCategory;
        private ICommand _CloseViewAddCategory;



        #endregion

        #region  propertities
        public string SearchProduct { get => _SearchProduct; set { if (_SearchProduct != value) { _SearchProduct = value; OnPropertyChanged(); Productviewmodel.SearchProduct = value; /*CurrentType = ListType.ElementAt(0);*/ } } }
        public ProductViewModel Productviewmodel { get => _Productviewmodel; set { if (_Productviewmodel != value) { _Productviewmodel = value; OnPropertyChanged(); } } }
        public TYPE_PRODUCT CurrentType { get => _CurrentType; set { if (_CurrentType != value) { _CurrentType = value; OnPropertyChanged(); Productviewmodel.Type = value; } } }
        public TableViewModel Tableviewmodel { get => _Tableviewmodel; set { if (_Tableviewmodel != value) { _Tableviewmodel = value; OnPropertyChanged(); } } }
        public TYPE_PRODUCT TypeInEditCATEGORYCombobox { get => _TypeInEditCATEGORYCombobox; set { if (_TypeInEditCATEGORYCombobox != value) { _TypeInEditCATEGORYCombobox = value; OnPropertyChanged(); if (value != null) { EditTypeInEditCatefory = value.Type; LoadProductBYType_EditType(); } } } }
        public string EditTypeInEditCatefory { get => _EditTypeInEditCatefory; set { if (_EditTypeInEditCatefory != value) { _EditTypeInEditCatefory = value; OnPropertyChanged(); } } }
        public string NameNewTypeProduct { get => _NameNewTypeProduct; set { if (_NameNewTypeProduct != value) { _NameNewTypeProduct = value; OnPropertyChanged(); } } }
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

        public PackIcon IconDisplay
        {
            get
            {
                return new PackIcon() { Kind = _iconDisplay, Width = 30, Height = 30 };
            }
        }

        public TYPE_PRODUCT Type_In_Edit_CATEGORY { get => _Type_In_Edit_CATEGORY; set { if (_Type_In_Edit_CATEGORY != value) { _Type_In_Edit_CATEGORY = value; OnPropertyChanged(); TypeInEditCATEGORYCombobox = value; } } }


        #endregion

        #region Init
        public MainViewModel()
        {

            Tableviewmodel = new TableViewModel();
            Productviewmodel = new ProductViewModel() { Tableviewmodel = Tableviewmodel };

            Load_Type();

            CurrentType = ListType.ElementAt(0);

        }

        public MainViewModel(TableViewModel tabVM)
        {

            Productviewmodel = new ProductViewModel() { Tableviewmodel = tabVM };

            Load_Type();

            CurrentType = ListType.ElementAt(0);

        }

        #endregion

        #region Command



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

                CurrentType = ListType.Last();

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

            Productviewmodel.LoadProductByType(CurrentType);

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
            if (TypeInEditCATEGORYCombobox == null || TypeInEditCATEGORYCombobox.ID == 0) return;

            using (var db = new mainEntities())
            {
                var list = db.PRODUCTs.Where(p => (p.ID_Type == TypeInEditCATEGORYCombobox.ID)).ToList();
                if (list.Count != 0)
                {
                    foreach (var p in list)
                    {
                        p.TYPE_PRODUCT = null;
                    }
                }

                db.TYPE_PRODUCT.Remove(db.TYPE_PRODUCT.Where(t => t.ID == TypeInEditCATEGORYCombobox.ID).FirstOrDefault());

                db.SaveChanges();

                int number = ListType.IndexOf(TypeInEditCATEGORYCombobox);

                TypeInEditCATEGORYCombobox = ListType.ElementAt(0);
                ListType.RemoveAt(number);

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

            WindowService.Instance.OpenWindowWithoutBorderControl(this, new EditType());

            Productviewmodel.LoadProductByType(CurrentType);

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

            if (TypeInEditCATEGORYCombobox == null) return;

            if (EditTypeInEditCatefory == "")
            {
                WindowService.Instance.OpenMessageBox("Vui lòng nhập tên danh mục!", "Lỗi", System.Windows.MessageBoxImage.Error);
                return;
            }

            using (var db = new mainEntities())
            {
                for ( int i = 1; i < ListType.Count; ++i)
                {
                    if (ListType[i].ID == TypeInEditCATEGORYCombobox.ID)
                    {
                        ListType[i].Type = EditTypeInEditCatefory;
                        break; 
                    }    
                }

                var type = db.TYPE_PRODUCT.Where(t => t.ID == TypeInEditCATEGORYCombobox.ID).FirstOrDefault();
                type.Type = EditTypeInEditCatefory;

                var list = db.PRODUCTs.Where(p => (p.ID_Type == type.ID || p.ID_Type == null)).ToList();
                if (list == null) return;

                int j = 0;
                foreach (var p in list)
                {
                    p.ID_Type = Productviewmodel.ListPoduct[j].ID_Type;
                    ++j;
                }

                TypeInEditCATEGORYCombobox = ListType[0];

                db.SaveChanges();
                CloseEditCategory();
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
                Productviewmodel.Currentproduct.ID_Type = TypeInEditCATEGORYCombobox.ID;
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
           
            Productviewmodel.LoadProductByType(CurrentType);
            ListType[0] = new TYPE_PRODUCT() { Type = "Tất cả", ID = new long() };
            TypeInEditCATEGORYCombobox = ListType[0];
            EditTypeInEditCatefory = null;
        }


        #endregion

        private void LoadProductBYType_EditType()
        {
            using (var db = new mainEntities())
            {
                if (TypeInEditCATEGORYCombobox == null) return;
                if (TypeInEditCATEGORYCombobox.Type == "Tất cả")
                {
                    var list = db.PRODUCTs.ToList();
                    list.ForEach(p => p.IsChecked = true);
                    Productviewmodel.ListPoduct = new ObservableCollection<PRODUCT>(list);
                    return;
                }

                var l = db.PRODUCTs.Where(p => p.ID_Type == null || p.ID_Type == TypeInEditCATEGORYCombobox.ID).ToList();

                if (l == null) return;

                Productviewmodel.ListPoduct = new ObservableCollection<PRODUCT>(l);
            }
        }

        void Load_Type()
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

