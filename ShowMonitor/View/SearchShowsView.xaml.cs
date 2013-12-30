using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.Remoting.Messaging;
using ShowMonitor.Model;
using ShowMonitor.ViewModel;

namespace ShowMonitor.View
{
    /// <summary>
    /// Interaction logic for ShowSubscriptions.xaml
    /// </summary>
    public partial class SearchShowsView : UserControl
    {
        public SearchShowsView(SearchShowsVM vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}
