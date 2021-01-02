using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FlaUI.UIA3;
using FlaUI.Core;

namespace TimeManagement.Tests
{
    public class UITest : IDisposable
    {
        static readonly string path = "..\\..\\..\\obj\\Debug\\TimeManagement.exe";
        private Application app;

        public UITest()
        {
            app = Application.Launch(path);
        }

        public void Dispose()
        {
            app.WaitWhileMainHandleIsMissing();
            app.Close();
        }

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
    }
}
