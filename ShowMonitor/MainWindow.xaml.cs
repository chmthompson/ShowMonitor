#region Using
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ShowMonitor.View;
using ShowMonitor.ViewModel;
using Microsoft.Practices.Prism.Commands;
#endregion

namespace ShowMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Fields
        private Dictionary<string, UserControl> _viewDictionary = new Dictionary<string, UserControl>();
        private DelegateCommand<string> _viewCommand;
        #endregion

        #region Constructor & Destructors
       /// <summary>
       /// Add Views to viewDictionary and sets datacontext
       /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _viewDictionary.Add("RSSFeedView", new RSSFeedView(new RSSFeedVM()));
            _viewDictionary.Add("SubscriptionsView", new ShowSubscriptionsView(new SubscribedShowsVM()));
            _viewDictionary.Add("SearchView", new SearchShowsView(new SearchShowsVM()));
            _viewDictionary.Add("ConfigurationView", new ConfigurationView(new ConfigurationVM()));
            _viewDictionary.Add("DownloadShowsView", new DownloadShowView(new DownloadShowVM()));
            this.DataContext = this;
        }
        #endregion

        public DelegateCommand<string> ViewCommand
        {
            get { return _viewCommand ?? (_viewCommand = new DelegateCommand<string>(x => SwitchView(_viewDictionary[x]))); }
        }

        private void SwitchView(UserControl view) {
            view.Width = mainStackPanel.ActualWidth;
            view.Height = mainStackPanel.ActualHeight;

            mainStackPanel.Children.Clear();
            mainStackPanel.Children.Add(view);
            view.InvalidateVisual();
        }
    }
}
