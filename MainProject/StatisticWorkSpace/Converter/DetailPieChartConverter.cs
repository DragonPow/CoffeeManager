using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MainProject.StatisticWorkSpace.Converter
{
    class DetailPieChartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<StatisticModel> list)
            {
                var rs = new SeriesCollection();

                for(int i = 0; i<list.Count; i++)
                {
                    var model = list[i];
                    var series = new PieSeries();
                    series.Title = model.Title;
                    series.Values = new ChartValues<long>() { model.Revenue };
                    rs.Add(series);
                    if (rs.Count >= 49)
                    {
                        var tempSeries = new PieSeries();
                        series.Title = String.Format("Khác ({0})", (list.Count -i).ToString());
                        long val = 0;
                        for (i++; i < list.Count; i++)
                        {
                            val += list[i].Revenue;
                        }
                        series.Values = new ChartValues<long>() { val };
                        rs.Add(series);
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
