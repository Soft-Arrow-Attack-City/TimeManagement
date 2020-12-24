using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace TimeManagement.DataModel
{
    public enum Freq { NoRepeat, Daily, Weekly, Monthly, Annual}

    [MessagePackObject]
    public class Schedule: TimeEvent
    {
        [Key(3)]
        public DateTime Start { get; set; } = DateTime.Now;
        [Key(4)]
        public TimeSpan Duration { get; set; } = new TimeSpan(0, 5, 0);
        [Key(5)]
        public string Comment { get; set; } = "";
        [Key(6)]
        public int Priority { get; set; } = 1;
        [Key(7)]
        public Freq Repeat { get; set; } = Freq.NoRepeat;

        [IgnoreMember]
        public Schedule NextSchedule
        {
            get
            {
                if (Repeat == Freq.NoRepeat)
                    throw new ArgumentException("Cannot repeat.");
                DateTime newStart=Start;
                DateTime today = DateTime.Now.Date;
                switch (Repeat)
                {

                    case Freq.Daily:
                        newStart = today.Date + newStart.TimeOfDay;
                        break;
                    case Freq.Weekly:
                        while (newStart < today) newStart.AddDays(7);
                        break;
                    case Freq.Monthly:
                        while (newStart < today) newStart.AddMonths(1);
                        break;
                    case Freq.Annual:
                        while (newStart < today) newStart.AddYears(1);
                        break;
                }
                return new Schedule { Title = Title, Start = newStart, Duration = Duration, Comment = Comment, Priority = Priority, Repeat = Repeat };
            }
        }



        [IgnoreMember]
        private static Dictionary<Guid, Schedule> ArchivedSchedules = new Dictionary<Guid, Schedule>();
        [IgnoreMember]
        private static Dictionary<Guid, Schedule> ActiveSchedules= new Dictionary<Guid, Schedule>();
        
        

        //程序刚打开的时候应该有这么一步，来处理一下可能已经过期了的日程。
        //根据当前的日期时间来刷新日程，该封存的封存，该排到下个周期的排到下个周期
        private static bool refreshSchedule()
        {
            List<Guid> toremovelist = new List<Guid>();

            foreach(KeyValuePair<Guid, Schedule> kvp in ActiveSchedules)
            {
                if (kvp.Value.Start < DateTime.Now)
                {
                    toremovelist.Add(kvp.Key);
                }
            }
            foreach(Guid id in toremovelist)
            {
                ArchivedSchedules.Add(id, ActiveSchedules[id]);
                if (ActiveSchedules[id].Repeat != Freq.NoRepeat)
                {
                    ActiveSchedules.Add(Guid.NewGuid(), ActiveSchedules[id].NextSchedule);
                }
                ActiveSchedules.Remove(id);
            }
            return true;
        }

        //判断前方有没有等待提醒的日程。（注意调用之前应该先刷新日程）
        public static bool haveActiveSchedule()
        {
            return ActiveSchedules.Count > 0;
        }

        //添加一个日程，允许在过去或者未来的某天添加一个可重复的日程，所以添加完以后要refresh。
        public static bool AddSchedule(Schedule s)
        {
            ActiveSchedules.Add(Guid.NewGuid(), s);
            refreshSchedule();
            return true;
        }

        //获取当前时刻最近的日程，用于启动定时器。只有当初次启动程序或者已经结掉一个日程需要找下一个的时候才会调用。
        //如果前面有很多已经错过的日程（指的是连结束时间都错过的那种），则直接扔进archive里面。用户也可以指定将某一个扔进archive里面。
        public static KeyValuePair<Guid, Schedule> getNearestSchedule()
        {
            refreshSchedule();
            if (!haveActiveSchedule()) throw new ArgumentException("No Schedule!");

            return new KeyValuePair<Guid, Schedule>();


        }

        public static bool popSchedule(Guid guid)
        {
            return true;
        }

        public static Dictionary<Guid, Schedule> getSchedulesofDay(DateTime dt)
        {
            return null;
        }

        public static bool saveAllSchedule()
        {
            return true;
        }

        public static bool loadAllSchedule()
        {
            return true;
        }


    }

}
