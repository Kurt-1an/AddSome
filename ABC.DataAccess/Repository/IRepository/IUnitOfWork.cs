using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork 
	{
		ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IPurchaseOrderRepository PurchaseOrder { get; }
        ISupplierRepository Supplier { get; }
        ICustomerRepository Customer { get; }
        IUserManagementRepository UserManagement { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }
        IAuditLogRepository AuditLog { get; }
        IContentRepository Content { get; }

        void Save();
	}
}
