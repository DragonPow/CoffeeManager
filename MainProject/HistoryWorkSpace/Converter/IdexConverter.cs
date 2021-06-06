using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using MainProject.ViewModel;

namespace MainProject.HistoryWorkSpace.Converter
{
    class IdexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int index;
            int currentPage = (int)values[1];
            ListViewItem listitem = (ListViewItem)values[0];
            ListView listView = ItemsControl.ItemsControlFromItemContainer(listitem) as ListView;
            index = listView.ItemContainerGenerator.IndexFromContainer(listitem);
            return index + (currentPage - 1) * MainProject.ViewModel.HistoryViewModel.Number_Bill_in_Page + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
