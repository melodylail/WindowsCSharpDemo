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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Demo.Model;

namespace Demo.Views
{
    /// <summary>
    /// Interaction logic for InvoiceAddressBox.xaml
    /// </summary>
    public partial class InvoiceAddressBox : UserControl
    {
        public InvoiceAddressBox()
        {
            InitializeComponent();
        }

        private String title;
        public String Title {
            get { return title; }
            set
            {
                title = value;
                UpdateTitle(value);
            }
        }
        private void UpdateTitle(string text)
        {
            TitleLabel.Content = text;
        }
        private String addressName;
        public String AddressName
        {
            get { return addressName; }
            set
            {
                addressName = value;
                ABName.Content = value;
            }
        }
    }
}
