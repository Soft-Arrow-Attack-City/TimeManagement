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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeManagement.Views
{
    /// <summary>
    /// Timeline.xaml 的交互逻辑
    /// </summary>
    public partial class Timeline : UserControl
    {
        private double mouseY = 0;
        private Random random = new Random();
        private string[] ScreenUsage1h;//按时间顺序排字符串，如果没有记录，直接放null进来。//4小时刻度出现时
        private string[] ScreenUsage30min;//按时间顺序排字符串，如果没有记录，直接放null进来。//1小时刻度出现时
        private string[] ScreenUsage15min;//按时间顺序排字符串，如果没有记录，直接放null进来。//15分钟刻度出现时
        private string[] ScreenUsage5min;//按时间顺序排字符串，如果没有记录，直接放null进来。//5分钟刻度出现时
        private string[] ScreenUsage1min;//按时间顺序排字符串，如果没有记录，直接放null进来。//1分钟刻度出现时

        private double startsecond, endsecond;




        public Timeline()
        {
            InitializeComponent();
            startsecond = 28800;
            endsecond = 86400;
            LoadData();

        }

        //窗口加载完成后执行，在xaml里添加loaded消息。
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            drawTimeline();
        }

        //将真的或者假的数据载入进来，并进行一定的处理
        private void LoadData()
        {

            ScreenUsage1h = new string[24];
            ScreenUsage30min = new string[48];
            ScreenUsage15min = new string[96];
            ScreenUsage5min = new string[288];
            ScreenUsage1min = new string[1440];

            //获取真的或者假的屏幕使用记录数据，要排好序的。尽管其中的时间粒度可能很粗或者很细，很不均匀。
            List<DataModel.Alog> winlog = DataModel.TimelineData.generatedata();

            Dictionary<string, int> count1min = new Dictionary<string, int>();
            Dictionary<string, int> count5min = new Dictionary<string, int>();
            Dictionary<string, int> count15min = new Dictionary<string, int>();
            Dictionary<string, int> count30min = new Dictionary<string, int>();
            Dictionary<string, int> count1h = new Dictionary<string, int>();


            int nlogs = winlog.Count;
            int now1min = 0;
            int now5min = 0;
            int now15min = 0;
            int now30min = 0;
            int now1h = 0;
            for(int i = 0; i < nlogs; i++)
            {
                if (winlog[i].t / 60 == now1min)
                {
                    if (count1min.ContainsKey(winlog[i].s))
                    {
                        count1min[winlog[i].s] += 1;
                    }
                    else count1min.Add(winlog[i].s, 1);
                }
                else if (count1min.Count > 0)
                {

                    ScreenUsage1min[now1min]=count1min.Where(kvp => kvp.Value == count1min.Max(kvp => kvp.Value)).First().Key;
                    now1min = winlog[i].t / 60;
                    count1min.Clear();
                    count1min.Add(winlog[i].s, 1);
                }

                if (winlog[i].t / 300 == now5min)
                {
                    if (count5min.ContainsKey(winlog[i].s))
                    {
                        count5min[winlog[i].s] += 1;
                    }
                    else count5min.Add(winlog[i].s, 1);
                }
                else if (count5min.Count > 0)
                {
                    ScreenUsage5min[now5min] = count5min.Where(kvp => kvp.Value == count5min.Max(kvp => kvp.Value)).First().Key;
                    now5min = winlog[i].t / 300;
                    count5min.Clear();
                    count5min.Add(winlog[i].s, 1);
                }

                if (winlog[i].t / 900 == now15min)
                {
                    if (count15min.ContainsKey(winlog[i].s))
                    {
                        count15min[winlog[i].s] += 1;
                    }
                    else count15min.Add(winlog[i].s, 1);
                }
                else if (count15min.Count > 0)
                {
                    ScreenUsage15min[now15min] = count15min.Where(kvp => kvp.Value == count15min.Max(kvp => kvp.Value)).First().Key;
                    now15min = winlog[i].t / 900;
                    count15min.Clear();
                    count15min.Add(winlog[i].s, 1);
                }

                if (winlog[i].t / 1800 == now30min)
                {
                    if (count30min.ContainsKey(winlog[i].s))
                    {
                        count30min[winlog[i].s] += 1;
                    }
                    else count30min.Add(winlog[i].s, 1);
                }
                else if (count30min.Count > 0)
                {
                    ScreenUsage30min[now30min] = count30min.Where(kvp => kvp.Value == count30min.Max(kvp => kvp.Value)).First().Key;
                    now30min = winlog[i].t / 1800;
                    count30min.Clear();
                    count30min.Add(winlog[i].s, 1);
                }

                if (winlog[i].t / 3600 == now1h)
                {
                    if (count1h.ContainsKey(winlog[i].s))
                    {
                        count1h[winlog[i].s] += 1;
                    }
                    else count1h.Add(winlog[i].s, 1);
                }///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////有bug：没有处理最后一个块的内容；clear和add不应只在count>0时发生。
                else if (count1h.Count > 0)
                {
                    ScreenUsage1h[now1h] = count1h.Where(kvp => kvp.Value == count1h.Max(kvp => kvp.Value)).First().Key;
                    count1h.Clear();
                    count1h.Add(winlog[i].s, 1);
                }


                now1min = winlog[i].t / 60;
                now5min = winlog[i].t / 300;
                now15min = winlog[i].t / 900;
                now30min = winlog[i].t / 1800; 
                now1h = winlog[i].t / 3600;
            }

        }



        //绘制时间线的函数
        public void drawTimeline()
        {

            //绘制计划的时间线，暂时先用随机数据。
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

            //绘制实际的时间线。采用winlog中的数据。




            actualGrid.Children.Clear();
            actualGrid.RowDefinitions.Clear();
            //只能显示出从startsecond到endsecond的部分，而且只能显示某一天。（按说应该是会给一个选择日期的按钮的）

            
            for (int i = 0; i < 24; i++)
            {
                actualGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                Button b = new Button();
                actualGrid.Children.Add(b);
                b.SetValue(Grid.RowProperty, i);
                b.Margin = new Thickness(3, 2, 5, 2);
                b.Height = double.NaN;
                SolidColorBrush c = new SolidColorBrush(Color.FromArgb(200, (byte)(random.NextDouble() * 256), (byte)(random.NextDouble() * 256), 255));
                b.Background = c;
                b.BorderBrush = c;
                b.Content = ScreenUsage1h[i];

            }
            
            drawTime();
        }

        public void drawTime()
        {
            timeGrid.Children.Clear();
            //4小时刻度出现：0，4，8，12，16，20，24。（一直都在）
            for (int drawtime = (int)startsecond / 60 / 60 / 4; drawtime <= endsecond / 60 / 60 / 4; drawtime++)
            {

                double margin = timeGrid.ActualHeight * (drawtime * 60 * 60 * 4 - startsecond) / (endsecond - startsecond) - 20;
                if (margin > -30)
                {
                    TextBlock a = new TextBlock();
                    a.Text = (drawtime * 4).ToString("D2") + ":00";
                    a.HorizontalAlignment = HorizontalAlignment.Right;
                    a.Margin = new Thickness(0, margin, 0, 0);
                    timeGrid.Children.Add(a);
                }

            }
            //1小时刻度出现：当间隔小于12小时
            if (endsecond - startsecond < 12 * 60 * 60)
            {
                for (int drawtime = (int)startsecond / 60 / 60; drawtime <= endsecond / 60 / 60; drawtime++)
                {
                    double margin = timeGrid.ActualHeight * (drawtime * 60 * 60 - startsecond) / (endsecond - startsecond) - 20;
                    if (margin > -30)
                    {
                        TextBlock a = new TextBlock();
                        a.Text = drawtime.ToString("D2") + ":00";
                        a.HorizontalAlignment = HorizontalAlignment.Right;
                        a.Margin = new Thickness(0, margin, 0, 0);
                        timeGrid.Children.Add(a);
                    }

                }
            }

            //15分钟刻度出现：当间隔小于3小时
            if (endsecond - startsecond < 3 * 60 * 60)
            {
                for (int drawtime = (int)startsecond / 60 / 15; drawtime <= endsecond / 60 / 15; drawtime++)
                {

                    double margin = timeGrid.ActualHeight * (drawtime * 60 * 15 - startsecond) / (endsecond - startsecond) - 20;
                    if (margin > -30)
                    {
                        TextBlock a = new TextBlock();
                        a.Text = (drawtime / 4).ToString("D2") + ":" + (drawtime % 4 * 15).ToString("D2");
                        a.HorizontalAlignment = HorizontalAlignment.Right;
                        a.Margin = new Thickness(0, margin, 0, 0);
                        timeGrid.Children.Add(a);

                    }
                }
            }
            //5分钟刻度出现：当间隔小于45分钟
            if (endsecond - startsecond < 45 * 60)
            {
                for (int drawtime = (int)startsecond / 60 / 5; drawtime <= endsecond / 60 / 5; drawtime++)
                {

                    double margin = timeGrid.ActualHeight * (drawtime * 60 * 5 - startsecond) / (endsecond - startsecond) - 20;
                    if (margin > -30)
                    {
                        TextBlock a = new TextBlock();
                        a.Text = (drawtime / 12).ToString("D2") + ":" + (drawtime % 12 * 5).ToString("D2");
                        a.HorizontalAlignment = HorizontalAlignment.Right;
                        a.Margin = new Thickness(0, margin, 0, 0);
                        timeGrid.Children.Add(a);

                    }

                }
            }
            //1分钟刻度出现：当间隔小于10分钟
            if (endsecond - startsecond < 10 * 60)
            {
                for (int drawtime = (int)startsecond / 60 / 1; drawtime <= endsecond / 60 / 1; drawtime++)
                {

                    double margin = timeGrid.ActualHeight * (drawtime * 60 - startsecond) / (endsecond - startsecond) - 20;
                    if (margin > -30)
                    {
                        TextBlock a = new TextBlock();
                        a.Text = (drawtime / 60).ToString("D2") + ":" + (drawtime % 60).ToString("D2");
                        a.HorizontalAlignment = HorizontalAlignment.Right;
                        a.Margin = new Thickness(0, margin, 0, 0);
                        timeGrid.Children.Add(a);
                    }


                }
            }
        }

        private void TimelineGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseY = e.GetPosition(timeGrid).Y;
        }


        private void TimelineGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton==MouseButtonState.Pressed)
            {
                //认为是在中键拖动
                double dragseconds = (mouseY-e.GetPosition(timeGrid).Y) / timeGrid.ActualHeight * (endsecond - startsecond);
                //MessageBox.Show(dragseconds.ToString());
                mouseY = e.GetPosition(timeGrid).Y;
                if ((startsecond + dragseconds >= 0) && (endsecond + dragseconds <= 86400)){
                    startsecond += dragseconds;
                    endsecond += dragseconds;
                }


                drawTimeline();
            }
        }


        private void TimelineGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //以鼠标点为中心缩放。总缩放指标为全长的0.2倍，按比例分给startsecond和endsecond。
            double pointprop = (double)(e.GetPosition(timeGrid).Y) / timeGrid.ActualHeight;
            if (pointprop > 0)
            {
                double totalsecond = (endsecond - startsecond) * 0.2;
                if (e.Delta > 0)
                {
                    startsecond -= totalsecond * pointprop;
                    endsecond += totalsecond * (1 - pointprop);
                }
                else
                {
                    startsecond += totalsecond * pointprop;
                    endsecond -= totalsecond * (1 - pointprop);
                }
                //这四条限制最小分度和上下位置，顺序不能随意调换！否则出bug
                if (endsecond - startsecond < 300)
                {
                    totalsecond = 300 - (endsecond - startsecond);
                    startsecond -= totalsecond * pointprop;
                    endsecond += totalsecond * (1 - pointprop);
                }
                if (startsecond < 0) startsecond = 0;
                if (endsecond - startsecond < 300) endsecond = startsecond + 300;
                if (endsecond > 86400) endsecond = 86400;
                if (endsecond - startsecond < 300) startsecond = endsecond - 300;
            }

            drawTimeline();
        }

    }
}
