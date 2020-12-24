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

using TimeManagement.DataModel;

namespace TimeManagement.Views
{
    /// <summary>
    /// schedulepage.xaml 的交互逻辑
    /// </summary>
    public partial class schedulepage : UserControl
    {
        private bool TodayToStart = false;
        private string[] chongfu = new string[]{"不重复", "每天", "每周", "每月", "每年"};
        private string[] tixing = new string[] { "不提醒", "准时提醒", "提前5分钟", "提前10分钟", "提前半小时" };


        public schedulepage()
        {
            InitializeComponent();
        }

        private void WhentoStartButton_Click(object sender, RoutedEventArgs e)
        {
            TodayToStart = !TodayToStart;
            if (TodayToStart) WhentoStartButton.Content = "从今天开始";
            else WhentoStartButton.Content = "从选定日期开始";
        }

        private void CreateScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            string title = ScheduleNameInputBox.Text;
            DateTime? starttime = ScheduleStartTimerPicker.SelectedTime;
            DateTime? endtime = ScheduleEndTimerPicker.SelectedTime;
            int repeat = ScheduleRepeatBox.SelectedIndex;
            int remind = ScheduleRemindBox.SelectedIndex;

            if (title.Length == 0)
            {
                MessageBox.Show("请输入日程标题！");
                return;
            }
            if (starttime==null)
            {
                MessageBox.Show("请选择日程开始时间！");
                return;
            }
            if (endtime == null)
            {
                MessageBox.Show("请选择日程结束时间！");
                return;
            }
            if (remind == -1)
            {
                MessageBox.Show("请选择提醒方式！");
                return;
            }
            if (repeat == -1)
            {
                MessageBox.Show("请选择重复周期！");
                return;
            }

            if (starttime >= endtime)
            {
                MessageBox.Show("结束时间必须在开始时间之后！");
                return;
            }

            DateTime startday = DateTime.Now.Date;
            if (!TodayToStart)
            {
                if (ScheduleCalendar.SelectedDate == null)
                {
                    MessageBox.Show("请选择一个开始日期！");
                    return;
                }
                startday = ScheduleCalendar.SelectedDate.Value;
            }

            MySchedule.AddSchedule(new MySchedule
            {
                Title = title,
                Start = startday.Date + starttime.Value.TimeOfDay,
                Duration = endtime.Value - starttime.Value,
                Repeat = (Freq)repeat,
                remindMode=(RemindMode)remind
            });
            ScheduleNameInputBox.Text = null;
            ScheduleStartTimerPicker.SelectedTime = null;
            ScheduleEndTimerPicker.SelectedTime = null;
            ScheduleRepeatBox.SelectedIndex = -1;
            ScheduleRemindBox.SelectedIndex = -1;
            ScheduleFlipper.IsFlipped = false;
            drawScheduleCards();

        }

        private void CancelCreateScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleNameInputBox.Text = null;
            ScheduleStartTimerPicker.SelectedTime = null;
            ScheduleEndTimerPicker.SelectedTime = null;
            ScheduleRepeatBox.SelectedIndex = -1;
            ScheduleRemindBox.SelectedIndex = -1;
            ScheduleFlipper.IsFlipped = false;
        }


        private void drawScheduleCards()
        {
            //标题上只要一个题目和一个大致的开始时间。
            //展开后，里面的字符串为：
            //开始时间：
            //结束时间：
            //重复周期：
            //提醒：未设置之类的

            //按钮：本次提前结束
            //按钮：永久结束
            //这些按钮里面应该对应着GUID的，所以就能取消到。
            ScheduleShowerPanel.Orientation= Orientation.Vertical;
            ScheduleShowerPanel.Children.Clear();
            

            List<Guid> guids = MySchedule.getAllActiveSchedules();
            foreach (Guid guid in guids)
            {
                MySchedule sched = MySchedule.getActiveSchedule(guid);


                Expander expd = new Expander();
                DockPanel header = new DockPanel();
                TextBlock tb1 = new TextBlock();
                tb1.Text = sched.Title;
                TextBlock tb2 = new TextBlock();
                tb2.Text = sched.Start.ToString("yyyy-MM-dd");
                tb2.HorizontalAlignment = HorizontalAlignment.Right;
                header.Children.Add(tb1);
                header.Children.Add(tb2);
                expd.Header = header;


                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Vertical;


                TextBlock tb3 = new TextBlock();
                tb3.Text = "开始时间："+ sched.Start.ToString("yyyy-MM-dd HH:mm:ss");
                tb3.Margin = new Thickness(24, 0, 24, 0);
                sp.Children.Add(tb3);

                TextBlock tb4 = new TextBlock();
                tb4.Text = "结束时间：" + (sched.Start+sched.Duration).ToString("yyyy-MM-dd HH:mm:ss");
                tb4.Margin = new Thickness(24, 0, 24, 0);
                sp.Children.Add(tb4);

                TextBlock tb5 = new TextBlock();
                tb5.Text = "重复："+chongfu[(int)sched.Repeat];
                tb5.Margin = new Thickness(24, 0, 24, 0);
                sp.Children.Add(tb5);

                TextBlock tb6 = new TextBlock();
                tb6.Text = "提醒模式：" + tixing[(int)sched.remindMode];
                tb6.Margin = new Thickness(24, 0, 24, 8);
                sp.Children.Add(tb6);

                Grid lsp = new Grid();
                lsp.ColumnDefinitions.Add(new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star)});
                lsp.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                Button bt = new Button();
                bt.Tag = guid;
                bt.Click += CancelOnce_Click;
                bt.Content = "本次提前结束";//一个结束本次，一个永久结束，对于可以重复的schedule来说，这两个是不一样的。
                lsp.Children.Add(bt);
                bt.SetValue(Grid.ColumnProperty, 0);
                Button bt2 = new Button();
                bt2.Tag = guid;
                bt2.Click += CancelAll_Click;
                bt2.Content = "永久取消日程";//一个结束本次，一个永久结束，对于可以重复的schedule来说，这两个是不一样的。
                lsp.Children.Add(bt2);
                bt2.SetValue(Grid.ColumnProperty, 1);
                sp.Children.Add(lsp);

                expd.Content = sp;

                MaterialDesignThemes.Wpf.ColorZone cz = new MaterialDesignThemes.Wpf.ColorZone();
                cz.Margin = new Thickness(3, 5, 3, 5);
                cz.Content = expd;

                ScheduleShowerPanel.Children.Add(cz);
            }
        }

        private void CancelOnce_Click(object sender, RoutedEventArgs e)
        {
            MySchedule.removeScheduleOnce((Guid)(((Button)sender).Tag));
            drawScheduleCards();
        }

        private void CancelAll_Click(object sender, RoutedEventArgs e)
        {
            MySchedule.removeScheduleAll((Guid)(((Button)sender).Tag));
            drawScheduleCards();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            drawScheduleCards();
        }
    }
}
