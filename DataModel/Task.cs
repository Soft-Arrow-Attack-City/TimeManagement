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


        [IgnoreMember]//未完成的任务
        public static Dictionary<Guid,Task> ActiveTasks =new Dictionary<Guid,Task>();

        [IgnoreMember]//已完成，未到期的任务
        public static Dictionary<Guid, Task> FinishedTasks = new Dictionary<Guid, Task>();

        [IgnoreMember]//已到期的任务（只显示最近一周的）
        public static Dictionary<Guid, Task> OveredTasks = new Dictionary<Guid, Task>();


        public static bool refreshTasks()
        {
            return true;
        }

        public static List<Guid> getActiveTasks()
        {
            return new List<Guid>();
        }

        public static List<Guid> getFinishedTasks()
        {
            return new List<Guid>();
        }

        public static List<Guid> getOveredTasks()
        {
            return new List<Guid>();
        }





    }
}
