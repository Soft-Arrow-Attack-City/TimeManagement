﻿using MaterialDesignThemes.Wpf;
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

        //窗口整体初始化完成后，执行绘制左侧边栏的代码。
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            drawTimeline();
        }      
        


        //假设没有后端数据的时候，先生成一些随机的数据作为后端。
        //后端数据的生成方式：时间点+任务。
        //直接用均分的方法，均分段里面有哪个记录多就直接记录为哪个。如果没有记录就不透明度为零（全透明），如果上下两块东西一样就合并。
        public void generatedata()
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
                b.Height = double.NaN;
                SolidColorBrush c = new SolidColorBrush(Color.FromArgb(200, (byte)(random.NextDouble() * 256), (byte)(random.NextDouble() * 256), 255));
                b.Background = c;
                b.BorderBrush = c;
                b.Content = "actual" + i.ToString();

            }
        }       
    }
}
