using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABC.Models
{
	public class AuditLog
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Action { get; set; } // E.g., Create, Update, Delete
		public string EntityName { get; set; }
		public string EntityKey { get; set; }
		public string Changes { get; set; }
        public DateTime Timestamp { get; set; }
        public string FormattedTime { get; set; }

        public string Role { get; set; }
    }

}
