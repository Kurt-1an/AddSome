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
	public class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
	{
		private AppDBContext _db;
		public PurchaseOrderRepository(AppDBContext db) : base(db)
		{
			_db = db;
		}


		public void Update(PurchaseOrder obj)
		{
			_db.PurchaseOrders.Update(obj);
		}
	}
}
