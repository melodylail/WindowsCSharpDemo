using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model
{

	[DataContract]
	public class OrderDetails
	{

		[DataMember (Name = "OrderID")]
		public String OrderID { get; set;}

		[DataMember (Name = "ProductID")]
        public String ProductID { get; set; }

		[DataMember (Name = "UnitPrice")]
        public Decimal UnitPrice { get; set; }

		[DataMember (Name = "Quantity")]
        public Int32 Quantity { get; set; }

		[DataMember (Name = "Amount")]
        public Decimal Amount { get; set; }

		[DataMember (Name = "Discount")]
        public Decimal Discount { get; set; }

	}

}