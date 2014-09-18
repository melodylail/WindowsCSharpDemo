using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Demo.Model;

namespace Demo.Interfaces
{
    interface ICustomerSummaryViewModel
    {
        CustomerList CustomerSummary { get; }
        Customers CurrentCustomer { get; }
        OrderList CurrentOrders { get; }
        ICommand NewOrderCommand { get; }
        ICommand ViewOrderCommand { get; }
        ICommand CustomerSelectionChanged { get; }
        ICommand OrderSelectionChanged { get; }
    }
}
