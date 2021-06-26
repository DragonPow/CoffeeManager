using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.StatisticWorkSpace
{
    class DetailStatisticViewModel : StatisticViewModel
    {
        string dateTimeRangeTitle;
        public string DateTimeRangeTitle => dateTimeRangeTitle;
        public string TitleDataGrid => String.Format("Báo Cáo Bán Hàng {0}", DateTimeRangeTitle);
        public override void SetTimeRange(DateTime minDate, DateTime maxDate)
        {
            List<StatisticModel> data;
            DatabaseController_Statistic dbController = new DatabaseController_Statistic();

            data = dbController.statisticByName(minDate, maxDate);

            dateTimeRangeTitle = getDateTimeRangeString(minDate, maxDate);

            ListModel.Clear();
            data.Sort((m1, m2) => (m1.Revenue < m2.Revenue) ? 1 : -1);
            listModel = new System.Collections.ObjectModel.ObservableCollection<StatisticModel>(data);
            OnPropertyChanged(nameof(ListModel));
        }

        public String getDateTimeRangeString(DateTime minDate, DateTime maxDate)
        {
            String rs = String.Format("{0} - {1}", minDate.ToString("dd/MM/yyy"), maxDate.ToString("dd/MM/yyy"));
            rs = String.Format("tháng {0}", minDate.ToString("MM/yyyy"));
            if (maxDate.Year == minDate.Year)
            {
                if (maxDate.Month == minDate.Month)
                {
                    if (minDate.Day == 1 && maxDate.Day == DateTime.DaysInMonth(maxDate.Year, maxDate.Month))
                    {
                        rs = String.Format("tháng {0}", minDate.ToString("MM/yyyy"));
                    }
                }
            }
            return rs;
        }

        public override String CreateTitle(StatisticModel model)
        {
            return model.Title;
        }
        public override String CreateLabel(StatisticModel model)
        {
            return model.Label;
        }

        public DetailStatisticViewModel()
        {
            formaterLabelAxisY = null;
            dateTimeRangeTitle = "tháng 5/2021";
        }
    }
}
