using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace TimeManagement.DataModel
{
    [MessagePackObject]
    public class TimeEvent
    {
        [Key(0)]
        public DateTime Created { get; } = DateTime.Now;
        [Key(1)]
        public string Title { get; set; } = "";
        [Key(2)]
        public string Description { get; set; } = "";

        [IgnoreMember]
        public string EventInfo { get { return $"Title: {Title}; Created: {Created}"; } }
    }
}