using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MainProject.StatisticWorkSpace.Converter
{
    public class TimeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String rs = "";
            if (value is StatisticModel model && parameter is StatisticMode mode)
            {
                switch (mode)
                {
                    case StatisticMode.DayOfWeek:
                        rs = GetDayOfWeek(model.TimeMin.DayOfWeek);
                        break;
                    case StatisticMode.WeekOfMonth:
                        rs = String.Format("Từ {0} - {1}"
                            , model.TimeMin.ToString("dd/MM")
                            , model.TimeMax.ToString("dd/MM"));
                        break;
                    case StatisticMode.MonthOfYear:
                        rs = String.Format("Năm {0}", model.TimeMin.Year.ToString());
                        break;
                }
            }
            return rs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
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
    }
}
