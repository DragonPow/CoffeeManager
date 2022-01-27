using MainProject.MainWorkSpace;
using MainProject.MainWorkSpace.Product;
using MainProject.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MainProject.ViewModel
{
    public class ManageProductviewModel : BaseViewModel, IMainWorkSpace
    {
        public string NameWorkSpace => "Quản lý thực đơn";
        private const PackIconKind _iconDisplay = PackIconKind.FoodForkDrink;
        public PackIcon IconDisplay
        {
            get
            {
                return new PackIcon() { Kind = _iconDisplay, Width = 30, Height = 30 };
            }
        }

        #region Feld

        private MainViewModel _MainVM;
        private ObservableCollection<PRODUCT> _Listpro;
        private string _NameNewTypeProduct;
        private string _NewNameEditType;

        private TYPE_PRODUCT _CurrentTypeInProManager;
        private string _EditTypeInEditCatefory;

        public mainEntities db = new mainEntities();



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


        #endregion

        #region Properties
        public MainViewModel MainVM { get => _MainVM; set { if (_MainVM  != value) { _MainVM = value; OnPropertyChanged(); } } }
        public TYPE_PRODUCT CurrentTypeInProManager { get => _CurrentTypeInProManager; set { if (_CurrentTypeInProManager != value) { _CurrentTypeInProManager = value; OnPropertyChanged(); if (value != null) { /*EditTypeInEditCatefory = value.Type; */ } } } }
        public string EditTypeInEditCatefory { get => _EditTypeInEditCatefory; set { if (_EditTypeInEditCatefory != value) { _EditTypeInEditCatefory = value; OnPropertyChanged(); } } }
        public string NameNewTypeProduct { get => _NameNewTypeProduct; set { if (_NameNewTypeProduct != value) { _NameNewTypeProduct = value; OnPropertyChanged(); } } }
        public string NewNameEditType { get => _NewNameEditType; set { if (_NewNameEditType != value) { _NewNameEditType = value; OnPropertyChanged(); } } }

        public ObservableCollection<PRODUCT> Listpro
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
        }
        #endregion



        public ManageProductviewModel(MainViewModel mainvm)
        {
            MainVM = mainvm;
        }
        public ManageProductviewModel()
        {
        }

        #region ICommand
        public ICommand AddEditCategory_Command
        {
            get
            {
                if (_AddEditCategory == null)
                {
                    _AddEditCategory = new RelayingCommand<object>(a =>
                    { 
                        try
                        {
                            AddEditCategory();
                            CloseViewAddCategory();
                        }
                        catch (ArgumentException e)
                        {
                            switch (e.ParamName)
                            {
                                case "NameEmpty":
                                    WindowService.Instance.OpenMessageBox("Vui lòng nhập tên danh mục!", "Lỗi", MessageBoxImage.Error);
                                    break;
                                case "NameExisting":
                                    WindowService.Instance.OpenMessageBox("Danh mục đã tồn tại, vui lòng đặt tên khác!", "Thông báo", MessageBoxImage.Information);
                                    break;
                                default:
                                    WindowService.Instance.OpenMessageBox("Lỗi cập nhật", "Lỗi", MessageBoxImage.Error);
                                    break;
                            }
                           
                        }                     
                    });
                }
                return _AddEditCategory;
            }
        }


        public void AddEditCategory()
        {
            if (NameNewTypeProduct == null || NameNewTypeProduct == "")
            {
                throw new ArgumentException("Name category is empty", "NameEmpty");             
            }

            /*using (var db = new mainEntities())*/
            {

                TYPE_PRODUCT type = db.TYPE_PRODUCT.Where(t => t.Type == NameNewTypeProduct).FirstOrDefault();

                if (type != null)
                {
                    throw new ArgumentException("Category is existing", "NameExisting");
                }
                db.TYPE_PRODUCT.Add(new TYPE_PRODUCT() { Type = NameNewTypeProduct });

                var l = db.TYPE_PRODUCT.ToList();

                db.SaveChanges();

                TYPE_PRODUCT Type = db.TYPE_PRODUCT.OrderByDescending(p => p.ID).FirstOrDefault();

                MainVM.ListType.Add(Type);

                MainVM.Productviewmodel.Type = Type;

                MainVM.CurrentTypeInHome = MainVM.ListType.Last();

            }
           

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

            MainVM.Productviewmodel.LoadProductByType(MainVM.CurrentTypeInHome);

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

          /*  using (var db = new mainEntities())*/
            {
                var list = db.PRODUCTs.Where(p => (p.ID_Type == CurrentTypeInProManager.ID) && p.IsProvided ).ToList();
                if (list.Count != 0)
                {
                    foreach (var p in list)
                    {
                        p.TYPE_PRODUCT = null;
                    }
                }
                db.TYPE_PRODUCT.Remove(db.TYPE_PRODUCT.Where(t => t.ID == CurrentTypeInProManager.ID).FirstOrDefault());

                db.SaveChanges();

                int number = MainVM.ListType.IndexOf(CurrentTypeInProManager);
                MainVM.ListType.RemoveAt(number);

                CurrentTypeInProManager = MainVM.ListType[0];
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
            MainVM.ListType.RemoveAt(0);

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

          /*  using (var db = new mainEntities())*/
            {
                /*for (int i = 1; i < ListType.Count; ++i)
                {
                    if (ListType[i].ID == CurrentTypeInProManager.ID)
                    {
                        ListType[i].Type = EditTypeInEditCatefory;
                        break;
                    }
                }*/

                var type = db.TYPE_PRODUCT.Where(t => t.ID == CurrentTypeInProManager.ID).FirstOrDefault();
                /*type.Type = EditTypeInEditCatefory;*/

                var list = db.PRODUCTs.Where(p => (p.ID_Type == type.ID || p.ID_Type == null) && p.IsProvided).ToList();
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
            MainVM.Productviewmodel.Currentproduct = pro;

            if (MainVM.Productviewmodel.Currentproduct == null) return;
            if (MainVM.Productviewmodel.Currentproduct.ID_Type == null)
            {
                MainVM.Productviewmodel.Currentproduct.ID_Type = CurrentTypeInProManager.ID;
            }
            else
            {
                MainVM.Productviewmodel.Currentproduct.ID_Type = null;
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

            if (MainVM.CurrentTypeInHome == CurrentTypeInProManager) MainVM.Productviewmodel.LoadProductByType(MainVM.CurrentTypeInHome);
            MainVM.ListType.Insert(0, new TYPE_PRODUCT() { Type = "Tất cả", ID = new long() });

            CurrentTypeInProManager = MainVM.ListType[0];
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
            if (NewNameEditType == null || NewNameEditType =="")
            {
                WindowService.Instance.OpenMessageBox("Không được để tên danh mục trống!","Lỗi", MessageBoxImage.Error);
                return;
            }

          /*  using (var db = new mainEntities())*/
            {
                TYPE_PRODUCT T = db.TYPE_PRODUCT.Where(t => t.Type == NewNameEditType).FirstOrDefault();

                if (T != null)
                {
                    WindowService.Instance.OpenMessageBox("Danh mục đã tồn tại, vui lòng đặt tên khác!", "Thông báo", System.Windows.MessageBoxImage.Information);
                    return;
                }
                var type = db.TYPE_PRODUCT.Where(t => t.ID == CurrentTypeInProManager.ID).FirstOrDefault();
                type.Type = NewNameEditType;

                for ( int i = 1; i < MainVM.ListType.Count; ++i)
                {
                    if (MainVM.ListType[i].ID == CurrentTypeInProManager.ID)
                    {
                        MainVM.ListType[i].Type = NewNameEditType;
                        break;
                    }    
                }    
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

            if (MainVM.CurrentTypeInHome == CurrentTypeInProManager) MainVM.Productviewmodel.LoadProductByType(MainVM.CurrentTypeInHome);
         /*   MainVM.ListType[0] = new TYPE_PRODUCT() { Type = "Tất cả", ID = new long() };*/

            /*CurrentTypeInProManager = ListType[0];*/
            Listpro = null;
        }
        #endregion

        private void LoadProductBYType_EditType()
        {
           /* using (var db = new mainEntities())*/
            {
                if (CurrentTypeInProManager == null) return;
                if (CurrentTypeInProManager.Type == "Tất cả")
                {
                    var list = db.PRODUCTs.Where(p => p.IsProvided).ToList();
                    list.ForEach(p => p.IsChecked = true);
                    Listpro = new ObservableCollection<PRODUCT>(list);
                    return;
                }

                var l = db.PRODUCTs.Where(p => (p.ID_Type == null || p.ID_Type == CurrentTypeInProManager.ID) && p.IsProvided).ToList();

                if (l == null) return;

                Listpro = new ObservableCollection<PRODUCT>(l);
            }
        }

    }
}
