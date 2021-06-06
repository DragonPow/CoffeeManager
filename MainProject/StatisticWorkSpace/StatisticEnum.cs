using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.StatisticWorkSpace
{
    public enum StatisticMode
    {
        DayOfWeek = 0,
        DayOfMonth = 1,
        WeekOfMonth = 2,
        MonthOfYear = 3
    }

    public static class StatisticEnum
    {
        public static String GetString(StatisticMode mode)
        {
            String rs = null;
            switch (mode)
            {
                case StatisticMode.DayOfWeek:
                    rs = "Từng ngày trong tuần";
                    break;
                case StatisticMode.DayOfMonth:
                    rs = "Từng ngày trong tháng";
                    break;
                case StatisticMode.WeekOfMonth:
                    rs = "Từng tuần trong tháng";
                    break;
                case StatisticMode.MonthOfYear:
                    rs = "Từng tháng trong năm";
                    break;
            }
            return rs;
        }
    }
}
