using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        [DisplayName("Contact Number")]
        public long ContactNumber { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("Apt./Suite/Unit No.")]
        public string ApSuUn { get; set; }

        [DisplayName("Street/Subdivision")]
        public string StreetorSubd { get; set; }

        [DisplayName("Barangay")]
        public string Barangay { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("Province")]
        public string Province { get; set; }

        [DisplayName("Zip Code")]
        public int ZipCode { get; set; }


    }
}
