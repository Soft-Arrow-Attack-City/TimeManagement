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


        public Timeline()
        {
            InitializeComponent();
        }


        //假设没有后端数据的时候，先生成一些随机的数据作为后端。
        //后端数据的生成方式：时间点+任务。
        //直接用均分的方法，均分段里面有哪个记录多就直接记录为哪个。如果没有记录就不透明度为零（全透明），如果上下两块东西一样就合并。
        public void generatedata()
        {

        }

        //绘制时间线的函数
        public void drawTimeline()
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

            //绘制实际的时间线。（需要用采集到的信息来绘制，要有足够长时间。现在是以秒为单位的数据，但将来可能是以小时为单位的数据。）
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
            drawTimeline();
        }

    }
}
