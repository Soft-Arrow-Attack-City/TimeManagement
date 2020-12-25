using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TimeManagement.DataModel;

namespace TimeManagement.Tests
{
    public class DataModelTests
    {
        [Fact]
        public void EventTest()
        {
            var timeEvent = new TimeEvent { Title = "Test title" };
            Assert.Equal("Test title", timeEvent.Title);
        }

        [Fact]
        public void ScheduleTest()
        {
            var timeEvent = new MySchedule { Title="Test title" };
            Assert.Equal("Test title", timeEvent.Title);
        }
    }
}
