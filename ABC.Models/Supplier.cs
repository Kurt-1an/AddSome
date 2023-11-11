using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ABC.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }


        //SUPPLIER LIST - NEW SUPPLIER
        //Supplier Information
        [Required]
        [DisplayName("Company Name")]
        public string supplierCompanyName { get; set; }

        [Required]
      
        [DisplayName("Contact Number")]
        public long supplierContactNumber { get; set; }

        [Required]
        [DisplayName("Email Address")]
        public string supplierEmail { get; set; }

        [Required]
     
        [DisplayName("Status")]
        public string supplierStatus { get; set; }

        [Required]
        [DisplayName("Description")]
        public string supplierDescription { get; set; }

        [DisplayName("Lot & Blk no.")]
        public string supplierLotBlk { get; set; }

        [Required]
        [DisplayName("Street/Subdivision")]
        public string supplierStreetSubdv { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("Barangay")]
        public string supplierBarangay { get; set; }

   
        [DisplayName("City")]
        public string supplierCity { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("Province")]
        public string supplierProvince { get; set; }

        [Required]
        [DisplayName("Zip Code")]
        public int supplierZipCode { get; set; }

        [DisplayName("Additional Note")]
        public string supplierNote { get; set; }
    }
}
