using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace TimeManagement.DataModel
{
    [MessagePackObject]
    public class TreeSession: TimeEvent
    {
        [Key(2)]
        public TimeSpan Duration { get; set; } = new TimeSpan(0, 25, 0);
        [Key(3)]
        public string Type { get; set; } = "";
        [Key(4)]
        public DateTime End { get; set; } = DateTime.Now.AddDays(1);
        [IgnoreMember]
        public bool Due { get { return DateTime.Now >= Created + Duration; } }
        [IgnoreMember]
        public bool Success { get { return End - Created >= Duration; } }
    }
}
