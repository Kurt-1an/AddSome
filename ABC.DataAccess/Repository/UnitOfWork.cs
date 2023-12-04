using ABC.DataAccess.Data;
using ABC.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
	{

		private AppDBContext _db;
		public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IPurchaseOrderRepository PurchaseOrder { get; private set; }
        public ISupplierRepository Supplier { get; private set; }
		public ICustomerRepository Customer { get; private set; }
		public IUserManagementRepository UserManagement { get; private set; }
		public IShoppingCartRepository ShoppingCart { get; private set; }
		public IApplicationUserRepository ApplicationUser { get; private set; }
		public IOrderHeaderRepository OrderHeader { get; private set; }
		public IOrderDetailRepository OrderDetail { get; private set; }
		public IAuditLogRepository AuditLog { get; private set; }
        public IContentRepository Content { get; private set; }





        public UnitOfWork(AppDBContext db)
		{
			_db = db;
			Category = new CategoryRepository(_db);
			Product = new ProductRepository(_db);
            PurchaseOrder = new PurchaseOrderRepository(_db);
            Supplier = new SupplierRepository(_db);
			Customer = new CustomerRepository(_db);	
			UserManagement = new UserManagementRepository(_db);
			ShoppingCart = new ShoppingCartRepository(_db);
			ApplicationUser = new ApplicationUserRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
			AuditLog = new AuditLogRepository(_db);
			Content = new ContentRepository(_db);
        }


        public void Save()
		{
			_db.SaveChanges();
		}
	}
}
