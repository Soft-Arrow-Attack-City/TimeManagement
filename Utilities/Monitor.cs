using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Management;

namespace TimeManagement.Utilities
{


    //矩形类。
    [StructLayout(LayoutKind.Sequential)]
    struct RECT
    {
        public int Left; //最左坐标
        public int Top; //最上坐标
        public int Right; //最右坐标
        public int Bottom; //最下坐标
        public RECT(int l = 0, int t = 0, int r = 0, int b = 0)
        {
            Left = l;
            Top = t;
            Right = r;
            Bottom = b;
        }
        public bool isLegal()
        {
            if ((Left < Right) && (Top < Bottom)) return true;
            else return false;
        }
        public int getArea()
        {
            return (Bottom - Top) * (Right - Left);
        }
    }

    //分屏树，对每个屏幕单独建树。
    class DivideScreenTree
    {
        private RECT bound;//本矩形的范围
        private IntPtr hWnd;//如果句柄为零，则说明还没有被分配，可以再有list。否则说明已经被分配，不能再有list了。
        private List<DivideScreenTree> childRect;

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]//指定坐标处窗体句柄
        private static extern IntPtr WindowFromPoint(int xPoint, int yPoint);

        //DEBUG
        [DllImport("user32.dll")]
        private extern static int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);



        public DivideScreenTree(Screen s)
        {
            bound.Left = s.Bounds.Left;
            bound.Right = s.Bounds.Right;
            bound.Top = s.Bounds.Top;
            bound.Bottom = s.Bounds.Bottom;
            hWnd = IntPtr.Zero;
            childRect = new List<DivideScreenTree>();
        }

        private DivideScreenTree(RECT r, IntPtr h)
        {
            bound.Left = r.Left;
            bound.Right = r.Right;
            bound.Top = r.Top;
            bound.Bottom = r.Bottom;
            hWnd = h;
            childRect = new List<DivideScreenTree>();
        }

        //判断两个矩形是否相交。注意右和下应该减一，这样判断出的相交才是真实的，否则紧挨着也会判相交。
        private bool isIntersetRect(RECT r)
        {
            //这个是计算几何里面的矩形相交判定公式。
            //Debug.Assert(r.isLegal());
            int zx = Math.Abs(bound.Left + bound.Right - r.Left - r.Right);
            int x = (bound.Right - 1 - bound.Left) + (r.Right - 1 - r.Left);
            int zy = Math.Abs(bound.Top + bound.Bottom - r.Top - r.Bottom);
            int y = (bound.Bottom - 1 - bound.Top) + (r.Bottom - 1 - r.Top);
            return (zx <= x) && (zy <= y);
        }

        //来一个窗口矩形，把分屏树或其子树分成多个矩形。
        //使用之前必须先判断这是一个有面积的矩形。也就是左右至少差1，上下至少差1。
        public void DivideByWindow(RECT r, IntPtr h)
        {
            //如果这一块已经有窗口句柄了，就不再分了，因为从上到下的顺序是有覆盖关系的。所以分过的就不能再分了。
            if (hWnd != IntPtr.Zero) return;
            //首先需要计算两个矩形是否相交。如果不相交，则直接返回。
            if (!isIntersetRect(r)) return;
            //确认相交以后，开始分。

            if (childRect.Count != 0)
            {
                //如果已经分过了，则往下进行，分它下面那些完整的矩形。
                foreach (DivideScreenTree dst in childRect)
                {
                    dst.DivideByWindow(r, h);
                }
            }
            else
            {
                //如果还没分过，则分它自己。把它自己的列表用起来。
                //判断都是严格的，因为像素，至少差1，否则会出现很多没有面积的矩形。

                //如果左边有空白，加进子树列表。
                if (bound.Left < r.Left)
                {
                    childRect.Add(
                        new DivideScreenTree(
                            new RECT(bound.Left, bound.Top, r.Left, bound.Bottom),
                            IntPtr.Zero
                            )
                        );
                }
                //如果右边有空白，加进子树列表。
                if (bound.Right > r.Right)
                {
                    childRect.Add(
                        new DivideScreenTree(
                            new RECT(r.Right, bound.Top, bound.Right, bound.Bottom),
                            IntPtr.Zero
                            )
                        );
                }
                //处理完左右，再看上下的情况。
                //如果上边有空白，加进子树列表。(注意处理左右)
                if (bound.Top < r.Top)
                {
                    childRect.Add(
                        new DivideScreenTree(
                            new RECT(Math.Max(bound.Left, r.Left), bound.Top, Math.Min(bound.Right, r.Right), r.Top),
                            IntPtr.Zero
                            )
                        );
                }
                //如果下边有空白，加进子树列表。(注意处理左右)
                if (bound.Bottom > r.Bottom)
                {
                    childRect.Add(
                        new DivideScreenTree(
                            new RECT(Math.Max(bound.Left, r.Left), r.Bottom, Math.Min(bound.Right, r.Right), bound.Bottom),
                            IntPtr.Zero
                            )
                        );
                }
                //最后处理中间的矩形，加进子树列表，而且已经有了窗体。
                //判断窗体是不是FolderView。如果是，则这一块应该是桌面。


                childRect.Add(
                new DivideScreenTree(
                    new RECT(
                        Math.Max(bound.Left, r.Left),
                        Math.Max(bound.Top, r.Top),
                        Math.Min(bound.Right, r.Right),
                        Math.Min(bound.Bottom, r.Bottom)
                        ),
                    WindowFromPoint(
                        Math.Max(bound.Left, r.Left) + 1,
                        Math.Max(bound.Top, r.Top) + 1
                        )
                    )
                );


            }
        }

        //建树完成后生成报表，某个窗口句柄占屏幕面积是多少。
        public void GetAppPixels(ref Dictionary<IntPtr, int> pixelDict)
        {
            //如果有子树，进子树算面积。
            if (childRect.Count > 0)
            {
                foreach (DivideScreenTree dst in childRect)
                {
                    dst.GetAppPixels(ref pixelDict);
                }
            }
            //如果已经是最细的子树了，则算面积。
            else
            {
                if (pixelDict.ContainsKey(hWnd)) pixelDict[hWnd] += bound.getArea();
                else pixelDict.Add(hWnd, bound.getArea());
            }

        }

    }

    //主类
    public class Monitor
    {
        private static List<DivideScreenTree> screenList;
        private delegate bool CallBack(IntPtr hwnd, int lParam);
        private static CallBack myCallBack = new CallBack(DealWith);
        private static bool FrontWindowAppears;
        private static IntPtr FrontWindow;


        private static bool DealWith(IntPtr hwnd, int lParam)
        {

            if (hwnd == FrontWindow) FrontWindowAppears = true;
            if (!FrontWindowAppears) return true;

            if (!IsWindow(hwnd)) return true;
            if (!IsWindowEnabled(hwnd)) return true;
            if (!IsWindowVisible(hwnd)) return true;

            RECT r = new RECT();
            GetWindowRect(hwnd, ref r);
            if (!r.isLegal()) return true;

            foreach (DivideScreenTree tree in screenList)
            {
                tree.DivideByWindow(r, hwnd);
                //if (tree.JudgeWindowValid(r, hwnd)) 
            }


            return true;
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern int EnumWindows(CallBack x, int y);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private extern static int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool IsWindow(IntPtr hWnd);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool IsWindowEnabled(IntPtr hWnd);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);


        private static string GetMainModuleFilepath(int processId)
        {
            string wmiQueryString = "SELECT ProcessId, ExecutablePath FROM Win32_Process WHERE ProcessId = " + processId;
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            {
                using (var results = searcher.Get())
                {
                    ManagementObject mo = results.Cast<ManagementObject>().FirstOrDefault();
                    if (mo != null)
                    {
                        //return (string)mo["ExecutablePath"];
                        return (string)mo["ExecutablePath"];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取每个窗口的占屏比和每个应用所有窗口总和的占屏比，占CPU巨高，允许调用频率：1qps
        /// </summary>
        /// <returns>第一个是窗口占屏比，第二个是每个程序的所有窗口占屏比之和</returns>
        public static void GetAllWindowProportion(ref Dictionary<string, double> WindowProportion, ref Dictionary<string, double> ProgramProportion)
        {
            screenList = new List<DivideScreenTree>();
            double allarea = 0;
            foreach (Screen s in Screen.AllScreens)
            {
                allarea += s.Bounds.Width * s.Bounds.Height;
                screenList.Add(new DivideScreenTree(s));
            }
            FrontWindowAppears = false;
            FrontWindow = GetForegroundWindow();


            EnumWindows(myCallBack, 0);


            Dictionary<IntPtr, int> pixelDict = new Dictionary<IntPtr, int>();
            foreach (DivideScreenTree tree in screenList)
            {
                tree.GetAppPixels(ref pixelDict);
            }

            WindowProportion.Clear();
            ProgramProportion.Clear();

            foreach (KeyValuePair<IntPtr, int> kvp in pixelDict)
            {
                StringBuilder s = new StringBuilder(512);
                GetWindowText(kvp.Key, s, s.Capacity);
                string winname = s.ToString() + "@" + kvp.Key.ToString();
                double winprop = kvp.Value / allarea;
                WindowProportion.Add(winname, winprop);

                int pid;
                GetWindowThreadProcessId(kvp.Key, out pid);
                Process myProcess = Process.GetProcessById(pid);
                string path = GetMainModuleFilepath(myProcess.Id);

                if (path == null) continue;

                if (ProgramProportion.ContainsKey(path))
                {
                    ProgramProportion[path] += winprop;
                }
                else
                {
                    ProgramProportion.Add(path, winprop);
                }


            }

        }

        /// <summary>
        /// 获取当前被激活的窗口标题，占CPU较低，允许调用频率：>10qps
        /// </summary>
        /// <returns>返回当前被激活的窗口标题</returns>
        public static string GetForgroundWindowName()
        {
            IntPtr hwnd = GetForegroundWindow();

            StringBuilder s = new StringBuilder(512);
            GetWindowText(hwnd, s, s.Capacity);
            string winname = s.ToString() + "@" + hwnd.ToString();

            return winname;
        }

        /// <summary>
        /// 获取当前被激活的窗口的程序的可执行文件路径，占CPU较高，允许调用频率：5qps
        /// </summary>
        /// <returns>返回当前激活的窗口的程序的可执行文件路径</returns>
        public static string GetForgroundWindowProgram()
        {
            IntPtr hwnd = GetForegroundWindow();
            int pid;
            GetWindowThreadProcessId(hwnd, out pid);
            Process myProcess = Process.GetProcessById(pid);
            string path = GetMainModuleFilepath(myProcess.Id);
            return path;
        }

        /// <summary>
        /// 获取当前被激活的窗口的程序的占屏比，占CPU较低，允许调用频率：>10qps
        /// </summary>
        /// <returns>返回当前被激活的窗口的程序的占屏比</returns>
        public static double GetForgroundWindowProportion()
        {
            IntPtr hwnd = GetForegroundWindow();
            RECT winrect = new RECT();
            GetWindowRect(hwnd, ref winrect);

            screenList = new List<DivideScreenTree>();
            double allarea = 0;
            double effectivearea = 0;
            foreach (Screen s in Screen.AllScreens)
            {
                allarea += s.Bounds.Width * s.Bounds.Height;
                RECT r = new RECT();
                r.Left = Math.Max(s.Bounds.Left, winrect.Left);
                r.Top = Math.Max(s.Bounds.Top, winrect.Top);
                r.Right = Math.Min(s.Bounds.Right, winrect.Right);
                r.Bottom = Math.Min(s.Bounds.Bottom, winrect.Bottom);
                if (r.isLegal())
                {
                    effectivearea += r.getArea();
                }

            }
            return effectivearea / allarea;
        }

        /// <summary>
        /// 按值从大到小打印字典的函数。
        /// </summary>
        /// <param name="dic">待打印的字典</param>
        public static void ShowDictionarySortByValue(Dictionary<string, double> dic)
        {
            var dicSort = from objDic in dic orderby objDic.Value descending select objDic;
            foreach (KeyValuePair<string, double> kvp in dicSort)
            {
                Console.WriteLine(kvp.Value.ToString("P6") + "\t" + kvp.Key);
            }
            Console.WriteLine();
        }

        public static void testMonitor()
        {
            //调用示例
            Dictionary<string, double> WindowProportion = new Dictionary<string, double>();
            Dictionary<string, double> ProgramProportion = new Dictionary<string, double>();

            while (true)
            {

                GetAllWindowProportion(ref WindowProportion, ref ProgramProportion);
                Console.Clear();
                Console.WriteLine("======每个窗口的占屏比======");
                ShowDictionarySortByValue(WindowProportion);
                Console.WriteLine("======每个程序的占屏比======");
                ShowDictionarySortByValue(ProgramProportion);
                Console.WriteLine("======主窗口相关信息======");
                Console.WriteLine("主窗口名称：\t" + GetForgroundWindowName());
                Console.WriteLine("执行文件路径：\t" + GetForgroundWindowProgram());
                Console.WriteLine("主窗口占屏比：\t" + GetForgroundWindowProportion().ToString("P6"));

                Thread.Sleep(1000);

            }
        }
    }

}