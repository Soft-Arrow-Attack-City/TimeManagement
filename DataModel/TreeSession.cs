using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MessagePack;

namespace TimeManagement.DataModel
{
    [MessagePackObject]
    public class TreeSession : TimeEvent
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

        private static readonly string fileName = "TreeSessions.dat";

        [IgnoreMember]
        private static Dictionary<Guid, TreeSession> TreeSessions = new Dictionary<Guid, TreeSession>();

        [IgnoreMember]
        public static int TotalMinutes
        {
            get => TreeSessions.Values
                .Select(t => (int)t.Duration.TotalMinutes)
                .Sum();
        }

        public static bool addTree(TreeSession tree)
        {
            TreeSessions.Add(new Guid(), tree);
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

    }
}
