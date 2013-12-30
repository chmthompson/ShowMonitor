#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;

using ShowMonitor.Model;
#endregion

namespace ShowMonitor.ViewModel
{
    public class SearchShowsVM : INotifyPropertyChanged
    {
        #region Private Fields
        private ObservableCollection<Show> _searchShowResults;
        private DelegateCommand _searchShowCommand, _addShowCommand;
        private string _searchForShow;
        #endregion

        #region Public Properties
        public DelegateCommand SearchShowCommand
        {
            get { return _searchShowCommand ?? (_searchShowCommand = new DelegateCommand(SearchShow)); }
        }

        public DelegateCommand AddShowCommand
        {
            get { return _addShowCommand ?? (_addShowCommand = new DelegateCommand(AddShow)); }
        }
        
        /// <summary>
        /// Show being search for set by View
        /// </summary>
        public string SearchForShow
        {
            get { return _searchForShow; }
            set
            {
                _searchForShow = value;
                OnPropertyChanged("SearchForShow");
            }
        }

        public Show SelectedSearchResultItem
        {
            get;
            set;
        }

        public ObservableCollection<Show> SearchShowResults
        {
            get { return _searchShowResults ?? (_searchShowResults = new ObservableCollection<Show>()); }
            set
            {
                _searchShowResults = value;
                OnPropertyChanged("SearchShowResults");
            }
        }
        #endregion

        #region Constructors / Destructors
        private void SearchShow()
        {
            //showResults.Items.Clear();
            //searchSeriesAsync caller = new searchSeriesAsync(TVDBShow.SearchSeries);
            //IAsyncResult result = caller.BeginInvoke(this.SearchForShow, new AsyncCallback(searchShowCallBack), null);
            RSSFeed feed = new RSSFeed();
            string url = String.Format("http://ezrss.it/search/index.php?show_name={0}&date=&quality=&release_group=&mode=rss", SearchForShow);
            SearchShowResults = feed.getRSSFeed(url);
            SearchShowResults.OrderBy(o => o.EpisodeDate);
        }

        // SearchSeries Async callback
        private void searchShowCallBack(IAsyncResult ar)
        {
            try
            {
                //AsyncResult result = (AsyncResult)ar;
                //searchSeriesAsync caller = (searchSeriesAsync)result.AsyncDelegate;
                //SearchShowResults = caller.EndInvoke(ar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddShow()
        {
            try
            {
                ShowAdapter adapter = new ShowAdapter();
                adapter.Add(new Show()
                {
                    ShowName = SelectedSearchResultItem.ShowName,
                    Episode = SelectedSearchResultItem.Episode,
                    Season = SelectedSearchResultItem.Season,
                    Quality = SelectedSearchResultItem.Quality
                });
                OnPropertyChanged("SubscribedShowList"); // Is there a better way to do this?
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
