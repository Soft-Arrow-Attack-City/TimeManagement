using System.Windows;

namespace TimeManagement
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //MySchedule.saveAllSchedule();
        }
    }
}