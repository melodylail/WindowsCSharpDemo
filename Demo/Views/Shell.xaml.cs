using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Demo.Views
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    [Export]
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IRegionManager regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();

            if (regionManager != null)
            {
                IRegion region = regionManager.Regions["MainWindow"];
                if (region != null)
                {
                    region.Add(new Logon(), "LogonView");
                }
            }
        }
    }
}
