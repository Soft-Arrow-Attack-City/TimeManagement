using FluentScheduler;
using MaterialDesignThemes.Wpf;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TimeManagement.ViewModel;

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
    }
}