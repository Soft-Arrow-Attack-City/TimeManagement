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
        // 测试重复日程，能否正常获取下一日程
        [Theory]
        [ClassData(typeof(ScheduleTestData))]
        public void RepeatTest(Freq freq, TimeSpan timeSpan)
        {
            MySchedule schedule = new MySchedule { Repeat = freq };
            MySchedule nextSchedule = schedule.NextSchedule;
            Assert.Equal(freq, nextSchedule.Repeat);
            Assert.Equal(timeSpan, nextSchedule.Start - schedule.Start);
        }
        // 对不重复的日程，获取下一日程会抛出异常
        [Fact]
        public void RepeatExceptionTest()
        {
            MySchedule schedule = new MySchedule();
            Assert.Equal(Freq.NoRepeat, schedule.Repeat);
            Assert.Throws<ArgumentException>(() => schedule.NextSchedule);
        }

        // 测试日程使用功能
        [Fact]
        public void AddScheduleTest()
        {
            MySchedule.removeAllSchedule();
            MySchedule schedule = new MySchedule { Start = DateTime.Now.AddDays(1) };
            MySchedule.AddSchedule(schedule);
            Assert.True(MySchedule.haveActiveSchedule());
            var guids = MySchedule.getAllActiveSchedules();
            Assert.Single(guids);
            Assert.Same(schedule, MySchedule.getActiveSchedule(guids.First()));
            Assert.Same(schedule, MySchedule.getSchedulesofDay(DateTime.Today.AddDays(1)).Values.First());
        }

        // 测试保存功能
        [Fact]
        public void SaveTest()
        {
            MySchedule.removeAllSchedule();
            MySchedule schedule = new MySchedule { Start = DateTime.Now.AddDays(1) };
            MySchedule.AddSchedule(schedule);
            MySchedule.removeAllSchedule();
            Assert.Empty(MySchedule.getAllActiveSchedules());
            MySchedule.loadAllSchedule();
            var guids = MySchedule.getAllActiveSchedules();
            var loaded = MySchedule.getActiveSchedule(guids.First());
            // 测试关键值相等（避免时区问题）
            Assert.Equal(schedule.Created, loaded.Created);
            Assert.Equal(schedule.Start, loaded.Start);
        }
        // 每个周期的时间差对照表
        public class ScheduleTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { Freq.Daily, TimeSpan.FromDays(1) };
                yield return new object[] { Freq.Weekly, TimeSpan.FromDays(7) };
                yield return new object[] { Freq.Monthly, DateTime.Now.AddMonths(1) - DateTime.Now };
                yield return new object[] { Freq.Annual, DateTime.Now.AddYears(1) - DateTime.Now };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
