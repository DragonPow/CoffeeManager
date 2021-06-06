using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MainProject.HistoryWorkSpace.Converter
{
    class ChangePageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0].GetType() != typeof(Int32) || values[1].GetType() != typeof(Int32)) return 0;
            int currentPage = (int)values[0];
            int allPage = (int)values[1];
            int offset = (int)values[2] ;

            if (offset > 0)
            {
                if (currentPage < allPage) currentPage += offset;
            }
            else
            {
                if (currentPage > 0) currentPage += offset;
            }
            return currentPage;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
