using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using LiveCharts;

namespace MainProject.StatisticWorkSpace.Converter
{
    class ChartValueConverter : IValueConverter
    {
        public string PropertyName { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<StatisticModel> list)
            {
                ChartValues<int> rs = new ChartValues<int>();
                if (PropertyName == "Revenue")
                {
                    foreach (var model in list)
                    {
                        rs.Add(model.Revenue);
                    }
                }
                else if (PropertyName == "Amount")
                {
                    foreach (var model in list)
                    {
                        rs.Add(model.Amount);
                    }
                }
                return rs;
            }
            else { throw new NotImplementedException(); }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
