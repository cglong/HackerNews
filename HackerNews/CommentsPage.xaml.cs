﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HackerNews.API;
using HackerNews.API.Models;
using System.Collections.ObjectModel;
using HackerNews.Common;
using Microsoft.Phone.Tasks;
using System.Windows.Media;
using System.Collections;
using HackerNews.Models;

namespace HackerNews
{
    public partial class CommentsPage : PhoneApplicationPage
    {
        #region List Properties

        public static CommentResponse CurrentPost { get; set; }

        public static ObservableCollection<CommentItem> Comments { get; set; }

        #endregion

        private bool isLoaded = false;

        public CommentsPage()
        {
            InitializeComponent();

            App.UnhandledExceptionHandled += new EventHandler<ApplicationUnhandledExceptionEventArgs>(App_UnhandledExceptionHandled);

            Comments = new ObservableCollection<CommentItem>();
        }

        private void App_UnhandledExceptionHandled(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                this.txtTitle.Visibility = System.Windows.Visibility.Collapsed;
                this.txtDescription.Visibility = System.Windows.Visibility.Collapsed;

                ToggleLoadingText();

                this.txtEmpty.Visibility = System.Windows.Visibility.Visible;
                this.txtEmpty.Text = "Sorry, could not download comments right now.";

                this.prgLoading.Visibility = System.Windows.Visibility.Collapsed;
            });
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (isLoaded == false)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            string id;
            if (NavigationContext.QueryString.TryGetValue("id", out id))
            {
                this.prgLoading.Visibility = System.Windows.Visibility.Visible;

                App.HackerNewsClient.GetComments((result) =>
                {
                    SmartDispatcher.BeginInvoke(() =>
                    {
                        CurrentPost = result;

                        this.txtTitle.Text = CurrentPost.title;
                        this.txtDescription.Text = CurrentPost.description;

                        Comments.Clear();

                        Flatten(CurrentPost.comments);

                        isLoaded = true;

                        ToggleLoadingText();
                        ToggleEmptyText();

                        this.prgLoading.Visibility = System.Windows.Visibility.Collapsed;
                    });
                }, id);
            }
        }

        private void Feedback_Click(object sender, EventArgs e)
        {
            if (this.prgLoading.Visibility == System.Windows.Visibility.Visible) return;

            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.To = App.FeedbackEmailAddress;
            emailComposeTask.Subject = "Hacker News Feedback";
            emailComposeTask.Body = "Version " + App.ExtendedVersionNumber + " (" + App.PlatformVersionNumber + ")\n\n";
            emailComposeTask.Show();
        }

        private void About_Click(object sender, EventArgs e)
        {
            if (this.prgLoading.Visibility == System.Windows.Visibility.Visible) return;

            NavigationService.Navigate(new Uri("/YourLastAboutDialog;component/AboutPage.xaml", UriKind.Relative));
        }

        private void ToggleLoadingText()
        {
            SmartDispatcher.BeginInvoke(() =>
            {
                this.txtLoading.Visibility = System.Windows.Visibility.Collapsed;

                this.lstComments.Visibility = System.Windows.Visibility.Visible;
            });
        }

        private void ToggleEmptyText()
        {
            SmartDispatcher.BeginInvoke(() =>
            {
                if (Comments.Count == 0)
                    this.txtEmpty.Visibility = System.Windows.Visibility.Visible;
                else
                    this.txtEmpty.Visibility = System.Windows.Visibility.Collapsed;
            });
        }

        private static void Flatten(List<Comment> enumerable)
        {
            foreach (Comment item in enumerable)
            {
                Comments.Add(new CommentItem(item));

                Flatten(item.comments);
            }
        }
    }
}