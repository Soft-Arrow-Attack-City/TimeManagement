using MaterialDesignThemes.Wpf;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TimeManagement.DataModel;
using TimeManagement.ViewModel;

namespace TimeManagement.Views
{
    /// <summary>
    /// PopupBox.xaml 的交互逻辑
    /// </summary>
    public partial class PopupBox : UserControl
    {
        public PopupBox()
        {
            InitializeComponent();

            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            DarkModeToggleButton.IsChecked = theme.GetBaseTheme() == BaseTheme.Dark;

            if (paletteHelper.GetThemeManager() is { } themeManager)
            {
                themeManager.ThemeChanged += (_, e)
                    => DarkModeToggleButton.IsChecked = e.NewTheme?.GetBaseTheme() == BaseTheme.Dark;
            }
        }

        private void MenuDarkModeButton_Click(object sender, RoutedEventArgs e)
            => ModifyTheme(DarkModeToggleButton.IsChecked == true);

        private static void ModifyTheme(bool isDarkTheme)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(isDarkTheme ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MySchedule.removeAllSchedule();
            MyTask.clearAllTasks();
            TreeSession.clearAllTrees();
            Task.Factory.StartNew(() => Thread.Sleep(500)).ContinueWith(t =>
            {
                MainWindowViewModel.MainSnackbarMessageQueue?.Enqueue("清除缓存成功");
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}