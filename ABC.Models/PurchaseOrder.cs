using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Models
{
    public class PurchaseOrder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("Supplier Name")]
        public string SupplierName { get; set; }

        [DisplayName("Shipment Preference")]
        public string ShipmentPreference { get; set; }

        [DisplayName("Location for Delivery")]
        public string LocationDelivery { get; set; }

        [DisplayName("Payment Term")]
        public string PaymentTerm { get; set; }

        [DisplayName("Expected Delivery Date")]
        [DataType(DataType.Date)]
        public DateTime ExpectedDeliveryDate { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Contact Number")]
        public long ContactNumber { get; set; }

        [DisplayName("Additional Note")]
        public string AdditionalNote { get; set; }
        [DisplayName("Purchased Products")]

        public List<PurchaseOrderItem> PurchasedProducts { get; set; }
    }

    public class PurchaseOrderItem
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [DisplayName("Cost")]
        public double Cost { get; set; }

        [DisplayName("Quantity")]
        public int Quantity { get; set; }
    }
}







