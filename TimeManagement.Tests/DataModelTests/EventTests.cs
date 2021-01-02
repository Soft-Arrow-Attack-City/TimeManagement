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
            Assert.True(now <= timeEvent.Created);
            Assert.True(timeEvent.Created <= DateTime.Now);
        }
        [Fact]
        public void InfoTest()
        {
            string title = "Test title";
            string description = "This is a description";
            TimeEvent timeEvent = new TimeEvent { Title = title, Description = description };
            Assert.Contains(title, timeEvent.EventInfo);
        }
    }
}
