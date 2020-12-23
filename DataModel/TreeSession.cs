using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace TimeManagement.DataModel
{
    [MessagePackObject]
    class TreeSession: Event
    {
        [Key(2)]
        public TimeSpan Duration { get; set; }
        [Key(3)]
        public string Type { get; set; }
        [Key(4)]
        public bool Success { get; set; }
    }
}
