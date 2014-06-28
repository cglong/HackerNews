﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HackerNews.API.Models
{
    public class Post : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string domain { get; set; }
        public string description { get; set; }
        public int? points { get; set; }
        public string user { get; set; }
        public string time_ago { get; set; }
        public int comments_count { get; set; }
        public string type { get; set; }

        private bool _is_read;
        public bool is_read
        {
            get
            {
                return _is_read;
            }

            set
            {
                _is_read = value;

                OnPropertyChanged("is_read");

                OnPropertyChanged("title_foreground");
                OnPropertyChanged("description_foreground");
                OnPropertyChanged("comment_fill");
            }
        }

        public SolidColorBrush title_foreground
        {
            get
            {
                if (this.is_read == true)
                    return new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
                else
                    return new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            }
        }

        public SolidColorBrush url_foreground
        {
            get
            {
                if (this.is_read == true)
                    return new SolidColorBrush(Color.FromArgb(128, 255, 102, 0));
                else
                    return new SolidColorBrush(Color.FromArgb(192, 255, 102, 0));
            }
        }

        public string friendly_url
        {
            get
            {
                if (this.url != null)
                {
                    string friendlyUrl = this.url.ToLower().Replace("https://", "").Replace("http://", "");

                    if (friendlyUrl.Length > 36)
                        return friendlyUrl.Substring(0, 33) + "...";
                    else
                        if (friendlyUrl.EndsWith("/") == true)
                            return friendlyUrl.Substring(0, friendlyUrl.Length - 1);
                        else
                            return friendlyUrl;
                }
                else
                {
                    return null;
                }
            }
        }

        public SolidColorBrush description_foreground
        {
            get
            {
                if (this.is_read == true)
                    return new SolidColorBrush(Color.FromArgb(128, 130, 130, 130));
                else
                    return new SolidColorBrush(Color.FromArgb(255, 130, 130, 130));
            }
        }

        public SolidColorBrush comment_fill
        {
            get
            {
                if (this.is_read == true)
                    return new SolidColorBrush(Color.FromArgb(128, 187, 187, 187));
                else
                    return new SolidColorBrush(Color.FromArgb(255, 187, 187, 187));
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
