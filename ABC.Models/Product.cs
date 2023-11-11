using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ABC.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

        //Product Information

        //Product Image
        [DisplayName("For Inventory")]
        [ValidateNever]
        public string ImageUrl { get; set; }
        public long Barcode { get; set; }
		public string SKU { get; set; }


        [Required]
        [DisplayName("Product Name")]
        public string productName { get; set; }
		public string Category { get; set; }

        [DisplayName("Sub Category")]
        public string subCategory { get; set; }
        public string Brand { get; set; }
        public string Warehouse { get; set; }
        public string Description { get; set; }


		//Pricing
		[DisplayName("Cost Price")]
        public float CostPrice { get; set; }

		[Required]
		[DisplayName("Retail Price")]
		public float RetailPrice { get; set; }


		//Inventory
		[Required]
		[DisplayName("Stock Quantity")]
		public int StockQuantity { get; set; }

        [Required]
        [DisplayName("Min. Stock Quantity")]
		public int MinimumStockQuantity { get; set; }


        //Warranty
        [Required]
        public string Type { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public string Provider { get; set; }



        //Prodect Specification
        [DisplayName("Specification #1")]
        public string SpecOne { get; set; }
        [DisplayName("Specification #2")]
        public string SpecTwo { get; set; }
        [DisplayName("Specification #3")]
        public string SpecThree { get; set; }


        //Additional Notes
        [DisplayName("Additional Notes")]
        public string? addNotes { get; set; }



        //Foreign Key Relation of Supplier
        [DisplayName("Supplier")]
        public int SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        [ValidateNever]
        public Supplier Supplier { get; set; }
        
    }
}

