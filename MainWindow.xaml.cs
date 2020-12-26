using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FluentScheduler;
using TimeManagement.ViewModel;
using TimeManagement.Utilities;

namespace TimeManagement
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Snackbar Snackbar;

        public MainWindow()
        {
            InitializeComponent();
            JobManager.Initialize();

            Task.Factory.StartNew(() => Thread.Sleep(1250)).ContinueWith(t =>
            {
                MainSnackbar.MessageQueue?.Enqueue("欢迎来到Time Management时间管理小程序！");
            }, TaskScheduler.FromCurrentSynchronizationContext());

            DataContext = new MainWindowViewModel(MainSnackbar.MessageQueue!);
        }

        private void MaterialItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Link.OpenInBrowser("http://materialdesigninxaml.net/");
        }

        private void DragablzItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Link.OpenInBrowser("https://dragablz.net/");
        }

        private void FluentSchedulerItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Link.OpenInBrowser("https://fluentscheduler.github.io/");
        }

        private void LiveChartsItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Link.OpenInBrowser("https://lvcharts.net/");
        }

        private void MessagePackItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Link.OpenInBrowser("https://msgpack.org/");
        }

        private void WpfAnimatedGifItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Link.OpenInBrowser("https://github.com/XamlAnimatedGif/WpfAnimatedGif");
        }

        private void CosturaItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Link.OpenInBrowser("https://github.com/Fody/Costura");
        }

        private void ActivityWatchItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Link.OpenInBrowser("https://activitywatch.net/");
        }
    }
}