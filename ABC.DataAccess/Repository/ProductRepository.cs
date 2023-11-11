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
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		private AppDBContext _db;
		public ProductRepository(AppDBContext db) : base(db)
		{
			_db = db;
		}


		public void Update(Product obj)
		{
			var objFromDB = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
			if (objFromDB != null)
			{
				objFromDB.Barcode = obj.Barcode;
                objFromDB.SKU = obj.SKU;
                objFromDB.productName = obj.productName;
                objFromDB.Category = obj.Category;
				objFromDB.subCategory = obj.subCategory;
                objFromDB.Brand = obj.Brand;
                objFromDB.Warehouse = obj.Warehouse;
                objFromDB.Description = obj.Description;
                objFromDB.CostPrice = obj.CostPrice;
                objFromDB.RetailPrice = obj.RetailPrice;
                objFromDB.StockQuantity = obj.StockQuantity;
                objFromDB.MinimumStockQuantity = obj.MinimumStockQuantity;
                objFromDB.Type = obj.Type;
                objFromDB.Duration = obj.Duration;
                objFromDB.Provider = obj.Provider;
                objFromDB.SpecOne = obj.SpecOne;
                objFromDB.SpecTwo = obj.SpecTwo;
                objFromDB.SpecThree = obj.SpecThree;
                objFromDB.addNotes = obj.addNotes;
                objFromDB.SupplierId = obj.SupplierId;

                if (obj.ImageUrl != null)
                {
                    objFromDB.ImageUrl = obj.ImageUrl;
                }
            }
		}
	}
}
