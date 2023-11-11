using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Models
{
    public class StockTransfer
    {
        [Key]
        public int TransferId { get; set; }

        [Required]
        public int numberofProducts { get; set; }
        
        [Required]
        public int TransferredQty { get; set; }

        [Required]
        [DisplayName("Source Location")]
        public string sourceLocation { get; set; }

        [Required]
        [DisplayName("Destination Location")]
        public string destinationLocation { get; set; }

        public DateTime TransferDate { get; set; }

        public string Name { get; set; }
    }
}
