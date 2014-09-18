using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Regions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using Demo.Model;
using Demo.Interfaces;
using Demo.Infrastructure;
using Demo.Views;

namespace Demo.ViewModels
{
    /// <summary>
    /// Class Demo.ViewModels.CustomerSummaryViewModel
    /// <para>
    /// This class implements the link between the data model and the CustomerSummary screen  <see cref="Demo.Views.CustomerSummary"/>.</para>
    /// </summary>
    public class CustomerSummaryViewModel : BindableBase, ICustomerSummaryViewModel
    {
        #region Public Inteface (implements ICustomerSummaryViewModel)
        #region Public Properties
        #region Public Property: CustomerSummary
        private CustomerList customerSummary;
        public CustomerList CustomerSummary
        {
            get { return customerSummary; }
        }
        #endregion
        #region Public Property: CurrentCustomer
        private Customers currentCustomer;
        public Customers CurrentCustomer
        {
            get { return currentCustomer; }
            set { SetProperty(ref currentCustomer, value); }
        }
        #endregion
        #region Public Property: CurrentOrders
        private OrderList currentOrders;
        public OrderList CurrentOrders
        {
            get { return currentOrders; }
            set { SetProperty(ref currentOrders, value); }
        }
        #endregion
        #endregion
        #region Public Commands
        #region Public Command: NewOrderCommand
        public ICommand NewOrderCommand { get; private set; }
        private void NewOrderCommandExecute()
        {
            Debug.WriteLine("Test command executed.");
        }
        #endregion
        #region Public Command: ViewOrderCommand
        public ICommand ViewOrderCommand { get; private set; }

