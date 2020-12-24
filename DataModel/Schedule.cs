using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace TimeManagement.DataModel
{
    public enum Freq { NoRepeat, Daily, Weekly, Monthly, Annual}
    public enum RemindMode { NoRemind, RemindOnTime, Advance5min, Advance10min, Advance30min }

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
        [Key(8)]
        public RemindMode remindMode { get; set; } = RemindMode.NoRemind;



        [IgnoreMember]
        public Schedule NextSchedule
        {
            get
            {
                if (Repeat == Freq.NoRepeat) throw new ArgumentException("Cannot repeat.");
                DateTime newStart=Start;
                DateTime today = DateTime.Now.Date;
                if (newStart < today)
                {
                    switch (Repeat)
                    {
                        case Freq.Daily:
                            newStart = today.Date + newStart.TimeOfDay;
                            break;
                        case Freq.Weekly:
                            while (newStart < today) newStart = newStart.AddDays(7);
                            break;
                        case Freq.Monthly:
                            while (newStart < today) newStart = newStart.AddMonths(1);
                            break;
                        case Freq.Annual:
                            while (newStart < today) newStart = newStart.AddYears(1);
                            break;
                    }
                }
                else
                {
                    switch (Repeat)
                    {
                        case Freq.Daily:
                            newStart = newStart.AddDays(1);
                            break;
                        case Freq.Weekly:
                            newStart = newStart.AddDays(7);
                            break;
                        case Freq.Monthly:
                            newStart = newStart.AddMonths(1);
                            break;
                        case Freq.Annual:
                            newStart = newStart.AddYears(1);
                            break;
                    }
                }


                return new Schedule {Description = Description, Title = Title, Start = newStart, Duration = Duration, Comment = Comment, Priority = Priority, Repeat = Repeat, remindMode = remindMode};
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

        //添加一个日程，允许在过去或者未来的某天添加一个可重复的日程，所以添加完以后要刷新日程。
        public static bool AddSchedule(Schedule s)
        {
            ActiveSchedules.Add(Guid.NewGuid(), s);
            refreshSchedule();
            return true;
        }

        //获取一个Guid为id的日程。如果获取不到，就直接报错了。
        public static Schedule getActiveSchedule(Guid id)
        {
            return ActiveSchedules[id];
        }

        //按照时间由近到远的顺序获取所有等待提醒的日程。（注意调用前应该刷新日程。）
        public static List<Guid> getAllActiveSchedules()
        {
            List<Guid> guids = new List<Guid>();
            var dicSort = from objDic in ActiveSchedules orderby objDic.Value.Start ascending select objDic;
            foreach (KeyValuePair<Guid, Schedule> kvp in dicSort)
            {
                guids.Add(kvp.Key);
            }
            return guids;
        }



        public static bool removeScheduleOnce(Guid id)
        {
            ArchivedSchedules.Add(id, ActiveSchedules[id]);
            if (ActiveSchedules[id].Repeat != Freq.NoRepeat)
            {
                ActiveSchedules.Add(Guid.NewGuid(), ActiveSchedules[id].NextSchedule);
            }
            ActiveSchedules.Remove(id);

            return true;
        }

        public static bool removeScheduleAll(Guid id)
        {
            ArchivedSchedules.Add(id, ActiveSchedules[id]);
            ActiveSchedules.Remove(id);
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
