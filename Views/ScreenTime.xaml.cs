using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FluentScheduler;

namespace TimeManagement.Views
{
    /// <summary>
    /// ScreenTime.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenTime : UserControl, INotifyPropertyChanged
    {
        private double _lastLecture;
        private double _trend;

        public ScreenTime()
        {
            InitializeComponent();

            LastHourSeries = new SeriesCollection
            {
                new LineSeries
                {
                    AreaLimit = -10,
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(3),
                        new ObservableValue(5),
                        new ObservableValue(6),
                        new ObservableValue(7),
                        new ObservableValue(3),
                        new ObservableValue(4),
                        new ObservableValue(2),
                        new ObservableValue(5),
                        new ObservableValue(8),
                        new ObservableValue(3),
                        new ObservableValue(5),
                        new ObservableValue(6),
                        new ObservableValue(7),
                        new ObservableValue(3),
                        new ObservableValue(4),
                        new ObservableValue(2),
                        new ObservableValue(5),
                        new ObservableValue(8)
                    }
                }
            };
            _trend = 0;

            DateTime now = DateTime.Now;

            bool timeout = false;

            JobManager.AddJob(() =>
            {
                JobManager.AddJob(
                    () => TimeUp(out timeout),
                    s => s.ToRunOnceIn(5).Seconds()
                );
                var r = new Random();
                while (!timeout)
                {
                    Thread.Sleep(500);
                    _trend += (r.NextDouble() > 0.3 ? 1 : 0.5) * r.Next(0, 2);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        LastHourSeries[0].Values.Add(new ObservableValue(_trend));
                        LastHourSeries[0].Values.RemoveAt(0);
                        SetLecture();
                    });
                }
                timeout = false;
                _trend = 0;
            },
            s => s.ToRunNow().AndEvery(8).Seconds().DelayFor(5).Seconds());

            DataContext = this;
        }

        private void TimeUp(out bool Timeout)
        {
            Timeout = true;
        }

        public SeriesCollection LastHourSeries { get; set; }

        public double LastLecture
        {
            get { return _lastLecture; }
            set
            {
                _lastLecture = value;
                OnPropertyChanged("LastLecture");
            }
        }

        private void SetLecture()
        {
            var target = ((ChartValues<ObservableValue>)LastHourSeries[0].Values).Last().Value;
            var step = (target - _lastLecture) / 4;
            Task.Run(() =>
            {
                for (var i = 0; i < 4; i++)
                {
                    Thread.Sleep(100);
                    LastLecture += step;
                }
                LastLecture = target;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateOnclick(object sender, RoutedEventArgs e)
        {
            TimePowerChart.Update(true);
        }
    }
}