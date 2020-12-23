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
using System.Diagnostics;
using System.ComponentModel;

namespace TimeManagement.Views
{
    /// <summary>
    /// VirtualTreePlanting.xaml 的交互逻辑
    /// </summary>
    public partial class VirtualTreePlanting : UserControl
    {
        public static TreeSession tree;
        public static HashSet<string> processes;
        public VirtualTreePlanting()
        {
            InitializeComponent();
        }

        private void Blacklist_Click(object sender, RoutedEventArgs e)
        {
            Process[] localAll = Process.GetProcesses();
            string[] processNames = localAll.Select(p => p.ProcessName).ToArray();
            processes = new HashSet<string>(processNames);
            foreach (string p in processes)
                BlacklistBox.Items.Add(p);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = blackListSearchText.Text;
            BlacklistBox.Items.Clear();
            if (s.Length > 0)
            {
                foreach (string p in processes)
                    if (p.StartsWith(blackListSearchText.Text))
                        BlacklistBox.Items.Add(p);
            }
            else
                foreach (string p in processes)
                    BlacklistBox.Items.Add(p);
            BlacklistBox.UpdateLayout();
        }
    }
}
