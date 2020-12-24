using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace TimeManagement.DataModel
{
    [MessagePackObject]
    public class Task: TimeEvent
    {
        [Key(3)]
        public DateTime Due { get; set; } = DateTime.Now.AddMinutes(1);
        [Key(4)]
        public string Comment { get; set; } = "";
        [Key(5)]
        public int Priority { get; set; } = 1;

        [IgnoreMember]
        public bool Overdue { get { return DateTime.Now > Due; } }
    }
}
