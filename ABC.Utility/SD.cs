using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Utility
{
	public static class SD
	{
		//Roles
		public const string Role_Customer = "Customer";
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";

		//Shipment Status
		public const string StatusPending = "Pending";
		public const string StatusApproved = "Approved";
		public const string StatusProcessing = "Processing";
		public const string StatusShipped = "Out for Delivery";
		public const string StatusCompleted = "Completed";

		public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";


        //Payment Status
        public const string PaymentStatusPending = "Pending"; //default 
		public const string PaymentStatusApproved = "Paid";


		public const string SessionCart = "SessionShoppingCart";
	}
}
