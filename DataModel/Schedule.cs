﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace TimeManagement.DataModel
{
    enum Freq { NoRepeat, Daily, Weekly, Monthly}

    [MessagePackObject]
    class Schedule: Event
    {
        [Key(3)]
        public DateTime Start { get; set; }
        [Key(4)]
        public TimeSpan Duration { get; set; }
        [Key(5)]
        public string Comment { get; set; }
        [Key(6)]
        public int Priority { get; set; }
        [Key(7)]
        public Freq Repeat { get; set; } = Freq.NoRepeat;
    }
}