        private void ViewOrderCommandExecute()
        {
            Debug.WriteLine("View Order Command executed.");

            IRegionManager regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();

            if (regionManager != null)
            {
                IRegion region = regionManager.Regions["MainWindow"];
                if (region != null)
                {
                    OrderDetailsView odv = region.GetView("OrderDetailsView") as OrderDetailsView;
                    OrderDetailsViewModel odvm = odv.DataContext as OrderDetailsViewModel;
                    odvm.CustomerName = CurrentCustomer.CompanyName;
                    odvm.Address = CurrentCustomer.Address;
                    odvm.City = CurrentCustomer.City;
                    odvm.PostalCode = CurrentCustomer.PostalCode;
                    odvm.Phone = CurrentCustomer.Phone;

                    odvm.CurrentOrder = SelectedOrder;

                    odvm.OrderDate = SelectedOrder.OrderDate;
                    odvm.RequiredDate = SelectedOrder.RequiredDate;
                    odvm.ShippedDate = SelectedOrder.ShippedDate;
                    region.Deactivate(region.GetView("CustomerSummaryView"));
                    region.Activate(odv);
                }
            }
        }
        #endregion
        #region Public Command: ViewInvoiceCommand
        public ICommand ViewInvoiceCommand { get; private set; }
        private void ViewInvoiceCommandExecute()
        {
            IRegionManager regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();

            if (regionManager != null)
            {
                IRegion region = regionManager.Regions["MainWindow"];
                if (region != null)
                {
                    InvoiceView invoice = region.GetView("InvoiceView") as InvoiceView;
                    InvoiceViewModel invoiceViewModel = invoice.DataContext as InvoiceViewModel;

                    invoiceViewModel.Customer = CurrentCustomer;
                    invoiceViewModel.Order = SelectedOrder;

                    region.Deactivate(region.GetView("CustomerSummaryView"));
                    region.Activate(invoice);
                }
            }
        }
        #endregion
        #region Public Command: CustomerSelectionChanged
        public ICommand CustomerSelectionChanged { get; private set; }
        private void CustomerSelectionChanged_Raised(SelectionChangedEventArgs args)
        {
            string name = "";

            name = (args.AddedItems[0] as Customers).CompanyName;

            Debug.WriteLine("New Selection: Name = {0}", name, name);

            var newCurrent = from c in CustomerSummary where c.CompanyName == name select c;

            if (newCurrent.Count<Customers>() == 1)
            {
                CurrentCustomer = newCurrent.First<Customers>();

                orderListTask = GetOrdersSummaryAsync(CurrentCustomer);
                orderListTask.Wait();
                CurrentOrders = orderListTask.Result;
            }
        }
        #endregion
        #region Public Command: OrderSelectionChanged
        public ICommand OrderSelectionChanged { get; private set; }
        private void OrderSelectionChanged_Execute(SelectionChangedEventArgs args)
        {
            if (args.AddedItems.Count > 0)
            {
                SelectedOrder = args.AddedItems[0] as Orders;
            }

            Debug.WriteLine("Ran OrderSelectionChanged_Execute");
        }
        #endregion
        #endregion
        #endregion
        #region Public Default Constructor: CustomerSummaryViewModel()
        public CustomerSummaryViewModel()
        {
            customerSummary = new CustomerList();

            //customerSummary = Application.Current.Resources["TestCustomers"] as CustomerList;
            customerListTask = GetCustomerSummaryAsync();
            customerListTask.Wait();
            customerSummary =  customerListTask.Result;

            Debug.WriteLine("CustomerSummary: count = {0}", CustomerSummary.Count);
            foreach (Customers customer in CustomerSummary)
            {
                Debug.WriteLine("\t{0} {1}", customer.CompanyName, customer.Address);
            }

            CustomerSelectionChanged = new DelegateCommand<SelectionChangedEventArgs>(CustomerSelectionChanged_Raised);
            OrderSelectionChanged = new DelegateCommand<SelectionChangedEventArgs>(OrderSelectionChanged_Execute);
            CurrentCustomer = new Customers();
            CurrentOrders = new OrderList();

            NewOrderCommand = new DelegateCommand(NewOrderCommandExecute);
            ViewOrderCommand = new DelegateCommand(ViewOrderCommandExecute);
            ViewInvoiceCommand = new DelegateCommand(ViewInvoiceCommandExecute);
        }
        #endregion
        #region Private Members
        #region Private Methods
        #region Private Method: async Task<CustomerList> GetCustomerSummaryAsync()
        private async Task<CustomerList> GetCustomerSummaryAsync()
        {
            CustomerList customerList = new CustomerList();
            String requestTemplate = Application.Current.Resources["GetCustomers"] as String;           
            String requesturl = String.Format(requestTemplate, BaseURLHelper.BaseURL );

            using (HttpClient client = new HttpClient())
            {
                //do
                //{
                    try
                    {
                        string response = await HttpHelper.GetAsync(client, requesturl);
                        CustomerList newCustomers = JsonConvert.DeserializeObject<CustomerList>(response);
                        customerList.AddRange(newCustomers.Take<Customers>(20));
                        String nextBatch = (string)JArray.Parse(response).Last["@metadata"]["next_batch"];
                        if (requesturl != null)
                        {
                            //requesturl = requesturl + "&auth=" + apiKey;
                        }
                        Debug.WriteLine("Just a break point.");
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Just a break point.");
                    }
              //  } while (requesturl != null);
            }

            return customerList;
        }
        #endregion
        #region Private Method: async Task<OrderList> GetOrdersSummaryAsync(Customers customer)
        private async Task<OrderList> GetOrdersSummaryAsync(Customers customer)
        {
            OrderList orderList = new OrderList();
            String requestTemplate = Application.Current.Resources["GetOrders"] as String;
            

            String requesturl = String.Format(requestTemplate, BaseURLHelper.BaseURL, customer.CustomerID);
            string response = null;
            using (HttpClient client = new HttpClient())
            {
                
                    try
                    {
                        response = await HttpHelper.GetAsync(client, requesturl);
                        OrderList newOrders = JsonConvert.DeserializeObject<OrderList>(response);
                        orderList.AddRange(newOrders.Take<Orders>(20));
                        String nextBatch = (string)JArray.Parse(response).Last["@metadata"]["next_batch"];
                        if (requesturl != null)
                        {
                           // requesturl = requesturl + "&auth=" + apiKey;
                        }
                        Debug.WriteLine("Just a break point.");
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Just a break point.");
                        requesturl = null;
                    }
               

            }
            //orderList.RemoveAll(o => o.CustomerID != customer.CustomerID);
            return orderList;
        }
        #endregion
        #endregion
        #region Private Data Members
        private Orders SelectedOrder = new Orders();
        private Task<CustomerList> customerListTask;
        private Task<OrderList> orderListTask;
        #endregion
        #endregion
    }
}
