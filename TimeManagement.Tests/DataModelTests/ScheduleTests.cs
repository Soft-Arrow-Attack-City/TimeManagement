using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TimeManagement.DataModel;
using System.Collections;

namespace TimeManagement.Tests
{
    public class ScheduleTests
    {
        [Theory]
        [ClassData(typeof(ScheduleTestData))]
        public void RepeatTest(Freq freq, TimeSpan timeSpan)
        {
            MySchedule schedule = new MySchedule { Repeat = freq };
            MySchedule nextSchedule = schedule.NextSchedule;
            Assert.Equal(freq, nextSchedule.Repeat);
            Assert.Equal(timeSpan, nextSchedule.Start - schedule.Start);
        }
        [Fact]
        public void RepeatExceptionTest()
        {
            MySchedule schedule = new MySchedule();
            Assert.Equal(Freq.NoRepeat, schedule.Repeat);
            Assert.Throws<ArgumentException>(() => schedule.NextSchedule);
        }

        public class ScheduleTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { Freq.Daily, DateTime.Now.AddDays(1) - DateTime.Now };
                yield return new object[] { Freq.Weekly, DateTime.Now.AddDays(7) - DateTime.Now };
                yield return new object[] { Freq.Monthly, DateTime.Now.AddMonths(1) - DateTime.Now };
                yield return new object[] { Freq.Annual, DateTime.Now.AddYears(1) - DateTime.Now };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
