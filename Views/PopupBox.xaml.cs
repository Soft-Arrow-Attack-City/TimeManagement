using MaterialDesignThemes.Wpf;
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


            //new MainWindow().Show();
            //Application.Current.MainWindow.ShowInTaskbar = false;
            //Application.Current.MainWindow.Visibility = Visibility.Hidden;
            //MainWindow ww = (MainWindow)Application.Current.MainWindow;
            //MainWindow w = new MainWindow();
            //w.Show();
            //Application.Current.MainWindow = w;
            //ww.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("清除缓存成功");
            
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("你没有获得任何成就！");
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("小组成员：赵紫延 赵祺铭 崔晏菲 白昊昕 祝子涵 于祎男 王浩威 王筱淳");
        }
    }
}
