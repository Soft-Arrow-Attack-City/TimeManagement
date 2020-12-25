using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TimeManagement.ViewModel;
using FluentScheduler;
using TimeManagement.DataModel;
using Task = System.Threading.Tasks.Task;
using System.Threading;

namespace TimeManagement.Views
{
    /// <summary>
    /// VirtualTreePlanting.xaml 的交互逻辑
    /// </summary>
    public partial class VirtualTreePlanting : UserControl
    {

        public VirtualTreePlanting()
        {
            InitializeComponent();
            DataContext = ViewModel;

        }

        private VirtualTreePlantingViewModel ViewModel { get; } = new VirtualTreePlantingViewModel();

        private void Blacklist_Click(object sender, RoutedEventArgs e)
        {
            BlacklistSearchText.Text = "Refreshing...";
            BlacklistSearchText.Text = "";
            foreach (string p in ViewModel.Selected)
                BlacklistBox.SelectedItems.Add(p);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string text = BlacklistSearchText.Text;
            if ((text.Length > 0) && !BlacklistBox.SelectedItems.Contains(text))
            {
                if (!BlacklistBox.Items.Contains(text))
                {
                    ViewModel.Processes.Add(text);
                    ViewModel.ListBoxContent.Add(text);
                }
                BlacklistBox.SelectedItems.Add(text);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e) => BlacklistBox.SelectedItems.Clear();

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Selected.Clear();
            ViewModel.Selected.UnionWith(BlacklistBox.SelectedItems.Cast<string>().ToList());
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PlantStart(new TreeSession
            {
                Duration = TimeSpan.FromMinutes(TimeSlider.Value),
                Title = TaskNameText.Text,
                Type = TaskProperties.SelectedItem?.ToString() ?? ""
            });
            PlantTitle.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }

        private void TreeFlipper_IsFlippedChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!e.NewValue)
            {
                Task.Factory.StartNew(() => Thread.Sleep(1000)).ContinueWith(t =>
                {
                    MainWindowViewModel.MainSnackbarMessageQueue?.Enqueue(ViewModel.PlantSuccess ? "种树成功！" : "种树失败！");
                }, TaskScheduler.Current);
                TreeImg.Source = new BitmapImage(
                    new Uri(ViewModel.PlantSuccess ?
                    "pack://application:,,,/Resources/Images/TreeSuccess.png" :
                    "pack://application:,,,/Resources/Images/TreeFailed.png"
                    ));
            }
        }
    }
}
