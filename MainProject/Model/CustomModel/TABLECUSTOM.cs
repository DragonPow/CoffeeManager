using MainProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Model
{
    public class TABLECUSTOM : BaseViewModel
    {
        private ObservableCollection<DetailPro> _listPro;
        private TABLE _table;
        public TABLE table
        {
            get => _table; 
            set
            {
                _table = value;
                if (table != null)
                {
                    table.PropertyChanged += Table_PropertyChanged;
                }
            }
        }
        public long Total
        {
            get
            {
                if (_listPro == null) return 0;
                return _listPro.Sum(i => (long)i.Pro.Price * i.Quantity);
            } 
        }

        public virtual ObservableCollection<DetailPro> ListPro
        {
            get
            {
                if (_listPro == null)
                {
                    _listPro = new ObservableCollection<DetailPro>();
                }
                return _listPro;
            }
            set
            {
                if (_listPro != value)
                {
                    _listPro = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsOnService { get => table.CurrentStatus.Equals("Already"); }
        public System.Windows.Visibility IsOnRepair
        {
            get
            {
                if (table.CurrentStatus.Equals("Fix")) return System.Windows.Visibility.Visible;
                else return System.Windows.Visibility.Collapsed;
            }
        }
        public System.Windows.Visibility IsNotOnRepair
        {
            get
            {
                if (!table.CurrentStatus.Equals("Fix")) return System.Windows.Visibility.Visible;
                else return System.Windows.Visibility.Collapsed;
            }
        }

        #region Init

        public TABLECUSTOM()
        {
            table = new TABLE();
        }

        private void Table_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
            {
                OnPropertyChanged(nameof(IsOnRepair));
                OnPropertyChanged(nameof(IsNotOnRepair));
                OnPropertyChanged(nameof(IsOnService));
            }
        }

        #endregion
    }
}
