using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeManagement.DataModel;
using FluentScheduler;


namespace TimeManagement.ViewModel
{
    class VirtualTreePlantingViewModel : INotifyPropertyChanged
    {
        public VirtualTreePlantingViewModel()
        {
            JobManager.AddJob(
                () => UpdateProcess(),
                s => s.ToRunNow().AndEvery(1).Minutes().DelayFor(1).Seconds()
            );
            JobManager.AddJob(
                () => InitializeListBox(),
                s => s.ToRunOnceIn(2).Seconds()
            );
        }

        private ObservableCollection<string> _ListBoxContent = new ObservableCollection<string>();
        private TreeSession Tree = new TreeSession();
        private string _SearchText = "";
        private HashSet<string> processes = new HashSet<string>();

        public event PropertyChangedEventHandler? PropertyChanged;

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
                        foreach (string p in processes)
                            if (p.ToLower().StartsWith(text))
                                ListBoxContent.Add(p);
                    }
                    else
                    {
                        InitializeListBox();
                    }
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

        private void UpdateProcess()
        {
            Process[] localAll = Process.GetProcesses();
            string[] processNames = localAll.Select(p => p.ProcessName).ToArray();
            processes.Clear();
            processes.UnionWith(processNames);
        }

        private void InitializeListBox()
        {
            ListBoxContent.Clear();
            foreach (string p in processes)
                ListBoxContent.Add(p);
        }
    }
}
