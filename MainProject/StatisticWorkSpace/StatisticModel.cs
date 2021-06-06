using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.StatisticWorkSpace
{
    class StatisticModel
    {
        public DateTime TimeMin;
        public DateTime TimeMax;
        public int Revenue { get; set; }
        public int Amount { get; set; }
        public String RevenueString => Revenue.ToString("N0") + "đ";
        public String Title { get; set; }
        public String Label { get; set; }
        public StatisticModel()
        {
            TimeMin = new DateTime(2001, 8, 30, 0, 0, 0);
            TimeMax = new DateTime(2001, 8, 30, 23, 59, 59);
            Title = "01/05/2017 - 07/05/2017";
            Label = "Tuần 1 năm 2001";
            Revenue = 50000;
        }
    }
}
