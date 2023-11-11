using ABC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DataAccess.Repository.IRepository
{
	public interface ISupplierRepository : IRepository<Supplier> 
	{
		void Update(Supplier obj);
	}
}
