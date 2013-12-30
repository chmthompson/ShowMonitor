using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using ShowMonitor.ViewModel;

namespace ShowMonitor.View
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class ConfigurationView : UserControl
    {
        public ConfigurationView(ConfigurationVM vm)
        {
            this.DataContext = vm;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderSelect.FolderSelectDialog fsd = new FolderSelect.FolderSelectDialog();

            if (fsd.ShowDialog(IntPtr.Zero))
            {
                torrentDirectory.Content = fsd.FileName;
            }
        }
    }
}
