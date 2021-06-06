using MainProject.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.StatisticWorkSpace
{
    class StatisticViewModel : BaseViewModel, IMainWorkSpace
    {
        public string NameWorkSpace => "Thu ngân";
        private const PackIconKind _iconDisplay = PackIconKind.Finance;
        public PackIcon IconDisplay
        {
            get
            {
                return new PackIcon() { Kind = _iconDisplay, Width = 30, Height = 30 };
            }
        }

        List<StatisticModel> listModel;
        public List<StatisticModel> ListModel { get => listModel; }
        public void SetListModel(List<StatisticModel> list)
        {
            ListModel.Clear();
            foreach (var model in list)
            {
                model.Label = CreateLabel(model);
                model.Title = CreateTitle(model);
            }
            list.Sort((m1, m2) => DateTime.Compare(m1.TimeMin, m2.TimeMin));
            listModel = list;
            OnPropertyChanged(nameof(ListModel));
        }

        StatisticMode currentMode = StatisticMode.DayOfWeek;
        public StatisticMode CurrentMode => currentMode;
        public String CurrentMode_String => StatisticEnum.GetString(currentMode);
        public void SetCurrentMode(int index)
        {
            if (index < 0 && index >= Enum.GetValues(typeof(StatisticMode)).Length) return;
            currentMode = (StatisticMode)index;
        }
        string OPTION_ALL_PRODUCT = "Tất cả sản phẩm";
        public List<string> ListOptionForProduct
        {
            get
            {
                var dbController = new DatabaseController_Statistic();
                var rs =  dbController.getProductNames();
                rs.Insert(0, OPTION_ALL_PRODUCT);
                return rs;
            }
        }
        public String SelectedOptionProduct { get; set; }

        public List<string> ListStatisticModes
        {
            get
            {
                var rs = new List<string>();
                foreach (StatisticMode e in Enum.GetValues(typeof(StatisticMode)))
                {
                    rs.Add(StatisticEnum.GetString(e));
                }
                return rs;
            }
        }

        public long TotalRevenue
        {
            get
            {
                long rs = 0;
                foreach (var model in ListModel) { rs += model.Revenue; }
                return rs;
            }
        }

        public Func<double, string> formaterLabelAxisY { get; set; }
        private String getMoneyLabel(int money)
        {
            string[] prefix = new string[] { "k", "tr", "tỷ" };
            int i = 0;
            while (money / 1000000 != 0)
            {
                money /= 1000; 
                i++;
            }
            if (money < 1000) { return money.ToString(); }
            double rs = money / 1000d;
            return String.Format("{0:0.#}", rs) + prefix[i];
        }

        public void ClearData()
        {
            SetListModel(new List<StatisticModel>());
        }

        public void SetTimeRange(DateTime minDate, DateTime maxDate)
        {
            List<StatisticModel> data = null;
            DatabaseController_Statistic dbController = new DatabaseController_Statistic();
            string option = (SelectedOptionProduct != OPTION_ALL_PRODUCT) ? SelectedOptionProduct : null;

            switch (CurrentMode)
            {
                case StatisticMode.DayOfWeek:
                    data = dbController.statisticByDay(minDate, maxDate, option);
                    break;
                case StatisticMode.DayOfMonth:
                case StatisticMode.WeekOfMonth:
                    data = dbController.statisticByWeek(minDate, maxDate, option);
                    break;
                case StatisticMode.MonthOfYear:
                    data = dbController.statisticByMonth(minDate, maxDate, option);
                    break;
            }
            SetListModel(data);
        }

        public String CreateLabel(StatisticModel model)
        {
            String rs = "<Error>";
            switch (CurrentMode)
            {
                case StatisticMode.DayOfWeek:
                    rs = GetDayOfWeek(model.TimeMin.DayOfWeek);
                    break;
                case StatisticMode.DayOfMonth:
                    rs = model.TimeMin.ToString("dd/MM");
                    break;
                case StatisticMode.WeekOfMonth:
                    rs = String.Format("{0} - {1}"
                        , model.TimeMin.ToString("dd/MM")
                        , model.TimeMax.ToString("dd/MM"));
                    break;
                case StatisticMode.MonthOfYear:
                    rs = String.Format("Tháng {0}", model.TimeMin.Month.ToString());
                    break;
            }
            return rs;
        }

        public String CreateTitle(StatisticModel model)
        {
            String rs = "<Error>";
            switch (CurrentMode)
            {
                case StatisticMode.DayOfWeek:
                    rs =  String.Format("{0} ({1})", GetDayOfWeek(model.TimeMin.DayOfWeek), model.TimeMin.ToString("dd/MM"));
                    break;
                case StatisticMode.DayOfMonth:
                    rs = String.Format("Ngày {0}", model.TimeMin.ToString("dd/MM"));
                    break;
                case StatisticMode.WeekOfMonth:
                    if (model.TimeMax.Day - model.TimeMin.Day < 6)
                    {
                        rs = String.Format("Ngày {0} - {1}", model.TimeMin.ToString("dd/MM"),
                        model.TimeMax.ToString("dd/MM"));
                    }
                    else
                    {
                        rs = String.Format("Tuần {0} - {1}", model.TimeMin.ToString("dd/MM"),
                        model.TimeMax.ToString("dd/MM"));
                    }
                    break;
                case StatisticMode.MonthOfYear:
                    rs = String.Format("Tháng {0}", model.TimeMin.ToString("MM/yyyy"));
                    break;
            }
            return rs;
        }

        public String GetDayOfWeek(DayOfWeek dayOfweek)
        {
            String rs = null;
            switch (dayOfweek)
            {
                case DayOfWeek.Monday:
                    rs = "Thứ Hai";
                    break;
                case DayOfWeek.Tuesday:
                    rs = "Thứ Ba";
                    break;
                case DayOfWeek.Wednesday:
                    rs = "Thứ Tư";
                    break;
                case DayOfWeek.Thursday:
                    rs = "Thứ Năm";
                    break;
                case DayOfWeek.Friday:
                    rs = "Thứ Sáu";
                    break;
                case DayOfWeek.Saturday:
                    rs = "Thứ Bảy";
                    break;
                case DayOfWeek.Sunday:
                    rs = "Chủ Nhật";
                    break;
            }
            return rs;
        }

        
        public StatisticViewModel()
        {
            listModel = new List<StatisticModel>
            {
                
            };
            foreach (var model in ListModel) { model.Label = CreateLabel(model); model.Title = CreateTitle(model); }
            SelectedOptionProduct = OPTION_ALL_PRODUCT;
            formaterLabelAxisY = val => getMoneyLabel((int)val);
            this.PropertyChanged += StatisticViewModel_PropertyChanged;
        }

        private void StatisticViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(ListModel)))
            {
                OnPropertyChanged(nameof(TotalRevenue));
            }
        }
    }
}
