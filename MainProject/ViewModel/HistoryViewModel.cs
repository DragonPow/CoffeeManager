using MainProject.MainWorkSpace.Bill;
using MainProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MainProject.ViewModel
{
    class HistoryViewModel: BaseViewModel
    {
        #region Fields
        private ObservableCollection<BILL> _ListBill;
        private BILL _CurrentBill;
        private int _NumberPage;
        private int Number_Bill_in_Page = 20;

        private DateTime _BeginTime;
        private DateTime _EndTime;

        ICommand _Load_Detail_Bill;
        ICommand _Exit_Detail_Bill;
        #endregion

        #region Properties
        public ObservableCollection<BILL> ListBill
        {
            get => _ListBill;
            set
            {
                if (_ListBill != value)
                {
                    _ListBill = value;
                    OnPropertyChanged();
                }
            }
        }
        public BILL CurrentBill
        {
            get => _CurrentBill;
            set
            {
                if (value != _CurrentBill)
                {
                    _CurrentBill = value;
                    OnPropertyChanged();
                }
            }
        }

        public int NumberPage
        {
            get => _NumberPage;
            set
            {
                if (value != _NumberPage)
                {
                    _NumberPage = value;
                    OnPropertyChanged();
                    LoadBillByNumberPage();
                }
            }
        }
        public DateTime BeginTime
        {
            get => _BeginTime;
            set
            {
                if (value != _BeginTime)
                {
                    _BeginTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime EndTime
        {
            get => _EndTime;
            set
            {
                if (value != _EndTime)
                {
                    _EndTime = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
        #region Init
        public HistoryViewModel()
        {
            LoadBillByNumberPage();
        }
        #endregion

        #region Command

        public ICommand Load_Detail_BillCommand
        {
            get
            {
                if (_Load_Detail_Bill == null)
                    _Load_Detail_Bill = new RelayingCommand<object>(a => Load_Detail_Bill());
                return _Load_Detail_Bill;
            }
        }

        public void Load_Detail_Bill()
        {
            BillView view = new BillView();
            view.DataContext = CurrentBill;

            view.Show();

        }

        public ICommand Exit_Detail_BillCommand
        {
            get
            {
                if (_Exit_Detail_Bill == null)
                    _Exit_Detail_Bill = new RelayingCommand<BillView>( v => v.Close() );
                return _Exit_Detail_Bill;
            }
        }
        #endregion

        void LoadBillByNumberPage()
        {
            using (var db = new mainEntities())
            {

                ListBill = new ObservableCollection<BILL>(db.BILLs.Where(b => (b.ID < NumberPage * Number_Bill_in_Page && b.ID >= (NumberPage - 1) * (Number_Bill_in_Page) && b.CheckoutDay >= BeginTime && b.CheckoutDay <=EndTime)).ToList());

            }
        }
    }
}
