using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;

namespace ShowMonitor.ViewModel
{
    public class ConfigurationVM
    {
        private DelegateCommand _saveSettings;

        public DelegateCommand SaveSettingsCommand
        {
            get { return _saveSettings ?? (_saveSettings = new DelegateCommand(SaveSettings)); }
        }

        private void SaveSettings()
        {
            ShowMonitor.Properties.Settings.Default.Save();
        }
    }
}
