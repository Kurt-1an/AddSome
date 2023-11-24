using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }

        public string? ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser? ApplicationUser { get; set; }

        //public SalesChannel salesChannel { get; set; } 
        //branch name 
        //discount 
        //charge 

        public List<OrderDetail> OrderDetails { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public double OrderTotal { get; set; }

        public string? OrderStatus { get; set; }
        public string? PaymentStatus {  get; set; }
        public string? TrackingNumber { get; set; } 
        public string? Carrier { get; set; }


        [Required]
        public string? PhoneNumber {  get; set; }

        [Required]
        public string? StreetName { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? Province { get; set; }

		[Required]
		public string? Barangay { get; set; }

		[Required]
        public string? PostalCode { get; set; }

        [Required]
        public string? Name { get; set; }


    }
}
