using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model
{

	[DataContract]
	public class Customers
	{
		[DataMember (Name = "CustomerID")]
		public String CustomerID { get; set;}

		[DataMember (Name = "CompanyName")]
		public String CompanyName { get; set;}

		[DataMember (Name = "Balance")]
		public Decimal Balance { get; set;}

		[DataMember (Name = "CreditLimit")]
        public Decimal CreditLimit { get; set; }

		[DataMember (Name = "ContactName")]
		public String ContactName { get; set;}

		[DataMember (Name = "ContactTitle")]
		public String ContactTitle { get; set;}

		[DataMember (Name = "Address")]
		public String Address { get; set;}

		[DataMember (Name = "City")]
		public String City { get; set;}

		[DataMember (Name = "Region")]
		public String Region { get; set;}

		[DataMember (Name = "PostalCode")]
		public String PostalCode { get; set;}

		[DataMember (Name = "Country")]
		public String Country { get; set;}

		[DataMember (Name = "Phone")]
		public String Phone { get; set;}

		[DataMember (Name = "Fax")]
		public String Fax { get; set;}
    }

}