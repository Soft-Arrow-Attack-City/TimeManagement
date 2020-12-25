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
        public bool usePriority = false;
        private bool TodayToStart = false;
        private string[] chongfu = new string[]{"不重复", "每天", "每周", "每月", "每年"};
        private string[] tixing = new string[] { "不提醒", "准时提醒", "提前5分钟", "提前10分钟", "提前半小时" };
        private string[] youxianji= new string[] { "最低", "较低", "一般", "较高", "最高" };


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


        private void drawTaskCards()
        {
            TaskShowerPanel.Children.Clear();
            TaskShowerPanel.Orientation = Orientation.Vertical;

            DateTime today = DateTime.Now.Date;
            List<Guid> guids1;
            if (usePriority)
            {
                guids1 = MyTask.getActiveTasksByPriority();
            }
            else
            {
                guids1 = MyTask.getActiveTasksByDue();
            }
            
            foreach (Guid id in guids1)
            {
                MyTask task = MyTask.getActiveTask(id);

                Expander ex = new Expander();
                ex.Margin = new Thickness(3, 5, 3, 5);
                DockPanel dp1 = new DockPanel();
                TextBlock tb1 = new TextBlock();
                tb1.Text = task.Title;
                TextBlock tb2 = new TextBlock();

                int days = (int)((task.Due-today).TotalDays);
                if (days <= 1)
                {
                    tb2.Text = $"{days} day left";
                    tb2.Foreground = Brushes.Red;
                }
                else if (days <= 3)
                {
                    tb2.Text = $"{days} days left";
                    tb2.Foreground = Brushes.Orange;
                }
                else if (days <= 7)
                {
                    tb2.Text = $"{days} days left";
                    tb2.Foreground = Brushes.BlueViolet;
                }
                else
                {
                    tb2.Text = $"{days} days left";
                    tb2.Foreground = Brushes.DeepSkyBlue;
                }
                tb2.HorizontalAlignment = HorizontalAlignment.Right;
                dp1.Children.Add(tb1);
                dp1.Children.Add(tb2);
                ex.Header = dp1;
                DockPanel dp2 = new DockPanel();
                TextBlock tb3 = new TextBlock();
                tb3.Text = $"Due: {task.Due.ToString("yyyy-MM-dd")}\n优先级：{youxianji[task.Priority]}";
                tb3.Margin = new Thickness(24, 8, 8, 16);
                dp2.Children.Add(tb3);
                //对于没过ddl的任务，未完成的任务有“标记为完成”和“删除”，已完成的任务有“标记为未完成”和“删除”，删除的任务就没了
                //对于已经过ddl的任务，只能删除，或者保留7天后自动清除。
                DockPanel dp3 = new DockPanel();
                dp3.Width = 80;
                dp3.HorizontalAlignment = HorizontalAlignment.Right;
                dp3.Margin = new Thickness(24, 8, 8, 16);

                Button bt1 = new Button();
                bt1.Height = 30;
                bt1.Width = 30;
                bt1.Style = refButton.Style;
                MaterialDesignThemes.Wpf.PackIcon pi1 = new MaterialDesignThemes.Wpf.PackIcon();
                pi1.Kind = MaterialDesignThemes.Wpf.PackIconKind.Check;
                pi1.Height = 24;
                pi1.Width = 24;
                bt1.Content = pi1;
                bt1.ToolTip = "标记为已完成";
                bt1.Tag = id;
                bt1.Click += FinishTask_Click;

                Button bt2 = new Button();
                bt2.Height = 30;
                bt2.Width = 30;
                bt2.Style = refButton.Style;
                MaterialDesignThemes.Wpf.PackIcon pi2 = new MaterialDesignThemes.Wpf.PackIcon();
                pi2.Kind = MaterialDesignThemes.Wpf.PackIconKind.TrashCanOutline;
                pi2.Height = 24;
                pi2.Width = 24;
                bt2.Content = pi2;
                bt2.ToolTip = "删除此任务";
                bt2.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                bt2.BorderBrush = Brushes.BlueViolet;
                bt2.Foreground = Brushes.Red;
                bt2.Tag = id;
                bt2.Click += DeleteTask_Click;

                dp3.Children.Add(bt1);
                dp3.Children.Add(bt2);

                dp2.Children.Add(dp3);
                ex.Content = dp2;
                TaskShowerPanel.Children.Add(ex);
            }

            TextBox pp1 = new TextBox();
            MaterialDesignThemes.Wpf.HintAssist.SetHint(pp1, "已完成的任务");
            pp1.IsEnabled = false;
            TaskShowerPanel.Children.Add(pp1);

            List<Guid> guids2 = MyTask.getFinishedTasks();
            foreach (Guid id in guids2)
            {
                MyTask task = MyTask.getFinishedTask(id);

                Expander ex = new Expander();
                ex.Margin = new Thickness(3, 5, 3, 5);
                DockPanel dp1 = new DockPanel();
                TextBlock tb1 = new TextBlock();
                tb1.Text = task.Title;
                TextBlock tb2 = new TextBlock();

                int days = (int)((task.Due - today).TotalDays);
                if (days <= 1)
                {
                    tb2.Text = $"{days} day left";
                }
                else
                {
                    tb2.Text = $"{days} days left";
                }
                tb2.Foreground = Brushes.Green;
                tb2.HorizontalAlignment = HorizontalAlignment.Right;
                dp1.Children.Add(tb1);
                dp1.Children.Add(tb2);
                ex.Header = dp1;
                DockPanel dp2 = new DockPanel();
                TextBlock tb3 = new TextBlock();
                tb3.Text = $"Due: {task.Due.ToString("yyyy-MM-dd")}\n优先级：{youxianji[task.Priority]}";
                tb3.Margin = new Thickness(24, 8, 8, 16);
                dp2.Children.Add(tb3);
                //对于没过ddl的任务，未完成的任务有“标记为完成”和“删除”，已完成的任务有“标记为未完成”和“删除”，删除的任务就没了
                //对于已经过ddl的任务，只能删除，或者保留7天后自动清除。
                DockPanel dp3 = new DockPanel();
                dp3.Width = 80;
                dp3.HorizontalAlignment = HorizontalAlignment.Right;
                dp3.Margin = new Thickness(24, 8, 8, 16);

                Button bt1 = new Button();
                bt1.Height = 30;
                bt1.Width = 30;
                bt1.Style = refButton.Style;
                MaterialDesignThemes.Wpf.PackIcon pi1 = new MaterialDesignThemes.Wpf.PackIcon();
                pi1.Kind = MaterialDesignThemes.Wpf.PackIconKind.UndoVariant;
                pi1.Height = 24;
                pi1.Width = 24;
                bt1.Content = pi1;
                bt1.ToolTip = "标记为未完成";
                bt1.Tag = id;
                bt1.Click += UnfinishTask_Click;

                Button bt2 = new Button();
                bt2.Height = 30;
                bt2.Width = 30;
                bt2.Style = refButton.Style;
                MaterialDesignThemes.Wpf.PackIcon pi2 = new MaterialDesignThemes.Wpf.PackIcon();
                pi2.Kind = MaterialDesignThemes.Wpf.PackIconKind.TrashCanOutline;
                pi2.Height = 24;
                pi2.Width = 24;
                bt2.Content = pi2;
                bt2.ToolTip = "删除此任务";
                bt2.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                bt2.BorderBrush = Brushes.BlueViolet;
                bt2.Foreground = Brushes.Red;
                bt2.Tag = id;
                bt2.Click += DeleteTask_Click;

                dp3.Children.Add(bt1);
                dp3.Children.Add(bt2);

                dp2.Children.Add(dp3);
                ex.Content = dp2;
                TaskShowerPanel.Children.Add(ex);
            }


            TextBox pp2 = new TextBox();
            MaterialDesignThemes.Wpf.HintAssist.SetHint(pp2, "已过期的任务");
            pp2.IsEnabled = false;
            TaskShowerPanel.Children.Add(pp2);


            List<Guid> guids3 = MyTask.getOveredTasks();
            foreach (Guid id in guids3)
            {
                MyTask task = MyTask.getOveredTask(id);

                Expander ex = new Expander();
                ex.Margin = new Thickness(3, 5, 3, 5);
                DockPanel dp1 = new DockPanel();
                TextBlock tb1 = new TextBlock();
                tb1.Text = task.Title;
                TextBlock tb2 = new TextBlock();

                int days = (int)((today - task.Due).TotalDays);
                if (days <= 1)
                {
                    tb2.Text = $"{days} day ago";
                }
                else
                {
                    tb2.Text = $"{days} days ago";
                }
                tb2.Foreground = Brushes.Gray;
                tb2.HorizontalAlignment = HorizontalAlignment.Right;
                dp1.Children.Add(tb1);
                dp1.Children.Add(tb2);
                ex.Header = dp1;
                DockPanel dp2 = new DockPanel();
                TextBlock tb3 = new TextBlock();
                tb3.Text = $"Due: {task.Due.ToString("yyyy-MM-dd")}\n优先级：{youxianji[task.Priority]}";
                tb3.Margin = new Thickness(24, 8, 8, 16);
                dp2.Children.Add(tb3);
                //对于没过ddl的任务，未完成的任务有“标记为完成”和“删除”，已完成的任务有“标记为未完成”和“删除”，删除的任务就没了
                //对于已经过ddl的任务，只能删除，或者保留7天后自动清除。
                DockPanel dp3 = new DockPanel();
                dp3.Width = 80;
                dp3.HorizontalAlignment = HorizontalAlignment.Right;
                dp3.Margin = new Thickness(24, 8, 8, 16);

                Button bt1 = new Button();
                bt1.Height = 30;
                bt1.Width = 30;
                bt1.Style = refButton.Style;
                MaterialDesignThemes.Wpf.PackIcon pi1 = new MaterialDesignThemes.Wpf.PackIcon();
                pi1.Kind = MaterialDesignThemes.Wpf.PackIconKind.UndoVariant;
                pi1.Height = 24;
                pi1.Width = 24;
                bt1.Content = pi1;
                bt1.Opacity = 0;

                Button bt2 = new Button();
                bt2.Height = 30;
                bt2.Width = 30;
                bt2.Style = refButton.Style;
                MaterialDesignThemes.Wpf.PackIcon pi2 = new MaterialDesignThemes.Wpf.PackIcon();
                pi2.Kind = MaterialDesignThemes.Wpf.PackIconKind.TrashCanOutline;
                pi2.Height = 24;
                pi2.Width = 24;
                bt2.Content = pi2;
                bt2.ToolTip = "删除此任务";
                bt2.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                bt2.BorderBrush = Brushes.BlueViolet;
                bt2.Foreground = Brushes.Red;
                bt2.Tag = id;
                bt2.Click += DeleteTask_Click;

                dp3.Children.Add(bt1);
                dp3.Children.Add(bt2);

                dp2.Children.Add(dp3);
                ex.Content = dp2;
                TaskShowerPanel.Children.Add(ex);
            }
        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MySchedule.loadAllSchedule();
            MyTask.loadAllTasks();
            drawScheduleCards();
            drawTaskCards();
        }

        private void CreateTask_Click(object sender, RoutedEventArgs e)
        {
            string taskName = TaskNameBox.Text;
            int prior = TaskPriorBar.Value - 1;

            if (taskName.Length == 0)
            {
                MessageBox.Show("请输入任务名称！");
                return;
            }

            if (TaskDueDateBox.SelectedDate == null)
            {
                MessageBox.Show("请选择截止日期！");
                return;
            }

            DateTime duedate = TaskDueDateBox.SelectedDate.Value;
            
            MyTask.AddTask(new MyTask { Title = taskName, Priority = prior, Due = duedate });

            NewTaskExpander.IsExpanded = false;

            TaskNameBox.Text = "";
            TaskPriorBar.Value = 3;
            TaskDueDateBox.SelectedDate = null;
            drawTaskCards();
        }

        private void CreateTaskCancle_Click(object sender, RoutedEventArgs e)
        {
            NewTaskExpander.IsExpanded = false;
        }


        private void FinishTask_Click(object sender, RoutedEventArgs e)
        {
            MyTask.FinishTask((Guid)((Button)sender).Tag);
            drawTaskCards();
        }

        private void UnfinishTask_Click(object sender, RoutedEventArgs e)
        {
            MyTask.UnfinishTask((Guid)((Button)sender).Tag);
            drawTaskCards();
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            MyTask.DeleteTask((Guid)((Button)sender).Tag);
            drawTaskCards();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            usePriority = ((System.Windows.Controls.Primitives.ToggleButton)sender).IsChecked.Value;
            drawTaskCards();
        }
    }
}
