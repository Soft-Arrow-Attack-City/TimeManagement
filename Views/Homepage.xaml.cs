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
