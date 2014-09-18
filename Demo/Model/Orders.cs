using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Demo.Model
{

	[DataContract]
	public class Orders
	{

		[DataMember (Name = "OrderID")]
        public String OrderID { get; set; }

		[DataMember (Name = "CustomerID")]
        public String CustomerID { get; set; }

		[DataMember (Name = "AmountTotal")]
		public Decimal AmountTotal { get; set;}

		[DataMember (Name = "EmployeeID")]
        public Int32 EmployeeID { get; set; }

		[DataMember (Name = "OrderDate")]
		public DateTime OrderDate { get; set;}

		[DataMember (Name = "RequiredDate")]
		public DateTime RequiredDate { get; set;}

		[DataMember (Name = "ShippedDate")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public DateTime ShippedDate { get; set;}

		[DataMember (Name = "ShipVia")]
		public String ShipVia { get; set;}

		[DataMember (Name = "Freight")]
		public String Freight { get; set;}

		[DataMember (Name = "ShipName")]
		public String ShipName { get; set;}

		[DataMember (Name = "ShipAddress")]
		public String ShipAddress { get; set;}

		[DataMember (Name = "ShipCity")]
		public String ShipCity { get; set;}

		[DataMember (Name = "ShipRegion")]
		public String ShipRegion { get; set;}

		[DataMember (Name = "ShipPostalCode")]
		public String ShipPostalCode { get; set;}

		[DataMember (Name = "ShipCountry")]
		public String ShipCountry { get; set;}

	}

}