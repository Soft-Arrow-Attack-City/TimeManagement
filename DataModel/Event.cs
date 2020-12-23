using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace TimeManagement.DataModel
{
    [MessagePackObject]
    class Event
    {
        [Key(0)]
        public Guid Id { get; } = Guid.NewGuid();
        [Key(1)]
        public DateTime Created { get; } = DateTime.Now;
        [Key(2)]
        public string Title { get; set; }

        [IgnoreMember]
        public string EventInfo { get { return $"Guid: {Id}; Created: {Created}"; } }
    }
}