using ABC.DataAccess.Data;
using ABC.DataAccess.Repository.IRepository;
using ABC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DataAccess.Repository
{
	public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
	{
		private AppDBContext _db;
		public AuditLogRepository(AppDBContext db) : base(db)			
		{
			_db = db;
		}


		public void Update(AuditLog obj)
		{
			_db.AuditLogs.Update(obj);
		}
	}
}
