using MaterialDesignThemes.Wpf;

namespace TimeManagement.ViewModel
{
    internal class MainWindowViewModel
    {
        public MainWindowViewModel(ISnackbarMessageQueue snackbarMessageQueue) => MainSnackbarMessageQueue = snackbarMessageQueue;

        public static ISnackbarMessageQueue MainSnackbarMessageQueue { get; private set; }
    }
}