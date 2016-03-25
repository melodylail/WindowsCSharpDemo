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
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using Demo.Views;
using Demo.Infrastructure;
using System.Windows;

namespace Demo.ViewModels
{
    class LogonViewModel : BindableBase
    {
        public LogonViewModel()
        {
            LogonCommand = new DelegateCommand<PasswordBox>(LogonCommand_Execute, LogonCommand_CanExecute);
        }
        private string _logon;
        public string Logon
        {
            get { return _logon; }
            set { SetProperty(ref _logon, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string _baseurl;
        public string BaseURL
        {
            get { return _baseurl; }
            set { SetProperty(ref _baseurl, value); }
        }

        public ICommand LogonCommand { get; private set; }
        private void LogonCommand_Execute(PasswordBox pBox)
        {
            Debug.WriteLine("Logon command received: User = {0} Pass = {1}", Logon, pBox.Password);

            String apiKey = "";
            BaseURLHelper.BaseURL = BaseURL;
            try
            {
                Task<String> authTask = HttpHelper.Authenticate(Logon, pBox.Password);
                authTask.Wait();
                apiKey = authTask.Result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception caught during authentication: {0}", e.Message, null);
            }

            if (apiKey == "")
            {
                string messageBoxText = "Authentication failed.";
                string caption = "CA Live API Creator Demo";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;

                MessageBox.Show(messageBoxText, caption, button, icon);
                pBox.Clear();
                return;
            }
            Debug.WriteLine("ApiKey {0} returned for User {1} Pass {2} for baseURL {3}", apiKey, Logon, pBox.Password, _baseurl);

            ApiHelper.ApiKey = apiKey;
           

            IRegionManager regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();

            if (regionManager != null)
            {
                IRegion region = regionManager.Regions["MainWindow"];
                if (region != null)
                {
                    region.Remove(region.GetView("LogonView"));
                    region.Add(new CustomerSummary(), "CustomerSummaryView");
                    region.Add(new OrderDetailsView(), "OrderDetailsView");
                    region.Add(new InvoiceView(), "InvoiceView");
                    region.Deactivate(region.GetView("OrderDetailsView"));
                    region.Deactivate(region.GetView("InvoiceView"));
                }
            }

        }
        private bool LogonCommand_CanExecute(PasswordBox pBox)
        {
            bool result = true;

            return result;
        }
    }
}
