﻿
using MainProject.MainWorkSpace.Bill;
using MainProject.MainWorkSpace.Table;
using MainProject.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace MainProject.ViewModel
{
   public  class TableViewModel : BaseViewModel, IMainWorkSpace
    {
        public string NameWorkSpace => "Quản lý bàn";
        private const PackIconKind _iconDisplay = PackIconKind.TableChair;
        public PackIcon IconDisplay
        {
            get
            {
                return new PackIcon() { Kind = _iconDisplay, Width = 30, Height = 30 };
            }
        }

        #region Field
        private ObservableCollection<TABLECUSTOM> _ListTable;
        private ObservableCollection<int> _ListFloor;
        private int _Currentfloors;
        private TABLECUSTOM _CurrentTable;
        private DetailPro _CurrentDetailPro;
        private ObservableCollection<DetailPro> _Currentlistdetailpro;
        private long _TotalCurrentTable;
        private bool _Isbringtohome;

        private BillViewModel _Billviewmodel;

        private ICommand _plusQuantityDetailProCommand;
        private ICommand _minusQuantityDetailProCommand;
        private ICommand _ClickQuantityDetailProCommand;        
        private ICommand _DeleteDetailPro;

        private ICommand _OpenViewChooseTable;
        private ICommand _CloseViewChooseTable;

        private ICommand _PayCommand;

        private ICommand _DeleteTableCommand;
        private ICommand _InsertTableCommand;
        private ICommand _UpdateStatusTableCommand;

        private ICommand _AddFloor;
        private ICommand _DeleteFloor;

        private string _TableName = "Chọn bàn";

        #endregion


        #region Init


        public TableViewModel()
        {
            CurrentFloors = 1;
            TotalCurrentTable = 0;

            using (var db = new mainEntities())
            {
                var listtab = db.TABLEs.Where(t => (t.Floor == CurrentFloors)).ToList();

                if (listtab == null) return;

                List<TABLECUSTOM> Tablecustoms = new List<TABLECUSTOM>();

                foreach (TABLE t in listtab)
                {
                    Tablecustoms.Add(new TABLECUSTOM() { Total = 0, table = t, ListPro = null}) ;
                }

                ListTable = new ObservableCollection<TABLECUSTOM>(Tablecustoms);
                ListFloor = new ObservableCollection<int>();

                var list = db.TABLEs.Where( f => f.Floor != 0).Select(f => f.Floor ).Distinct();

                foreach( int f in list)
                {
                    ListFloor.Add(f);
                }    
            }
        }
        #endregion



        #region Properties

        public bool Isbringtohome
        {
            get => _Isbringtohome;
            set
            {
                if (_Isbringtohome != value)
                {
                    _Isbringtohome = value;
                    OnPropertyChanged();
                    if ( value == true)
                    {
                        TableName = "Mang về";
                        CurrentTable = null;
                    }
                    else
                         if (CurrentTable == null) TableName = "Chọn bàn >";
                }
            }
        }
        public BillViewModel Billviewmodel
        {
            get => _Billviewmodel;
            set
            {
                if (_Billviewmodel != value)
                {
                    _Billviewmodel = value;
                    OnPropertyChanged();              
                }
            }
        }
        public long TotalCurrentTable { get => _TotalCurrentTable; set { if (_TotalCurrentTable != value) { _TotalCurrentTable = value; OnPropertyChanged(); } } }
        public ObservableCollection<DetailPro> Currentlistdetailpro 
        { 
            get => _Currentlistdetailpro; 
            set 
            { 
                if (_Currentlistdetailpro != value) 
                { 
                    _Currentlistdetailpro = value; 
                    OnPropertyChanged();
                   
                } 
            } 
        }
        public ObservableCollection<int> ListFloor
        {
            get => _ListFloor;
            set
            {
                if (_ListFloor != value)
                {
                    _ListFloor = value; OnPropertyChanged();
                }
            }
        }

        public int CurrentFloors
        {
            get => _Currentfloors;
            set
            {
                if (value != _Currentfloors)
                {
                    _Currentfloors = value;
                    OnPropertyChanged();

                    using (var db = new mainEntities())
                    {
                        var listtab = db.TABLEs.Where(t => (t.Floor == CurrentFloors)).ToList();

                        if (listtab == null) return;

                        List<TABLECUSTOM> Tablecustoms = new List<TABLECUSTOM>();

                        foreach (TABLE t in listtab)
                        {
                            Tablecustoms.Add(new TABLECUSTOM() { Total = 0, table = t, ListPro = null });
                        }

                        ListTable = new ObservableCollection<TABLECUSTOM>(Tablecustoms);

                    }

                }
            }
        }
        public TABLECUSTOM CurrentTable
        {
            get => _CurrentTable;
            set
            {
                if (value != _CurrentTable)
                {
                    _CurrentTable = value;
                    OnPropertyChanged();
                    if (value != null)
                    {
                        TableName = "Bàn: " + value.table.Name.ToString();
                        Isbringtohome = false;
                        CloseChooseTable();
                    }
                    else
                         if (!Isbringtohome) TableName = "Chọn bàn >";
                }
            }
      
        }

        public DetailPro CurrentDetailPro
        {
            get => _CurrentDetailPro;
            set
            {
                if (value != _CurrentDetailPro)
                {
                    _CurrentDetailPro = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<TABLECUSTOM> ListTable
        {
            get
            {
                if (_ListTable == null)
                {
                    _ListTable = new ObservableCollection<TABLECUSTOM>();
                }
                return _ListTable;
            }
            set
            {
                if (_ListTable != value)
                {
                    _ListTable = value;
                    OnPropertyChanged();
                }
            }
        }

       
            public string TableName
            {
            get => _TableName;            
            set
            {
                if (_TableName != value)
                {
                    _TableName = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Command

        public ICommand PlusDetailProQuantityCommand
        {
            get
            {
                if (_plusQuantityDetailProCommand == null)
                    _plusQuantityDetailProCommand = new RelayingCommand<object>(a => Plus());
                return _plusQuantityDetailProCommand;
            }
        }

        public void Plus()
        {
            CurrentDetailPro.Quantity++;
            TotalCurrentTable += (long) CurrentDetailPro.Pro.Price;

        }
        public ICommand MinusDetailProQuantityCommand
        {
            get
            {
                if (_minusQuantityDetailProCommand == null)
                    _minusQuantityDetailProCommand = new RelayingCommand<object>(a => Minus());

                return _minusQuantityDetailProCommand;
            }

        }

        public void Minus()
        {
            if (CurrentDetailPro.Quantity < 1) return;

            CurrentDetailPro.Quantity--;
            TotalCurrentTable -=  CurrentDetailPro.Pro.Price;

            if (CurrentDetailPro.Quantity == 0) Currentlistdetailpro.Remove(CurrentDetailPro);
        }

        public ICommand ClickQuantityDetailProCommand
        {
            get
            {
                if (_ClickQuantityDetailProCommand == null)
                {
                    _ClickQuantityDetailProCommand = new RelayingCommand<int>(a => ChangeQuantityCommand(a));
                }
                return _ClickQuantityDetailProCommand;
            }
        }

        public void ChangeQuantityCommand(int Number)
        {
            TotalCurrentTable -= CurrentDetailPro.Quantity * (int)CurrentDetailPro.Pro.Price;
            CurrentDetailPro.Quantity = Number;
            TotalCurrentTable += Number * (int)CurrentDetailPro.Pro.Price;
        }


        public ICommand OpenViewChooseTableCommand
        {
            get
            {
                if (_OpenViewChooseTable == null)
                {
                    _OpenViewChooseTable = new RelayingCommand<Object>(a => OpenChooseTable());
                }
                return _OpenViewChooseTable;
            }
        }

        public void OpenChooseTable( )
        {
            SelectTableView v = new SelectTableView();
            WindowService.Instance.OpenWindow(this, v);
        }

        public ICommand CloseViewChooseTableCommand
        {
            get
            {
                if (_CloseViewChooseTable == null)
                {
                    _CloseViewChooseTable = new RelayingCommand<Object>(a => CloseChooseTable());
                }
                return _CloseViewChooseTable;
            }
        }

        public void CloseChooseTable()
        {
            Window window = WindowService.Instance.FindWindowbyTag("Selected Table").First();
            window.Close();
        }



      

        public ICommand DeleteDetailProCommand
        {
            get
            {
                if (_DeleteDetailPro == null)
                {
                    _DeleteDetailPro = new RelayingCommand<Object>(a => RemoveDetail());
                }
                return _DeleteDetailPro;
            }
        }
        public void RemoveDetail()
        {
            TotalCurrentTable -=  CurrentDetailPro.Quantity + (int)CurrentDetailPro.Pro.Price;
            Currentlistdetailpro.Remove(CurrentDetailPro);
        }

        public ICommand Pay_Command
        {
            get
            {
                if (_PayCommand == null)
                    _PayCommand = new RelayingCommand<object>(a => Pay());
                return _PayCommand;
            }

        }
        public void Pay()
        {
            if (CurrentTable == null && !Isbringtohome)
            {
                WindowService.Instance.OpenMessageBox("Chưa chọn bàn", "Lỗi", System.Windows.MessageBoxImage.Error);
                return;
            }

            if ( Isbringtohome)
            {
                using (var db = new mainEntities())
                {
                    CurrentTable = new TABLECUSTOM() { table = db.TABLEs.Where(t => t.ID == 0).FirstOrDefault() };
                }
            }

            if (Currentlistdetailpro == null || Currentlistdetailpro.Count == 0 )
            {
                WindowService.Instance.OpenMessageBox("Chưa có món được chọn!", "Lỗi", System.Windows.MessageBoxImage.Error);
                return;
            }                

            CurrentTable.ListPro = Currentlistdetailpro;
            CurrentTable.Total = TotalCurrentTable;

            Billviewmodel = new BillViewModel(CurrentTable);
            
            BillView billView = new BillView();
            billView.DataContext = Billviewmodel;

            billView.ShowDialog();

            if (Billviewmodel.IsClose)
            {
                CurrentTable = null;
                Currentlistdetailpro = new ObservableCollection<DetailPro>();
                TotalCurrentTable = 0;
            }

        }


        public ICommand DeleteTableCommand
        {
            get
            {
                if (_DeleteTableCommand == null)
                {
                    _DeleteTableCommand = new RelayingCommand<int>(a => Delete(a));
                }
                return _DeleteTableCommand;
            }
        }

        private void Delete(int number)
        {
            ListTable.RemoveAt(number - 1);

            for (int i = number; i < ListTable.Count; ++i)
                --ListTable[i].table.Name;

            using (var db = new mainEntities())
            {
                TABLE table = db.TABLEs.Where(d => (d.Name == number && d.Floor == CurrentFloors)).FirstOrDefault();

                if (table != null)
                {
                    var TABLEs = db.TABLEs.Where(t => t.Name > number && t.Floor == CurrentFloors);

                    foreach (TABLE tab in TABLEs)
                    {
                        --tab.Name;
                    }

                }

                db.SaveChanges();
            }
        }

        public ICommand InsertTableCommand
        {
            get
            {
                if (_InsertTableCommand == null)
                {
                    _InsertTableCommand = new RelayingCommand<object>(a => Insert());
                }
                return _InsertTableCommand;
            }
        }

        public void Insert()
        {
            TABLE tab = new TABLE() { Floor = CurrentFloors, STATUS_TABLE = new STATUS_TABLE() { Status = "Empty"}, Name = ListTable.Count + 1};

            ListTable.Add(new TABLECUSTOM() { table = tab });

            using (var db = new mainEntities())
            {
                db.TABLEs.Add(tab);
                db.SaveChanges();
            }

        }

        ICommand UpdateStatusTableCommand
        {
            get
            {
                if (_UpdateStatusTableCommand == null)
                {
                    _UpdateStatusTableCommand = new RelayingCommand<int>(a => Update(a));
                }
                return _UpdateStatusTableCommand;
            }
        }

        public void Update(int Number)
        {

        }

     
                   
        #endregion

    }
}