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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Demo.ViewModels;

namespace Demo.Views
{
    /// <summary>
    /// Interaction logic for CustomerSummary.xaml
    /// </summary>
    public partial class CustomerSummary : UserControl
    {
        public CustomerSummary()
        {
            InitializeComponent();
        }

        private void CustomerSummary_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Height = 600;
            Application.Current.MainWindow.Width = 900;
        }

        private void OrderGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Todo: Change this to an interface reference (ICustomerSummaryViewModel)
            CustomerSummaryViewModel vm = DataContext as CustomerSummaryViewModel;

            vm.OrderSelectionChanged.Execute(e);
        }

        private void CustomerGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Todo: Change this to an interface reference (ICustomerSummaryViewModel)
            CustomerSummaryViewModel vm = DataContext as CustomerSummaryViewModel;

            vm.CustomerSelectionChanged.Execute(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
