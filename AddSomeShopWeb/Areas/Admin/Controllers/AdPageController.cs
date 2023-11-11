using ABC.DataAccess.Data;
using ABC.Models.ViewModels;
using ABC.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AddSomeShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class AdPageController : Controller
    {
        private readonly AppDBContext _context;

        public AdPageController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult getOrderTotalData()
        {
            // Assuming you have a DbContext named _context
            var orderTotalData = _context.OrderHeaders
                .Select(o => o.OrderTotal)
                .ToList();

            return Json(orderTotalData);
        }

        /*
        public IActionResult GetTotalPurchaseData()
        {
             var purchasetotaldata = _context.Purchase
                .Select(p => p.PurchaseTotal)
                .ToList();

            return Json(purchasetotaldata);
        } */

        public int CountLowStockProducts()
        {
            int lowStockThreshold = 4; // Define your low stock threshold
            int lowStockProductCount = _context.Products.Count(p => p.StockQuantity < lowStockThreshold);
            return lowStockProductCount;
        }

        public int CountOutOfStockProducts()
        {
            int outOfStockProductCount = _context.Products.Count(p => p.StockQuantity == 0);
            return outOfStockProductCount;
        }

        public IActionResult Index()
        {
            int totalCustomers = _context.Customers.Count();
            int totalUnprocessedOrders = _context.OrderHeaders.Count(o => o.OrderStatus == "Unprocessed");
            int ordersOutForDelivery = _context.OrderHeaders.Count(o => o.OrderStatus == "Deliver Order");
            int canceledOrders = _context.OrderHeaders.Count(o => o.OrderStatus == "Cancelled");
            int totalProductsInStock = _context.Products.Count(p => p.StockQuantity > 0);
            int totalProdCategories = _context.Categories.Count();

            var viewModel = new DashboardViewModel
            {
                TotalCustomers = totalCustomers,
                TotalUnprocessedOrders = totalUnprocessedOrders,
                OrdersOutForDelivery = ordersOutForDelivery,
                CanceledOrders = canceledOrders,
                TotalProducts = totalProductsInStock,
                TotalCategories = totalProdCategories,
                LowStockProductCount = CountLowStockProducts(),
                OutofStockProductCount = CountOutOfStockProducts(),

            };
            return View(viewModel);
        }
    }
}
