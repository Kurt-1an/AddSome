using ABC.DataAccess.Data;
using ABC.Models;
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
        private readonly AppDBContext _db;

        public AdPageController(AppDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            int totalcustomers = _db.Customers.Count();
            int unprocessedOrders = _db.OrderHeaders.Count(o => o.OrderStatus == "To Process");
            int ordersoutfordelivery = _db.OrderHeaders.Count(o => o.OrderStatus == "Deliver Order");
            int cancelledOrders = _db.OrderHeaders.Count(o => o.OrderStatus == "Cancelled");
            int totalProductsInstock = _db.Products.Count(p => p.StockQuantity > 0);
            int lowstockproducts = _db.Products.Count(p => p.StockQuantity <= 4);
            int outofstockproducts = _db.Products.Count(p => p.StockQuantity == 0);
            int totalProdcategories = _db.Categories.Count();
            double salesRevenue = _db.OrderHeaders.Count(o => o.PaymentStatus == "Paid");
            double totalCost = _db.Products.Sum(p => p.CostPrice);


            ViewBag.TotalCustomers = totalcustomers;
            ViewBag.unprocessedOrders = unprocessedOrders;
            ViewBag.ordersoutfordelivery = ordersoutfordelivery;
            ViewBag.cancelledOrders = cancelledOrders;
            ViewBag.totalProductsInstock = totalProductsInstock;
            ViewBag.lowstockproducts = lowstockproducts;
            ViewBag.outofstockproducts = outofstockproducts;
            ViewBag.totalProdcategories = totalProdcategories;
            ViewBag.SalesRevenue = salesRevenue;
            ViewBag.TotalCost = totalCost;


            var bestSellingProducts = _db.OrderDetails
        .Include(detail => detail.Product)
        .GroupBy(detail => detail.Product)
        .OrderByDescending(group => group.Sum(detail => detail.Count))
        .Select(group => group.Key)
        .ToList();

            ViewBag.OrderDetails = _db.OrderDetails.ToList();


            return View(bestSellingProducts);
        }
    }
}

