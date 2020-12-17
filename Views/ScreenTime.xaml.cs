using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Windows.Controls;

namespace TimeManagement.Views
{
    /// <summary>
    /// ScreenTime.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenTime : UserControl
    {
        public Func<ChartPoint, string> PointLabel { get; set; }
        public SeriesCollection SeriesCollection { get; set; }

        public ScreenTime()
        {
            InitializeComponent();
            PointLabel = chartPoint => string.Format("{0}({1:p})", chartPoint.Y, chartPoint.Participation);
            DataContext = this;
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> { 3, 5, 7, 4, 6, 8, 1 }
                },
                    new ColumnSeries
                {
                    Values = new ChartValues<decimal> { 3, 5, 7, 4, 6, 8, 1 }
                }
            };
        }

        private void pipChart_DataClick(object sender, ChartPoint chartPoint)
        {
            var chart = (PieChart)chartPoint.ChartView;
            //clear selected slice
            foreach (PieSeries series in chart.Series)
            {
                series.PushOut = 0;
                var selectedSeries = (PieSeries)chartPoint.SeriesView;
                selectedSeries.PushOut = 8;
            }

        }
    }
}