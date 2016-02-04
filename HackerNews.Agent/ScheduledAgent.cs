using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using HackerNews.API;
using System;

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

        protected override async void OnInvoke(ScheduledTask task)
        {
            var client = new ServiceClient(Debugger.IsAttached);
            await client.GetTopPosts(posts =>
            {
                var first = posts[0];
                var tileData = new IconicTileData
                {
                    Title = "Hacker News",
                    BackgroundColor = Color.FromArgb(255, 255, 102, 0),
                    Count = posts.Count,
                    IconImage = new Uri(@"Assets\Tiles\IconImage.png", UriKind.Relative),
                    SmallIconImage = new Uri(@"Assets\Tiles\SmallIconImage.png", UriKind.Relative),
                    WideContent1 = first.title,
                    WideContent2 = first.description,
                };

                var mainTile = ShellTile.ActiveTiles.First();
                if (mainTile != null)
                {
                    mainTile.Update(tileData);
                }

                NotifyComplete();
            });
        }
    }
}
