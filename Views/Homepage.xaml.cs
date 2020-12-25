using System.Windows;
using System.Windows.Controls;
using TimeManagement.Utilities;

namespace TimeManagement.Views
{
    /// <summary>
    /// Homepage.xaml 的交互逻辑
    /// </summary>
    public partial class Homepage : UserControl
    {
        public Homepage()
        {
            InitializeComponent();
        }

        private void GitHubButton_Click(object sender, RoutedEventArgs e) =>
            Link.OpenInBrowser("https://github.com/Soft-Arrow-Attack-City/TimeManagement");

        private void MaterialFrameworkButton_Click(object sender, RoutedEventArgs e) =>
            Link.OpenInBrowser("http://materialdesigninxaml.net/");
    }
}