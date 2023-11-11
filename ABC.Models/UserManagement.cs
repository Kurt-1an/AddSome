using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABC.Models
{
    public class UserManagement
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(30)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }


        [DisplayName("Contact Number")]
        public long ContactNumber { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Access Level")]
        public string AccessLevel { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;



        [NotMapped] // Exclude from database
        public string FormattedID => $"ABC-{Id}";
    }
}
