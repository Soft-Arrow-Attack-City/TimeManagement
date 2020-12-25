using TimeManagement.DataModel;
using Xunit;

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
            var timeEvent = new MySchedule { Title = "Test title" };
            Assert.Equal("Test title", timeEvent.Title);
        }
    }
}