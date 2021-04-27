using MainProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MainProject
{
    class TableViewModel : BaseViewModel
    {
        #region Fields
        private TABLE _table;
        private List<DetailPro> _listPro;
        private long Total;
        private ICommand _getTotal;
        private ICommand _payCommand;
        private ICommand _addDetailPro;

        #endregion

        #region properties

        public TABLE Table
        {
            get
            {
                if (_table == null)
                {
                    _table = new TABLE();
                }
                return _table;
            }
            set
            {
                if (value != _table)
                {
                    _table = value;
                    OnPropertyChanged();
                }
            }
        }


        internal List<DetailPro> ListPro
        {
            get => _listPro;
            set
            {
                if (value != _listPro)
                {
                    _listPro = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Init
        public TableViewModel()
        {
            Table = new TABLE();
            Total = 0;
            ListPro = new List<DetailPro>();
        }
        public TableViewModel(int Number)
        {
            Table = new TABLE();
            Table.Number = Number;
            Total = 0;
            ListPro = new List<DetailPro>();
        }
        #endregion

        #region Command
        public ICommand Pay_Command
        {
            get
            {
                if (_payCommand == null)
                    _payCommand = new RelayingCommand<object>(a => Pay());
                return _payCommand;
            }

        }
        public void Pay()
        {
            //Open BillWordSpace

        }

        public ICommand GetTotal
        {
            get
            {
                if (_getTotal == null)
                    _getTotal = new RelayingCommand<object>(a => getTotal());
                return _getTotal;
            }
        }

        public long getTotal()
        {
            foreach (DetailPro p in ListPro)
                Total += (long)p.Pro.Price * p.Quantity;
            return Total;
        }

        public ICommand AddDetailPro
        {
            get
            {
                if (_addDetailPro == null)
                {
                    _addDetailPro = new RelayingCommand<DetailPro>(a => addDetailPro(a));
                }
                return _addDetailPro;
            }
        }


        public void addDetailPro(DetailPro pro)
        {
            ListPro.Add(pro);
        }

        #endregion

    }
}