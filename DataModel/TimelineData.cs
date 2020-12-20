using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagement.DataModel
{
    class Alog
    {
        public int t;//暂定0到86400之间的一个数。
        public string s;

        public Alog(int tt, string ss)
        {
            t = tt;// new DateTime(2020, 12, 11, tt / 3600, (tt / 60) % 60, tt % 60);
            s = ss;
        }

        public static bool operator <(Alog a, Alog b)
        {
            return a.t < b.t;
        }
        public static bool operator >(Alog a, Alog b)
        {
            return a.t > b.t;
        }

        public static bool operator ==(Alog a, Alog b)
        {
            return a.t == b.t;
        }
        public static bool operator !=(Alog a, Alog b)
        {
            return a.t != b.t;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

    class AlogCompare : IComparer<Alog>     //对时间排序
    {
        public int Compare(Alog x, Alog y)
        {
            return (int)(x.t - y.t);//.TotalSeconds;
        }
    }

    class TimelineData
    {


        private static Random random = new Random();
        


        //当前暂未与后端对接，使用generagedata生产随机的数据，测试前端代码。
        public static List<Alog> generatedata()
        {

            SortedSet<Alog> logs = new SortedSet<Alog>(new AlogCompare());
            List<string> ls = new List<string>(new string[] { "1rdgv4v", "2c32crq", "3q5r34ctv", "44c3c6c", "54vkgt3", "634ct9v8", "743vc53", "86b3bv" });
            for (int i = 0; i < 1000; i++)
            {
                logs.Add(new Alog(random.Next(10000) + 26000, ls[random.Next(3)]));
                logs.Add(new Alog(random.Next(10000) + 36000, ls[random.Next(3) + 1]));
                logs.Add(new Alog(random.Next(10000) + 46000, ls[random.Next(3) + 2]));
                logs.Add(new Alog(random.Next(10000) + 56000, ls[random.Next(3) + 3]));
                logs.Add(new Alog(random.Next(10000) + 66000, ls[random.Next(3) + 4]));
                logs.Add(new Alog(random.Next(10000) + 76000, ls[random.Next(3) + 5]));
            }

            List<Alog> sortedLogs = new List<Alog>(logs);
            return sortedLogs;
        }


    }
}
