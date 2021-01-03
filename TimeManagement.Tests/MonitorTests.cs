using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Threading;

namespace TimeManagement.Tests
{
    public class MonitorTests
    {
        [Fact]
        public void MonitorPercentage()
        {
            Dictionary<string, double> WindowProportion = new Dictionary<string, double>();
            Dictionary<string, double> ProgramProportion = new Dictionary<string, double>();
            // 10次获取屏幕窗口比例
            for (int i = 0; i < 10; i++)
            {
                Utilities.Monitor.GetAllWindowProportion(ref WindowProportion, ref ProgramProportion);
                Thread.Sleep(500);
            }
            // 所有窗口和程序的比例为正
            Assert.All(WindowProportion.Values, item => Assert.True(item > 0));
            Assert.All(ProgramProportion.Values, item => Assert.True(item > 0));
            // 所有窗口和程序的比例之和为1
            Assert.Equal(1, WindowProportion.Values.Sum(), 4);
            Assert.Equal(1, ProgramProportion.Values.Sum(), 4);
        }
    }
}
