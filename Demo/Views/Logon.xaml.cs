using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Demo.Infrastructure;
using Demo.ViewModels;

namespace Demo.Views
{
    /// <summary>
    /// Interaction logic for Logon.xaml
    /// </summary>
    [ViewExport(RegionName = "MainRegion")]
    public partial class Logon : UserControl
    {
        public Logon()
        {
            InitializeComponent();
        }

        private void Logon_Loaded(object sender, RoutedEventArgs e)
        {
            LoginBox.Text = "demo";
            passBox.Password = "Password1";
            BaseURLHelper.BaseURL = Application.Current.Resources["BaseURL"] as String;
            BaseURL.Text = BaseURLHelper.BaseURL;
            FocusManager.SetFocusedElement(Application.Current.MainWindow, LoginBox);
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LogonViewModel vm = DataContext as LogonViewModel;

            vm.Logon = LoginBox.Text;
            vm.BaseURL = BaseURL.Text;
            vm.LogonCommand.Execute(passBox);
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
