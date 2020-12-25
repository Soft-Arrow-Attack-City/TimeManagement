using System.Diagnostics;

namespace TimeManagement.Utilities
{
    public static class Link
    {
        public static void OpenInBrowser(string url)
        {
            // Referenced MaterialDesignDemo.Domain
            // https://github.com/dotnet/corefx/issues/10361
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
    }
}