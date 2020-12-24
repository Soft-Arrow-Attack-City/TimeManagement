using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagement.ViewModel
{
    class MainWindowViewModel
    {
        public MainWindowViewModel(ISnackbarMessageQueue snackbarMessageQueue) => MainSnackbarMessageQueue = snackbarMessageQueue;
        public static ISnackbarMessageQueue MainSnackbarMessageQueue { get; private set; }
    }
}
