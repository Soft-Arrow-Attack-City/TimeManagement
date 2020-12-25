using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace TimeManagement.DataModel
{
    [MessagePackObject]
    public class TreeSession : TimeEvent
    {
        [Key(3)]
        public TimeSpan Duration { get; set; } = new TimeSpan(0, 25, 0);

        [Key(4)]
        public string Type { get; set; } = "";

        [Key(5)]
        public bool Success { get; set; } = false;

        [IgnoreMember]
        public DateTime End { get => Created + Duration; }

        [IgnoreMember]
        public bool Due { get => DateTime.Now > End; }

        private static readonly string fileName = "TreeSessions.dat";

        [IgnoreMember]
        private static Dictionary<Guid, TreeSession> TreeSessions = new Dictionary<Guid, TreeSession>();

        [IgnoreMember]
        public static TimeSpan getTotalDuration
        {
            get => TreeSessions.Values
                .Aggregate
                (TimeSpan.Zero,
                (s, nextTree) => s + nextTree.Duration);
        }

        public static bool addTree(TreeSession tree)
        {
            TreeSessions.Add(Guid.NewGuid(), tree);
            return saveAllTreeSession();
        }

        public static bool saveAllTreeSession()
        {
            try
            {
                File.WriteAllBytes(fileName, MessagePackSerializer.Serialize(TreeSessions));
            }
            catch (IOException e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
            return true;
        }

        public static bool loadAllTreeSession()
        {
            try
            {
                if (File.Exists(fileName))
                    TreeSessions = MessagePackSerializer.Deserialize<Dictionary<Guid, TreeSession>>(
                        File.ReadAllBytes(fileName));
            }
            catch (IOException e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
            return true;
        }

        [IgnoreMember]
        public static List<TreeSession> RecentTree { get => TreeSessions.Values.OrderByDescending(t => t.End).ToList(); }
    }
}