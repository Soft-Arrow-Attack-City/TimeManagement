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

        //这个消息响应让时间跟着窗口大小变化。
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            drawTime();
        }

        //将真的或者假的数据载入进来，并进行一定的处理
        private bool LoadData()
        {
            //修复24:00:00的bug，要加1。
            ScreenUsage1h = new string[24 + 1];
            ScreenUsage30min = new string[48 + 1];
            ScreenUsage15min = new string[96 + 1];
            ScreenUsage5min = new string[288 + 1];
            ScreenUsage1min = new string[1440 + 1];

            //获取真的或者假的屏幕使用记录数据，要排好序的。尽管其中的时间粒度可能很粗或者很细，很不均匀。
            List<DataModel.Alog> winlog = DataModel.TimelineData.generatedata();

            Dictionary<string, int> count1min = new Dictionary<string, int>();
            Dictionary<string, int> count5min = new Dictionary<string, int>();
            Dictionary<string, int> count15min = new Dictionary<string, int>();
            Dictionary<string, int> count30min = new Dictionary<string, int>();
            Dictionary<string, int> count1h = new Dictionary<string, int>();


            int nlogs = winlog.Count;
            if (nlogs == 0) return false;//当日没有数据！

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
                else
                {
                    if (count1min.Count > 0)
                    {
                        ScreenUsage1min[now1min] = count1min.Where(kvp => kvp.Value == count1min.Max(kvp => kvp.Value)).First().Key;
                        count1min.Clear();
                    }
                    now1min = winlog[i].t / 60; 
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
                else 
                {
                    if (count5min.Count > 0)
                    {
                        ScreenUsage5min[now5min] = count5min.Where(kvp => kvp.Value == count5min.Max(kvp => kvp.Value)).First().Key;
                        count5min.Clear();
                    }
                        
                    now5min = winlog[i].t / 300;
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
                else 
                {
                    if (count15min.Count > 0)
                    {
                        ScreenUsage15min[now15min] = count15min.Where(kvp => kvp.Value == count15min.Max(kvp => kvp.Value)).First().Key;
                        count15min.Clear();
                    }
                        
                    now15min = winlog[i].t / 900;
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
                else 
                {
                    if (count30min.Count > 0)
                    {
                        ScreenUsage30min[now30min] = count30min.Where(kvp => kvp.Value == count30min.Max(kvp => kvp.Value)).First().Key;
                        count30min.Clear();
                    }
                        
                    now30min = winlog[i].t / 1800;
                    count30min.Add(winlog[i].s, 1);
                }

                if (winlog[i].t / 3600 == now1h)
                {
                    if (count1h.ContainsKey(winlog[i].s))
                    {
                        count1h[winlog[i].s] += 1;
                    }
                    else count1h.Add(winlog[i].s, 1);
                }
                else
                {
                    if (count1h.Count > 0)
                    {
                        ScreenUsage1h[now1h] = count1h.Where(kvp => kvp.Value == count1h.Max(kvp => kvp.Value)).First().Key;
                        count1h.Clear();
                    }
                    now1h = winlog[i].t / 3600;
                    count1h.Add(winlog[i].s, 1);
                }
            }
            ScreenUsage1min[now1min] = count1min.Where(kvp => kvp.Value == count1min.Max(kvp => kvp.Value)).First().Key;
            ScreenUsage5min[now5min] = count5min.Where(kvp => kvp.Value == count5min.Max(kvp => kvp.Value)).First().Key;
            ScreenUsage15min[now15min] = count15min.Where(kvp => kvp.Value == count15min.Max(kvp => kvp.Value)).First().Key;
            ScreenUsage30min[now30min] = count30min.Where(kvp => kvp.Value == count30min.Max(kvp => kvp.Value)).First().Key;
            ScreenUsage1h[now1h] = count1h.Where(kvp => kvp.Value == count1h.Max(kvp => kvp.Value)).First().Key;
            return true;
        }



        //绘制时间线的函数
        public void drawTimeline()
        {

            //绘制计划的时间线，暂时先用随机数据。
            planGrid.Children.Clear();
            planGrid.RowDefinitions.Clear();
            /*
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
            */
            //绘制实际的时间线。采用winlog中的数据。
            actualGrid.Children.Clear();
            actualGrid.RowDefinitions.Clear();
            //只能显示出从startsecond到endsecond的部分，而且只能显示某一天。（按说应该是会给一个选择日期的按钮的）
            //小时模式：时间跨度大于8小时。
            //一屏放14个左右的块是舒服的
            string[] usingstring = ScreenUsage1h;
            int interval = 3600;

            if (endsecond - startsecond < 1800*14)
            {
                usingstring = ScreenUsage30min;
                interval = 1800;
            }
            if (endsecond - startsecond < 900*14)
            {
                usingstring = ScreenUsage15min;
                interval = 900;
            }
            if (endsecond - startsecond < 300*14)
            {
                usingstring = ScreenUsage5min;
                interval = 300;
            }
            if (endsecond - startsecond < 60*14)
            {
                usingstring = ScreenUsage1min;
                interval = 60;
            }


            int realcount = 0;
            for (int i = (int)(startsecond/ interval); i <=(int)(endsecond/ interval);i++)
            {
                if ((endsecond / interval) == i) break;//这一行可以解决24:00导致数组越界的bug！

                double height = interval;
                if (i == (int)(startsecond / interval)) height += (i * interval - startsecond);
                if (i == (int)(endsecond / interval)) height -= (i * interval - endsecond + interval);
                
                string nowstring = usingstring[i];


                
                while ((usingstring[i + 1] == nowstring) && (i < (int)(endsecond / interval)))
                {
                    i++;
                    if ((endsecond / interval) == i) break;//这一行也要加，用来解决24:00导致数组越界的bug！
                    height += interval;
                    if (i == (int)(startsecond / interval)) height += (i * interval - startsecond);
                    if (i == (int)(endsecond / interval)) height -= (i * interval - endsecond + interval);

                }

                

                //如果和下面的相同，就一起做了！
                actualGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(height, GridUnitType.Star) });
                if (nowstring == null)
                {
                    realcount++;
                    continue;
                }

                Button b = new Button();
                actualGrid.Children.Add(b);
                b.SetValue(Grid.RowProperty,realcount++);
                b.Margin = new Thickness(3, 2, 5, 2);
                b.Height = double.NaN;
                b.Content = nowstring;
                SolidColorBrush c = new SolidColorBrush(Color.FromArgb(200, (byte)nowstring.GetHashCode(), (byte)(nowstring.GetHashCode()/256), 255));
                b.Background = c;
                b.BorderBrush = c;
                


            }
            
            drawTime();
        }

        public void drawTime()
        {
            timeGrid.Children.Clear();

            int interval = 60 * 60 * 4;
            int mod = 1;
            int div = 1;
            int multi = 0;
            if (endsecond - startsecond < 12 * 60 * 60)
            {
                interval = 60*60;
                mod = 1;
                div = 4;
                multi = 0;
            }
            if (endsecond - startsecond < 3 * 60 * 60)
            {
                interval = 60 * 15;
                mod = 4;
                div = 16;
                multi = 15;
            }
            if (endsecond - startsecond < 45 * 60)
            {
                interval = 60 * 5;
                mod = 12;
                div = 48;
                multi = 5;
            }
            if (endsecond - startsecond < 10 * 60)
            {
                interval = 60;
                mod = 60;
                div = 240;
                multi = 1;
            }


            for (int drawtime = (int)startsecond / interval; drawtime <= endsecond / interval; drawtime++)
            {

                double margin = timeGrid.ActualHeight * (drawtime * interval - startsecond) / (endsecond - startsecond) - 20;
                if (margin > -30)
                {
                    TextBlock a = new TextBlock();
                    a.Text = (drawtime * 4 / div).ToString("D2") + ":" + (drawtime % mod * multi).ToString("D2");
                    a.HorizontalAlignment = HorizontalAlignment.Right;
                    a.Margin = new Thickness(0, margin, 0, 0);
                    timeGrid.Children.Add(a);

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
                if (e.Delta < 0)
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
