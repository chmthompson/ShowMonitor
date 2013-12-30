using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using ShowMonitor.ViewModel;

namespace ShowMonitor.View
{
    /// <summary>
    /// Interaction logic for DownloadShowView.xaml
    /// </summary>
    public partial class DownloadShowView : UserControl
    {
        public DownloadShowView(DownloadShowVM vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}
