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
    //一条记录
    class Alog
    {
        public DateTime t;
        public string s;

        public Alog(DateTime tt,string ss)
        {
            t = tt;
            s = ss;
        }

        public Alog(int tt, string ss)
        {
            t = new DateTime(2020, 12, 11, tt / 3600, (tt / 60) % 60, tt % 60);
            s = ss;
        }

        public static bool operator <(Alog a, Alog b)
        {
            return a.t < b.t;
        }
        public static bool operator >(Alog a, Alog b)
        {
            return a.t > b.t;
        }

        public static bool operator ==(Alog a, Alog b)
        {
            return a.t == b.t;
        }
        public static bool operator !=(Alog a, Alog b)
        {
            return a.t != b.t;
        }
    }


    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Snackbar Snackbar;
        private Random random = new Random();
        private SortedSet<Alog> logs = new SortedSet<Alog>();


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

        //窗口整体初始化完成后，执行绘制左侧边栏的代码。
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            drawTimeline();

            System.Timers.Timer timer = new System.Timers.Timer(100);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(delegate { aclock.Time = DateTime.Now; }));
        }






        //假设没有后端数据的时候，先生成一些随机的数据作为后端。
        //后端数据的生成方式：时间点+任务。
        //直接用均分的方法，均分段里面有哪个记录多就直接记录为哪个。如果没有记录就不透明度为零（全透明），如果上下两块东西一样就合并。（这个合并比较难。？！）

        public void generatedata()
        {
            logs.Clear();
            List<string> ls = new List<string>(new string[] { "rdgv4v", "c32crq", "q5r34ct34v", "4c3c6c", "4vt3", "34ct9v8", "43vc53", "6b3bv" });
            for (int i = 0; i < 500; i++)
            {
                logs.Add(new Alog(random.Next(86400), ls[random.Next(8)]));
            }
        }

        public void processdata()
        {

        }

        //绘制时间线的函数
        private void drawTimeline()
        {
            //绘制计划的时间线
            planGrid.Children.Clear();
            planGrid.RowDefinitions.Clear();
            for (int i = 0; i < 10; i++)
            {
                planGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(random.NextDouble(), GridUnitType.Star) });
                Button b = new Button();
                planGrid.Children.Add(b);
                b.SetValue(Grid.RowProperty, i);
                b.Margin = new Thickness(5, 2, 3, 2);
                b.Height = double.NaN;
                SolidColorBrush c = new SolidColorBrush(Color.FromArgb(200, (byte)(random.NextDouble() * 256), (byte)(random.NextDouble() * 256), 255));
                b.Background = c;
                b.BorderBrush = c;
                b.Content = "plan" + i.ToString();

            }

            actualGrid.Children.Clear();
            actualGrid.RowDefinitions.Clear();
            for (int i = 0; i < 10; i++)
            {
                actualGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(random.NextDouble(), GridUnitType.Star) });
                Button b = new Button();
                actualGrid.Children.Add(b);
                b.SetValue(Grid.RowProperty, i);
                b.Margin = new Thickness(3, 2, 5, 2);
                if (random.NextDouble() > 0.5) b.Height = 0;
                else b.Height = double.NaN;
                SolidColorBrush c = new SolidColorBrush(Color.FromArgb(200, (byte)(random.NextDouble() * 256), (byte)(random.NextDouble() * 256), 255));
                b.Background = c;
                b.BorderBrush = c;
                b.Content = "actual" + i.ToString();

            }
        }
        private void TimelineGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            drawTimeline();
        }

    }
}
