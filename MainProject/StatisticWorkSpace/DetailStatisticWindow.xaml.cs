using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainProject.StatisticWorkSpace
{
    /// <summary>
    /// Interaction logic for DetailStatisticWindow.xaml
    /// </summary>
    public partial class DetailStatisticWindow : Window
    {
        public DateTime min;
        public DateTime max;

        public DetailStatisticWindow()
        {
            InitializeComponent();
            Activated += DetailStatisticWindow_Activated;
            btnBack.Click += BtnBack_Click;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            this.Close();
        }

        private void DetailStatisticWindow_Activated(object sender, EventArgs e)
        {
            var viewModel = DataContext as DetailStatisticViewModel;
            if (viewModel != null)
            {
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                    new Action<DetailStatisticViewModel, DateTime, DateTime>(setDateRange), viewModel, min, max);
            }
        }

        void setDateRange(DetailStatisticViewModel viewModel, DateTime min, DateTime max)
        {
            viewModel.SetTimeRange(min, max);
        }
    }
}
