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
        private Random random = new Random();
        private SortedSet<DataModel.Alog> winlog=null;
        private int startsecond, endsecond;




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

        private void TimelineGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            //MessageBox.Show(e.Delta.ToString());
            //所有刻度都出现在该出现的位置。
            timeGrid.Children.Clear();


            //4小时刻度出现：0，4，8，12，16，20，24。（一直都在）
            int drawtime = 0;
            


            TextBlock a = new TextBlock();
            a.Text = "asdf";
            a.HorizontalAlignment = HorizontalAlignment.Right;
            a.Margin = new Thickness(0,350,0,0);

            timeGrid.Children.Add(a);
            if (endsecond - startsecond <= 8 * 60 * 60)
            {

            }




            //1小时刻度出现：当间隔小于8小时

            //15分钟刻度出现：当间隔小于2小时

            //5分钟刻度出现：当间隔小于30分钟

            //1分钟刻度出现：当间隔小于10分钟

            //drawTimeline();
        }

    }
}
