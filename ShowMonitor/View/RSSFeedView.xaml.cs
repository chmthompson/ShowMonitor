#region Using
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.Caching;
using System.Runtime.Remoting.Messaging;

using ShowMonitor.ViewModel;
using ShowMonitor.Model;
#endregion

namespace ShowMonitor.View
{
    /// <summary>
    /// Interaction logic for RSSFeedView.xaml
    /// </summary>
    public partial class RSSFeedView : UserControl
    {

        #region Default Constructors & Destructors
        public RSSFeedView(RSSFeedVM vm)
        {
            this.DataContext = vm;
            InitializeComponent();
        }
        #endregion
    }
}
