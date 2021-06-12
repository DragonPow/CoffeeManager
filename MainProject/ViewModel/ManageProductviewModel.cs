using MainProject.MainWorkSpace;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.ViewModel
{
    class ManageProductviewModel : BaseViewModel, IMainWorkSpace
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

        private MainViewModel _MainVM;

        public MainViewModel MainVM { get => _MainVM; set { if (_MainVM  != value) { _MainVM = value; OnPropertyChanged(); } } }


        public ManageProductviewModel(MainViewModel mainvm)
        {
            MainVM = mainvm;
        }
        public ManageProductviewModel()
        {
        }
    }
}
