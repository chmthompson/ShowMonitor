using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using ShowMonitor.Model;
namespace ShowMonitor.ViewModel
{
    public class DownloadShowVM : INotifyPropertyChanged
    {
        private DelegateCommand _downloadShowsCommand;
        private string _taskNotification;
        private readonly BackgroundWorker worker = new BackgroundWorker();

        public string TaskNotification
        {
            get { return _taskNotification; }
            set
            {
                _taskNotification += value + "\n";
                OnPropertyChanged("TaskNotification");            }
        }
        public DelegateCommand DownloadShowsCommand
        {
            get { return _downloadShowsCommand ?? (_downloadShowsCommand = new DelegateCommand(DownloadShows)); }
        }

        public DownloadShowVM()
        {
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
        }

        private void DownloadShows()
        {
            TaskNotification = "Beginning Feed check....";
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Downloads the torrent files if a database show is in the feed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            RSSFeed feed = new RSSFeed();
            ShowAdapter adapter = new ShowAdapter();
            ObservableCollection<Show> rssFeed, databaseShows = new ObservableCollection<Show>();

            worker.ReportProgress(0, "Retreiving Feed.....");
            rssFeed = feed.getRSSFeed(ShowMonitor.Properties.Settings.Default.RSSUrl);
            worker.ReportProgress(0, "Grabbing shows from database....");
            databaseShows = adapter.GetSubscribedShows();
            worker.ReportProgress(0, "Cross checking database shows against feed....");

            foreach (var show in databaseShows)
            {
                worker.ReportProgress(0,"Checking " + show.ToString() + "....");
                if (rssFeed.Any(p => p.ShowName == show.ShowName))
                {
                    // Show rssShow = (from rssEntry in rssFeed where rssEntry.ShowName.Equals(show.ShowName) select rssEntry).First();
                    worker.ReportProgress(0, "*****Downloading " + show.ShowName);
                    //rssShow.Download();
                }
            }
        }

        /// <summary>
        /// Updates Task Notification, which the view binds to
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            TaskNotification = e.UserState as String;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TaskNotification = "Finished Database Check";
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
