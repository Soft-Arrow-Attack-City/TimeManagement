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
                var newStart = Start;
                switch (Repeat)
                {
                    case Freq.Daily:
                        newStart.AddDays(1);
                        break;
                    case Freq.Weekly:
                        newStart.AddDays(7);
                        break;
                    case Freq.Monthly:
                        newStart.AddMonths(1);
                        break;
                    case Freq.Annual:
                        newStart.AddYears(1);
                        break;
                }
                return new Schedule { Title = Title, Start = newStart, Duration = Duration, Comment = Comment, Priority = Priority, Repeat = Repeat };
            }
        }
    }
}
