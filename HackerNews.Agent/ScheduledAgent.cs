using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;

namespace HackerNews.Agent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        static ScheduledAgent()
        {
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        protected override void OnInvoke(ScheduledTask task)
        {
            NotifyComplete();
        }
    }
}