#region Using
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.ComponentModel;
using System.Runtime.Remoting.Messaging;
using Microsoft.Practices.Prism.Commands;

using ShowMonitor.Model;
#endregion

namespace ShowMonitor.ViewModel
{
    public class RSSFeedVM : INotifyPropertyChanged
    {
        #region Private Fields
        private DelegateCommand _addShowToDatabase;
        private ObservableCollection<Show> _rssList = new ObservableCollection<Show>();
        #endregion

        #region Public Properties

        public DelegateCommand AddDatabaseShow
        {
            get { return _addShowToDatabase ?? (_addShowToDatabase = new DelegateCommand(addDatabaseShow));  }
        }
        
        public Show SelectedRSSItem
        {
            get;
            set;
        }

        public ObservableCollection<Show> RSSList
        {
            get { return _rssList; }
            set
            {
                _rssList = value;
                OnPropertyChanged("RSSList");
            }
        }
        #endregion

        #region Constructors & Destructors
        public RSSFeedVM()
        {
             InvokeRSSFeed();
        }
        #endregion

        #region Private Methods
        private void addDatabaseShow()
        {
            ShowAdapter adapter = new ShowAdapter();
            adapter.Add(SelectedRSSItem);            
        }

        /// <summary>
        /// Cache the async callback from InvokeRSSFeed
        /// </summary>
        /// <param name="ar"></param>
        private void RssFeedCallBack(IAsyncResult ar)
        {
            AsyncResult result = (AsyncResult)ar;
            getRssFeedAsync caller = (getRssFeedAsync)result.AsyncDelegate;
            ObservableCollection<Show> Shows = new ObservableCollection<Show>(caller.EndInvoke(ar).OrderBy(o => o.ShowName));

            RSSList = Shows;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Invokes the RSS feed 
        /// </summary>
        public void InvokeRSSFeed()
        {
            RSSFeed ezFeed = new RSSFeed();
            getRssFeedAsync caller = new getRssFeedAsync(ezFeed.getRSSFeed);
            IAsyncResult result = caller.BeginInvoke(ShowMonitor.Properties.Settings.Default.RSSUrl, new AsyncCallback(this.RssFeedCallBack), null);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
