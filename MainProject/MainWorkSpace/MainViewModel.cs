using MainProject.Model;
using MainProject.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private const PackIconKind _iconDisplay = PackIconKind.Home;

       
        #endregion

        #region  propertities

        public ProductViewModel Productviewmodel { get => _Productviewmodel; set { if (_Productviewmodel != value) { _Productviewmodel = value; OnPropertyChanged(); } } }
        public TYPE_PRODUCT CurrentType { get => _CurrentType; set { if (_CurrentType != value) { _CurrentType = value; OnPropertyChanged();  Productviewmodel.Type = value; } } }
        public TableViewModel Tableviewmodel { get => _Tableviewmodel; set { if (_Tableviewmodel != value) { _Tableviewmodel = value; OnPropertyChanged(); } } }
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
                return new PackIcon() { Kind = _iconDisplay, Width=30, Height=30 };
            }
        }

        public TYPE_PRODUCT Type_In_Edit_CATEGORY { get => _Type_In_Edit_CATEGORY; set { if (_Type_In_Edit_CATEGORY != value) { _Type_In_Edit_CATEGORY = value; OnPropertyChanged(); Productviewmodel.TypeInEditCATEGORYCombobox = value ; } } }


        #endregion

        #region Init
        public MainViewModel()
        {

            Tableviewmodel = new TableViewModel();
            Productviewmodel = new ProductViewModel() { Tableviewmodel = Tableviewmodel };

            using (var db = new mainEntities())
            {
                var l = new List<TYPE_PRODUCT>();
             
                l.AddRange(db.TYPE_PRODUCT.Distinct().ToList());

                ListType = new ObservableCollection<TYPE_PRODUCT>(l);
            }
        }
        #endregion

        #region Command

     
        #endregion
    }
}
