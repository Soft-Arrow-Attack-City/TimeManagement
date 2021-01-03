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
using FlaUI.Core.Definitions;

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
                window.FindFirstDescendant("PopupBox").Click();
                Thread.Sleep(500);
                var ExitButton = window.FindFirstDescendant("ExitButton").AsButton();
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
                // 共8位同学，测试能正常渲染
                // 下方项目链接不再重复测试
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
                var tabs = window.FindFirstDescendant("MainZoneTabablz");
                // 此处必须用ByText来获取Tab的Header
                var scheduleTab = tabs.FindFirstDescendant(cf => cf.ByText("日程管理"));
                scheduleTab.Click();
                Thread.Sleep(500);
                var expander = window.FindFirstDescendant("NewTaskExpander");
                var ecp = expander.Patterns.ExpandCollapse.Pattern;
                ecp.Expand();
                Thread.Sleep(500);
                var namebox = window.FindFirstDescendant("TaskNameBox").AsTextBox();
                // 测试文本框能输入内容
                Assert.True(namebox.Patterns.Text.IsSupported);
                var s = $"测试任务{DateTime.Now}";
                namebox.Text = s;
                window.FindFirstDescendant("TaskDueDateBox").AsDateTimePicker().SelectedDate = DateTime.Now.AddMinutes(5);
                Thread.Sleep(500);
                expander.FindFirstDescendant(cf => cf.ByText("创建 !")).AsButton().Click();
                Thread.Sleep(500);
                // 测试创建任务后，新任务创建窗口被折叠
                Assert.Equal(ExpandCollapseState.Collapsed, ecp.ExpandCollapseState.Value);
                var newTask = tabs.FindFirstDescendant(cf => cf.ByText(s));
                // 测试新建的任务已存在
                Assert.NotNull(newTask);
            }
        }
    }
}
