using Xunit;

namespace TimeManagement.Tests
{
    public class MainWindowTests
    {
        [StaFact]
        public void MainWindowTitle()
        {
            MainWindow mainWindow = new MainWindow();
            Assert.Equal("MainWindow", mainWindow.Title);
        }
    }
}