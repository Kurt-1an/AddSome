using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ABC.Models
{
    public class Content
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("About")]
        public string? About { get; set; }


        [DisplayName("Privacy Policy")]
        public string? Privacy { get; set; }


        [DisplayName("Vision & Mission")]
        public string? Vision { get; set; }
    }
}
