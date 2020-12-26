using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace TimeManagement.DataModel
{
    [MessagePackObject]
    public class MyTask : TimeEvent
    {
        [Key(3)]
        public DateTime Due { get; set; } = DateTime.Now.Date;

        [Key(4)]
        public string Comment { get; set; } = "";

        [Key(5)]
        public int Priority { get; set; } = 1;

        //[IgnoreMember]
        //public bool Overdue { get { return DateTime.Now > Due; } }
        [IgnoreMember]
        private static readonly string fileName = "Task.dat";

        [IgnoreMember]//未完成的任务，蓝，黄，红，允许完成或删除
        private static Dictionary<Guid, MyTask> ActiveTasks = new Dictionary<Guid, MyTask>();

        [IgnoreMember]//已完成，未到期的任务，绿，允许删除
        private static Dictionary<Guid, MyTask> FinishedTasks = new Dictionary<Guid, MyTask>();

        [IgnoreMember]//已到期的任务（可以设置成仅保留最近一周的），灰，允许删除
        private static Dictionary<Guid, MyTask> OveredTasks = new Dictionary<Guid, MyTask>();

        public static MyTask getActiveTask(Guid id)
        {
            return ActiveTasks[id];
        }

        public static MyTask getFinishedTask(Guid id)
        {
            return FinishedTasks[id];
        }

        public static MyTask getOveredTask(Guid id)
        {
            return OveredTasks[id];
        }

        public static bool AddTask(MyTask mt)
        {
            ActiveTasks.Add(Guid.NewGuid(), mt);
            refreshTasks();
            saveAllTasks();
            return true;
        }

        public static bool FinishTask(Guid id)
        {
            FinishedTasks.Add(id, ActiveTasks[id]);
            ActiveTasks.Remove(id);
            saveAllTasks();
            return true;
        }

        public static bool UnfinishTask(Guid id)
        {
            ActiveTasks.Add(id, FinishedTasks[id]);
            FinishedTasks.Remove(id);
            saveAllTasks();
            return true;
        }

        public static bool DeleteTask(Guid id)
        {
            if (ActiveTasks.ContainsKey(id)) ActiveTasks.Remove(id);
            if (FinishedTasks.ContainsKey(id)) FinishedTasks.Remove(id);
            if (OveredTasks.ContainsKey(id)) OveredTasks.Remove(id);
            saveAllTasks();
            return true;
        }

        public static bool refreshTasks()
        {
            DateTime today = DateTime.Now.Date;
            List<Guid> dellist1 = new List<Guid>();
            foreach (KeyValuePair<Guid, MyTask> kvp in ActiveTasks)
            {
                if (kvp.Value.Due.Date < today) dellist1.Add(kvp.Key);
            }

            List<Guid> dellist2 = new List<Guid>();
            foreach (KeyValuePair<Guid, MyTask> kvp in FinishedTasks)
            {
                if (kvp.Value.Due.Date < today) dellist2.Add(kvp.Key);
            }

            foreach (Guid id in dellist1)
            {
                OveredTasks.Add(id, ActiveTasks[id]);
                ActiveTasks.Remove(id);
            }
            foreach (Guid id in dellist2)
            {
                OveredTasks.Add(id, FinishedTasks[id]);
                FinishedTasks.Remove(id);
            }
            return true;
        }

        public static List<Guid> getActiveTasksByDue()
        {
            List<Guid> guids = new List<Guid>();
            var dicSort = from objDic in ActiveTasks orderby objDic.Value.Due ascending, objDic.Value.Priority descending select objDic;
            foreach (KeyValuePair<Guid, MyTask> kvp in dicSort)
            {
                guids.Add(kvp.Key);
            }
            return guids;
        }

        public static List<Guid> getActiveTasksByPriority()
        {
            List<Guid> guids = new List<Guid>();
            var dicSort = from objDic in ActiveTasks orderby objDic.Value.Priority descending, objDic.Value.Due ascending select objDic;
            foreach (KeyValuePair<Guid, MyTask> kvp in dicSort)
            {
                guids.Add(kvp.Key);
            }
            return guids;
        }

        public static List<Guid> getFinishedTasks()
        {
            List<Guid> guids = new List<Guid>();
            var dicSort = from objDic in FinishedTasks orderby objDic.Value.Due ascending select objDic;
            foreach (KeyValuePair<Guid, MyTask> kvp in dicSort)
            {
                guids.Add(kvp.Key);
            }
            return guids;
        }

        public static List<Guid> getOveredTasks()
        {
            List<Guid> guids = new List<Guid>();
            var dicSort = from objDic in OveredTasks orderby objDic.Value.Due ascending select objDic;
            foreach (KeyValuePair<Guid, MyTask> kvp in dicSort)
            {
                guids.Add(kvp.Key);
            }
            return guids;
        }

        public static bool saveAllTasks()
        {
            refreshTasks();
            try
            {
                File.WriteAllBytes($"Active{fileName}", MessagePackSerializer.Serialize(ActiveTasks));
                File.WriteAllBytes($"Finished{fileName}", MessagePackSerializer.Serialize(FinishedTasks));
                File.WriteAllBytes($"Overed{fileName}", MessagePackSerializer.Serialize(OveredTasks));
            }
            catch (IOException e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
            return true;
        }

        public static bool loadAllTasks()
        {
            try
            {
                if (File.Exists($"Active{fileName}"))
                    ActiveTasks = MessagePackSerializer.Deserialize<Dictionary<Guid, MyTask>>(File.ReadAllBytes($"Active{fileName}"));
                if (File.Exists($"Finished{fileName}"))
                    FinishedTasks = MessagePackSerializer.Deserialize<Dictionary<Guid, MyTask>>(File.ReadAllBytes($"Finished{fileName}"));
                if (File.Exists($"Overed{fileName}"))
                    OveredTasks = MessagePackSerializer.Deserialize<Dictionary<Guid, MyTask>>(File.ReadAllBytes($"Overed{fileName}"));
            }
            catch (IOException e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
            refreshTasks();
            return true;
        }

        public static void clearAllTasks()
        {
            ActiveTasks.Clear();
            FinishedTasks.Clear();
            OveredTasks.Clear();
        }
    }
}