using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TimeManagement.DataModel;

namespace TimeManagement.Tests
{
    public class EventTests
    {
        [Fact]
        public void TimeTest()
        {
            DateTime now = DateTime.Now;
            TimeEvent timeEvent = new TimeEvent();
            // 创建时间在测试开始之后
            Assert.True(now <= timeEvent.Created);
            // 又在测试语句之前
            Assert.True(timeEvent.Created <= DateTime.Now);
        }
        [Fact]
        public void InfoTest()
        {
            string title = "Test title";
            TimeEvent timeEvent = new TimeEvent { Title = title };
            // 输出信息中包含标题
            Assert.Contains(title, timeEvent.EventInfo);
        }
    }
}
