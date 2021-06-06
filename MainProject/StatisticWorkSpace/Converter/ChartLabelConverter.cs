using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MainProject.StatisticWorkSpace.Converter
{
    public class ChartLabelConverter : IValueConverter
    {
        public string Axis { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<StatisticModel> list)
            {
                String[] rs = new string[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    rs[i] = list[i].Label;
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
