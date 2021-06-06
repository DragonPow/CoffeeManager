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
        public TABLE table { get; set; }
        public long Total { get; set; }

        public virtual ObservableCollection<DetailPro> ListPro
        {
            get
            {
                if (_listPro == null) {
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

        #region Init

        public TABLECUSTOM ()
            {
                Total = 0;
            }

        #endregion
    }
}
