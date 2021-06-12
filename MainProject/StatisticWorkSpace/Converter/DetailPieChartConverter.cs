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
        public DetailPieChartConverter()
        {
            PointLabelFormatter = cp => string.Format("({0:P})", cp.Participation);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Collections.ObjectModel.ObservableCollection<StatisticModel> list)
            {
                var rs = new SeriesCollection();

                for (int i = 0; i < list.Count; i++)
                {
                    var model = list[i];
                    var series = new PieSeries
                    {
                        Title = model.Title,
                        Tag = string.Format("Hạng {0}", i + 1),
                        Values = new ChartValues<long>() { model.Revenue }
                    };
                    rs.Add(series);

                    if (rs.Count >= 7 && rs.Count<list.Count)
                    {
                        var tempSeries = new PieSeries
                        {
                            Title = String.Format("Còn lại ({0})", (list.Count - rs.Count).ToString()),
                            Tag = "Còn lại"
                        };

                        long val = 0;
                        for (i++; i < list.Count; i++)
                        {
                            val += list[i].Revenue;
                        }
                        tempSeries.Values = new ChartValues<long>() { val };
                        rs.Add(tempSeries);
                    }
                }

                foreach (PieSeries series in rs)
                {
                    series.DataLabels = true;
                    series.LabelPoint = cp => string.Format("{0} ({1:P})", series.Tag, cp.Participation);
                }
                return rs;
            }
            else { throw new NotImplementedException(); }
        }

        public Func<ChartPoint, String> PointLabelFormatter { get; set; }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
