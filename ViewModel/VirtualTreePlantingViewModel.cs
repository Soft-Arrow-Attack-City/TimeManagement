using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FluentScheduler;
using TimeManagement.DataModel;
using Task = System.Threading.Tasks.Task;

namespace TimeManagement.ViewModel
{
    class VirtualTreePlantingViewModel : INotifyPropertyChanged
    {
        public VirtualTreePlantingViewModel()
        {
            JobManager.AddJob(
                () => UpdateProcess(),
                s => s.ToRunNow().AndEvery(30).Seconds().DelayFor(1).Seconds()
            );
            JobManager.AddJob(
                () => InitializeListBox(),
                s => s.ToRunOnceIn(2).Seconds()
            );
            TreeSession.loadAllTreeSession();
        }

        private ObservableCollection<string> _ListBoxContent = new ObservableCollection<string>();
        private ObservableCollection<TreeSession> _TreeHistoryListViewContent = new ObservableCollection<TreeSession>();
        private string _SearchText = "";
        public HashSet<string> Processes { get; set; } = new HashSet<string>();
        public HashSet<string> Selected { get; set; } = new HashSet<string>();
        private TreeSession MyTree { get; set; }
        private bool _Planting = false;
        public bool PlantSuccess { get; private set; } = false;
        public string TreeTitle { get => MyTree?.Title ?? "未命名任务"; }
        private TimeSpan _TotalDuration = TimeSpan.Zero;
        private TimeSpan _TimeLeft = TimeSpan.Zero;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public string SearchText
        {
            get => _SearchText;
            set
            {
                if (SetField(ref _SearchText, value))
                {
                    ListBoxContent.Clear();
                    if (_SearchText.Length > 0)
                    {
                        string text = _SearchText.Trim().ToLower();
                        foreach (string p in Processes)
                            if (p.ToLower().StartsWith(text))
                                ListBoxContent.Add(p);
                    }
                    else
                        InitializeListBox();
                }
            }
        }

        public ObservableCollection<string> ListBoxContent
        {
            get => _ListBoxContent;
            set
            {
                SetField(ref _ListBoxContent, value);
            }
        }        
        
        public ObservableCollection<TreeSession> TreeHistoryListViewContent
        {
            get => _TreeHistoryListViewContent;
            set
            {
                SetField(ref _TreeHistoryListViewContent, value);
            }
        }

        public TimeSpan TotalDuration
        {
            get => _TotalDuration;
            set
            {
                SetField(ref _TotalDuration, value);
            }
        }

        public bool Planting
        {
            get => _Planting;
            set
            {
                SetField(ref _Planting, value);
            }
        }        
        
        public TimeSpan TimeLeft
        {
            get => _TimeLeft;
            set
            {
                SetField(ref _TimeLeft, value);
            }
        }

        private void UpdateProcess()
        {
            Process[] localAll = Process.GetProcesses();
            string[] processNames = localAll.Select(p => p.ProcessName).ToArray();
            Processes.UnionWith(processNames);
        }

        private void InitializeListBox()
        {
            ListBoxContent.Clear();
            foreach (string p in Processes)
                ListBoxContent.Add(p);
        }

        public void PlantStart(TreeSession tree)
        {
            MyTree = tree;
            Registry registry = new Registry();
            registry.Schedule(() => CheckPlanting()).WithName("tree").ToRunEvery(3).Seconds();
            registry.Schedule(() => UpdateTime()).WithName("timer").ToRunEvery(1).Seconds();
            JobManager.Initialize(registry);
        }       
        
        public void UpdateHistory()
        {
            TreeHistoryListViewContent = new ObservableCollection<TreeSession>(TreeSession.RecentTree);
            TotalDuration = TreeSession.getTotalDuration;
        }

        private void CheckPlanting()
        {
            string[] currentProcesses = Process.GetProcesses().Select(p => p.ProcessName).ToArray();
            if (MyTree.Due ||
                (Selected.Intersect(currentProcesses).Count() > 0))
            {
                JobManager.RemoveJob("tree");
                JobManager.RemoveJob("timer");
                if (MyTree.Due)
                    PlantSuccess = true;
                else
                    PlantSuccess = false;

                Planting = false;
                MyTree.Success = PlantSuccess;
                TreeSession.addTree(MyTree);
            }
        }

        public void UpdateTime()
        {
            TimeLeft = MyTree.End - DateTime.Now;
        }
    }
}
