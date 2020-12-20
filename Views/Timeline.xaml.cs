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
        private SortedSet<DataModel.Alog> winlog=null;
        private double startsecond, endsecond;




        public Timeline()
        {
            InitializeComponent();
            //获取真的或者假的屏幕使用记录数据，要排好序的。
            winlog = DataModel.TimelineData.generatedata();
            startsecond = 28800;
            endsecond = 86400;

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
            for (int i = 0; i < 10; i++)
            {
                actualGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(random.NextDouble(), GridUnitType.Star) });
                Button b = new Button();
                actualGrid.Children.Add(b);
                b.SetValue(Grid.RowProperty, i);
                b.Margin = new Thickness(3, 2, 5, 2);
                b.Height = double.NaN;
                SolidColorBrush c = new SolidColorBrush(Color.FromArgb(200, (byte)(random.NextDouble() * 256), (byte)(random.NextDouble() * 256), 255));
                b.Background = c;
                b.BorderBrush = c;
                b.Content = "actual" + i.ToString();

            }
        }

        private void drawTime()
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


                drawTime();
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

            drawTime();
        }

    }
}
