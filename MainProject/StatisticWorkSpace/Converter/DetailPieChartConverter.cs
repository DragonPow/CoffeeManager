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
            if (value is List<StatisticModel> list)
            {
                var rs = new SeriesCollection();

                for (int i = 0; i < list.Count; i++)
                {
                    var model = list[i];
                    var series = new PieSeries
                    {
                        Title = model.Title,
                        Tag = string.Format("Top{0}", i + 1),
                        Values = new ChartValues<long>() { model.Revenue }
                    };
                    rs.Add(series);

                    if (rs.Count >= 5)
                    {
                        var tempSeries = new PieSeries
                        {
                            Title = String.Format("Khác ({0})", (list.Count - i).ToString()),
                            Tag = "Khác"
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
