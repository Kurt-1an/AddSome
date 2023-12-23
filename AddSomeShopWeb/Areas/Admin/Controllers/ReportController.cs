using ABC.DataAccess.Data;
using ABC.Models;
using System.Linq;
using ABC.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AddSomeShopWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class ReportController : Controller
	{
		private readonly AppDBContext _db;

		public ReportController(AppDBContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			// Sales Revenue
			double salesRevenue = _db.OrderHeaders
				.Where(order => order.PaymentStatus == "Paid")
				.Sum(order => order.OrderTotal);

			// Calculate Profit
			double totalProfit = _db.OrderHeaders
				.Where(order => order.PaymentStatus == "Paid")
				.SelectMany(order => _db.OrderDetails.Where(detail => detail.OrderHeaderId == order.Id))
				.Sum(detail => detail.Product != null ?
					(detail.Price - detail.Product.CostPrice) * detail.Count : 0);

			// Number of Items Sold
			int numberOfItemsSold = _db.OrderHeaders
				.Where(order => order.PaymentStatus == "Paid")
				.SelectMany(order => _db.OrderDetails.Where(detail => detail.OrderHeaderId == order.Id))
				.Sum(detail => detail.Count);

			// Total Cost Price
			double totalCostPrice = _db.Products.Sum(product => product.CostPrice);

			var bestSellingProducts = _db.OrderDetails
		.Include(detail => detail.Product)
		.GroupBy(detail => detail.Product)
		.OrderByDescending(group => group.Sum(detail => detail.Count))
		.Select(group => group.Key)
		.ToList();

			ViewBag.SalesRevenue = salesRevenue;
			ViewBag.NumberOfItemsSold = numberOfItemsSold;
			ViewBag.Profit = totalProfit;
			ViewBag.TotalCostPrice = totalCostPrice;
			ViewBag.OrderDetails = _db.OrderDetails.ToList();

			return View(bestSellingProducts);
		}

		public IActionResult InvIndex()
		{
			int totalProducts = _db.Products.Count();
			ViewBag.TotalProducts = totalProducts;

			int totalCategories = _db.Categories.Count();
			ViewBag.TotalCategories = totalCategories; // Corrected ViewBag key

			int totalPurOrders = _db.PurchaseOrders.Count();
			ViewBag.TotalPurOrders = totalPurOrders;

			int instockProducts = _db.Products.Count(p => p.StockQuantity > 0);
			ViewBag.InstockProducts = instockProducts;

			int lowStockThreshold = 5;

			int lowStockProducts = _db.Products.Count(p => p.StockQuantity > 0 && p.StockQuantity <= lowStockThreshold); // Corrected condition
			ViewBag.LowStockProducts = lowStockProducts;

			int outOfStockProducts = _db.Products.Count(p => p.StockQuantity == 0);
			ViewBag.OutOfStockProducts = outOfStockProducts;

			return View();
		}
	}
}
