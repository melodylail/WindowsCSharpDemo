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
using Demo.Model;
using Demo.ViewModels;

namespace Demo.Views
{
    /// <summary>
    /// Interaction logic for InvoiceView.xaml
    /// </summary>
    public partial class InvoiceView : UserControl
    {
        public InvoiceView()
        {
            InitializeComponent();
        }
        private void InvoiceView_Loaded(object sender, RoutedEventArgs e)
        {
            Customers customer = (DataContext as InvoiceViewModel).Customer;
            Orders order = (DataContext as InvoiceViewModel).Order;

            ShipToBox.ABName.Content = order.ShipName;
            ShipToBox.ABAddress.Content = order.ShipAddress;
            ShipToBox.ABCity.Content = order.ShipCity;
            ShipToBox.ABPostalCode.Content = order.ShipPostalCode;
            ShipToBox.ABCountry.Content = order.ShipCountry;

            BillToBox.ABName.Content = customer.CompanyName;
            BillToBox.ABAddress.Content = customer.Address;
            BillToBox.ABCity.Content = customer.City;
            BillToBox.ABPostalCode.Content = customer.PostalCode;
            BillToBox.ABCountry.Content = customer.Country;

           // Total.Content = order.AmountTotal.ToString("$#,##0.00;$(#,##0.00)");
            InvoiceViewModel vm = DataContext as InvoiceViewModel;

            vm.Loaded.Execute(e);

        }
    }
}
