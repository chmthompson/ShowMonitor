#region Using
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Remoting.Messaging;
using Microsoft.Practices.Prism.Commands;
using System.Windows;
using ShowMonitor.Model;
#endregion

namespace ShowMonitor.ViewModel
{
    public class SubscribedShowsVM : INotifyPropertyChanged
    {
        #region Private Fields
        private DelegateCommand _removeShowCommand;
        #endregion

        #region Public Properties

        /// <summary>
        /// List of subscribed shows in database
        /// </summary>
        public ObservableCollection<Show> SubscribedShowList
        {
            get {
                try
                {
                    return (new ShowAdapter()).GetSubscribedShows();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public DelegateCommand RemoveShowCommand
        {
            get { return _removeShowCommand ?? (_removeShowCommand = new DelegateCommand(RemoveShow)); }
        }

        public Show SelectedSubscribedShowItem
        {
            get;
            set;
        } 
        #endregion

        #region Constructors & Destructors
        /// <summary>
        /// Default constructor that sets the show list to the current shows in the database
        /// </summary>
        public SubscribedShowsVM()
        {
        }

        public SubscribedShowsVM(String showName)
        {
        }
        #endregion

        #region Private Methods

        private void RemoveShow()
        {
            try
            {
                ShowAdapter adapter = new ShowAdapter();
                adapter.Delete(SelectedSubscribedShowItem.ShowName);
                OnPropertyChanged("SubscribedShowList");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        } 
    }
        #endregion
}
