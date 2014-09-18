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
    public class OrderDetailsViewModel : BindableBase
    {
        #region Public Command BackButton
        public ICommand BackButton { get; private set; }
        private void BackButtonExecute()
        {
            Debug.WriteLine("Back page requested.");

            IRegionManager regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();

            if (regionManager != null)
            {
                IRegion region = regionManager.Regions["MainWindow"];
                if (region != null)
                {
                    region.Activate(region.GetView("CustomerSummaryView"));
                    region.Deactivate(region.GetView("OrderDetailsView"));
                }
            }
        }
        #endregion
        #region Public Inner Class DisplayLine
        public class DisplayLine
        {
            public String ProductName { get; private set; }
            public Decimal UnitPrice { get; private set; }
            public Int32 Quantity { get; private set; }
            public Decimal Amount { get; private set; }
            public Decimal Discount { get; private set; }

            public DisplayLine(OrderDetails details)
            {
                ProductName = ResolveProductName(details);
                UnitPrice = details.UnitPrice;
                Quantity = details.Quantity;
                Amount = details.Amount;
                Discount = details.Discount;
            }
            Task<Products> productNameTask;
            private async Task<Products> LookUpProduct(String productID)
            {
                Products product = null;

                String requestTemplate = Application.Current.Resources["GetProduct"] as String;

                String requesturl = String.Format(requestTemplate, BaseURLHelper.BaseURL, productID);
                string response = null;
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Todo: Need List<DisplayLine>!
                        response = await HttpHelper.GetAsync(client, requesturl);
                        List<Products> productsFound = JsonConvert.DeserializeObject<List<Products>>(response);
                        product = productsFound[0];
                        Debug.WriteLine("Just a break point.");
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Just a break point.");
                        requesturl = null;
                    }
                }
                return product;
            }
            private String ResolveProductName(OrderDetails details)
            {
                String productName = "";
                if (ProductNameCache.ContainsKey(details.ProductID))
                {
                    productName = ProductNameCache[details.ProductID];
                }
                else
                {
                    Products product = null;

                    productNameTask = LookUpProduct(details.ProductID);
                    productNameTask.Wait();
                    product = productNameTask.Result;
                    productName = product.ProductName;
                    ProductNameCache[details.ProductID] = productName;
                }
                return productName;
            }
        }
        #endregion
        private static Dictionary<String, String> ProductNameCache = new Dictionary<string, string>();
        #region Public Property: DisplayLineList
        private List<DisplayLine> displayLineList = null;
        public List<DisplayLine> DisplayLineList
        {
            get { return displayLineList; }
            set { SetProperty(ref displayLineList, value); }
        }
        #endregion
        #region Public Command: Loaded
        public ICommand Loaded { get; private set; }
        private void Loaded_Executed()
        {
            orderDetailsTask = GetOrderDetailsAsync(CurrentOrder.OrderID);
            orderDetailsTask.Wait();
            Details = orderDetailsTask.Result;

        }
        #endregion

        #region Private Async Method: async Task<List<DisplayLine>> GetOrderDetailsAsync(String orderID)
        private Task<List<DisplayLine>> orderDetailsTask;
        private async Task<List<DisplayLine>> GetOrderDetailsAsync(String orderID)
        {
            List<DisplayLine> details = new List<DisplayLine>();

            String requestTemplate = Application.Current.Resources["GetDetails"] as String;
           
            String requesturl = String.Format(requestTemplate,BaseURLHelper.BaseURL, orderID);
            string response = null;
            using (HttpClient client = new HttpClient())
            {
                
                    try
                    {
                        // Todo: Need List<DisplayLine>!
                        response = await HttpHelper.GetAsync(client, requesturl);
                        OrderDetailsList newDetails = JsonConvert.DeserializeObject<OrderDetailsList>(response);
                        newDetails.Take<OrderDetails>(20);
                        foreach (var detail in newDetails)
                        {
                            details.Add(new DisplayLine(detail));
                        }
                        string nextBatch = (string)JArray.Parse(response).Last["@metadata"]["next_batch"];
                        if (requesturl != null)
                        {
                            //requesturl = requesturl + "&auth=" + apiKey;
                        }
                        Debug.WriteLine("Just a break point.");
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Just a break point.");
                        requesturl = null;
                    }
               
            }
            return details;
        }
        #endregion
        #region Public Property: CustomerName
        private string customerName;
        public string CustomerName
        {
            get { return customerName; }
            set { SetProperty(ref customerName, value); }
        }
        #endregion
        private string address;
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }
        private string city;
        public string City
        {
            get { return city; }
            set { SetProperty(ref city, value); }
        }
        private string postalCode;
        public string PostalCode
        {
            get { return postalCode; }
            set { SetProperty(ref postalCode, value); }
        }
        private string phone;
        public string Phone
        {
            get { return phone; }
            set { SetProperty(ref phone, value); }
        }
        private DateTime orderDate;
        public DateTime OrderDate
        {
            get { return orderDate; }
            set { SetProperty(ref orderDate, value); }
        }
        private DateTime requiredDate;
        public DateTime RequiredDate
        {
            get { return requiredDate; }
            set { SetProperty(ref requiredDate, value); }
        }
        private DateTime shippedDate;
        public DateTime ShippedDate
        {
            get { return shippedDate; }
            set { SetProperty(ref shippedDate, value); }
        }
        private Orders currentOrder;
        public Orders CurrentOrder
        {
            get { return currentOrder; }
            set { SetProperty(ref currentOrder, value); }
        }
        public OrderDetailsViewModel()
        {
            BackButton = new DelegateCommand(BackButtonExecute);
            Loaded = new DelegateCommand(Loaded_Executed);
        }
        private List<DisplayLine> orderDetails;
        public List<DisplayLine> Details
        {
            get { return orderDetails; }
            set { SetProperty(ref orderDetails, value); }
        }
    }
}
