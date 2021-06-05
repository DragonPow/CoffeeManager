﻿using MainProject.MainWorkSpace.Bill;
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
        private int _NumberAllpage;
        public static int Number_Bill_in_Page = 20;      

        private DateTime _BeginTime;
        private DateTime _EndTime;

        ICommand _Load_Detail_Bill;
        ICommand _Exit_Detail_Bill;
        ICommand _Search_Bill;
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
        public int NumberAllPage
        {
            get;
            set;
                      
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
            for (int i=1;i<10;i++)
            {
                ListBill.Add(new BILL()
                {
                    ID = 10,
                    CheckoutDay = new DateTime(2021, i, i),
                    TotalPrice = i * 100,
                    TABLE = new TABLE() { Number = i }
                });
            }
            _NumberPage = 1;

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
            view.DataContext = new BillViewModel() { Total = (long)CurrentBill.TotalPrice, CurrentBill = CurrentBill, CodeDiscount = CurrentBill.VOUCHER.CODE, Discount = ( (int)CurrentBill.TotalPrice*CurrentBill.VOUCHER.Percent/ ( 100 - CurrentBill.VOUCHER.Percent)) };
            view.Show();

        }

        public ICommand Search_BillCommand
        {
            get
            {
                if (_Search_Bill == null)
                    _Search_Bill = new RelayingCommand<object>(a => Search_Bill());
                return _Search_Bill;
            }
        }

        public void Search_Bill()
        {
            if (EndTime < BeginTime)
            {
                WindowService.Instance.OpenMessageBox("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!", "Lỗi", System.Windows.MessageBoxImage.Error);
                return;
            }

            NumberPage = 1;
            using ( var db = new mainEntities())
            {
                NumberAllPage = ((db.BILLs.Where((b => (b.CheckoutDay >= BeginTime && b.CheckoutDay <= EndTime))).Count() / Number_Bill_in_Page) + (db.BILLs.Count() % Number_Bill_in_Page != 0 ? 1 : 0));

                var list = db.BILLs.Where(b => (b.CheckoutDay >= BeginTime && b.CheckoutDay <= EndTime)).OrderBy(b => b.ID).Take(Number_Bill_in_Page).ToList();

                if (list == null) return;

                ListBill = new ObservableCollection<BILL>(list);
            }
           

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
              

                var list = db.BILLs.Where(b => (b.CheckoutDay >= BeginTime && b.CheckoutDay <= EndTime)).OrderBy(b => b.ID).Skip(NumberPage - 1).Take(Number_Bill_in_Page).ToList();

                if (list == null) return;

                ListBill = new ObservableCollection<BILL>(list);
            }
        }
    }
}
