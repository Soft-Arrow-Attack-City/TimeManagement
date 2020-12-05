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

namespace TimeManagement
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Snackbar Snackbar;
        private Random random = new Random();


        public MainWindow()
        {
            InitializeComponent();

            Task.Factory.StartNew(() => Thread.Sleep(2500)).ContinueWith(t =>
            {
                //note you can use the message queue from any thread, but just for the demo here we 
                //need to get the message queue from the snackbar, so need to be on the dispatcher
                MainSnackbar.MessageQueue?.Enqueue("欢迎来到Time Management时间管理小程序！");
            }, TaskScheduler.FromCurrentSynchronizationContext());

            //DataContext = new MainWindowViewModel(MainSnackbar.MessageQueue!);

            Snackbar = MainSnackbar;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            //////////////////////////////////////////////////垂直alignment设置成Stretch为什么不管用！！！！！！！！！！！！！！！！！！！！！！
            planGrid.Children.Clear();
            planGrid.RowDefinitions.Clear();
            for (int i = 0; i < 16; i++)
            {
                
                planGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100+i, GridUnitType.Star) });


                Button b = new Button();
                planGrid.Children.Add(b);
                b.SetValue(Grid.RowProperty, i);
                b.Margin = new Thickness(5, 2, 5, 2);
                //b.VerticalAlignment = VerticalAlignment.Stretch;

                SolidColorBrush c = new SolidColorBrush(Color.FromArgb(255, (byte)(random.NextDouble() * 256), (byte)(random.NextDouble() * 256), 255));
                b.Background = c;
                b.BorderBrush = c;
                b.Content = "plan";

            }

            actualGrid.Children.Clear();
            actualGrid.RowDefinitions.Clear();
            for (int i = 0; i < 16; i++)
            {

                actualGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100+15-i, GridUnitType.Star) });


                Button b = new Button();
                actualGrid.Children.Add(b);
                b.SetValue(Grid.RowProperty, i);
                b.Margin = new Thickness(5, 2, 5, 2);
                //b.VerticalAlignment = VerticalAlignment.Stretch;

                SolidColorBrush c = new SolidColorBrush(Color.FromArgb(255, (byte)(random.NextDouble() * 256), (byte)(random.NextDouble() * 256), 255));
                b.Background = c;
                b.BorderBrush = c;
                b.Content = "actual";

            }

        }


        
    }
}
