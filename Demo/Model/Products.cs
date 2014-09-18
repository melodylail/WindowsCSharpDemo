using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model
{

	[DataContract]
	public class Products
	{

		[DataMember (Name = "ProductID")]
		public String ProductID { get; set;}

		[DataMember (Name = "ProductName")]
		public String ProductName { get; set;}

		[DataMember (Name = "SupplierID")]
        public Int32 SupplierID { get; set; }

		[DataMember (Name = "CategoryID")]
        public Int32 CategoryID { get; set; }

		[DataMember (Name = "QuantityPerUnit")]
        public String QuantityPerUnit { get; set; }

		[DataMember (Name = "UnitPrice")]
		public Decimal UnitPrice { get; set;}

		[DataMember (Name = "UnitsInStock")]
        public Int32 UnitsInStock { get; set; }

		[DataMember (Name = "UnitsOnOrder")]
		public String UnitsOnOrder { get; set;}

		[DataMember (Name = "ReorderLevel")]
        public Int32 ReorderLevel { get; set; }

		[DataMember (Name = "Discontinued")]
		public Boolean Discontinued { get; set;}

	}

}