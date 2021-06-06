using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MainProject.StatisticWorkSpace.Converter
{
    public class MoneyConverter : IValueConverter
    {
        String currency = "đ";
        public String Currency
        {
            get => currency;
            set => currency = value;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!targetType.Equals(typeof(String))) { throw new NotImplementedException(); }

            String rs = "Cannot convert";
            long money;
            if (long.TryParse(value.ToString(),out money))
            {
                rs = money.ToString("N0")+currency;
            }
            return rs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
