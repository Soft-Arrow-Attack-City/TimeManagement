using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FlaUI.UIA3;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using System.Windows.Controls;
using System.Threading;

namespace TimeManagement.Tests
{
    public class UITest : IDisposable
    {
        static readonly string path = @"..\..\..\obj\Debug\TimeManagement.exe";
        private Application app;

        public UITest()
        {
            app = Application.Launch(path);
        }

        public void Dispose()
        {
            if (!app.HasExited)
                app.Close();
        }

        // 测试App能正常启动，显示主窗口及标题
        [Fact]
        public void LaunchTest()
        {
            using (var automation = new UIA3Automation())
            {
                var window = app.GetMainWindow(automation);
                Assert.NotNull(window);
                Assert.NotNull(window.Title);
            }
        }

        // 测试退出按钮能正常使用
        [Fact]
        public void ExitTest()
        {
            using (var automation = new UIA3Automation())
            {
                var window = app.GetMainWindow(automation);
                var PopupBox = window.FindFirstDescendant("PopupBox");
                PopupBox.Click();
                Thread.Sleep(500);
                var ExitButton = window.FindFirstDescendant("ExitButton");
                ExitButton.Click();
                Assert.True(app.HasExited);
            }
        }

        // 测试左边栏名单能正常显示
        [Fact]
        public void TreeViewTest()
        {
            using (var automation = new UIA3Automation())
            {
                var window = app.GetMainWindow(automation);
                var DevelopersTreeView = window.FindFirstDescendant("Developers").AsTreeItem();
                Assert.Equal(8, DevelopersTreeView.Items.Count());
            }
        }

        // 测试日程功能
        [Fact]
        public void ScheduleTest()
        {
            using (var automation = new UIA3Automation())
            {
                var window = app.GetMainWindow(automation);

            }
        }

        // 测试种树功能
        [Fact]
        public void PlantingTest()
        {
            using (var automation = new UIA3Automation())
            {
                var window = app.GetMainWindow(automation);
            }
        }
    }
}
