using ABC.DataAccess.Data;
using ABC.DataAccess.Repository.IRepository;
using ABC.Models;
using ABC.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AddSomeShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class StockTransferController : Controller
    {

        private readonly AppDBContext _db;

        public StockTransferController(AppDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        //ADD
        public IActionResult Create()
        {
            return View();

        }

        // Add a new action to fetch product data as JSON for Select2
        [HttpGet]
        public IActionResult GetProducts(string term)
        {
            var products = _db.Products
                .Where(p => p.productName.Contains(term))
                .Select(p => new { id = p.Id, text = p.productName, img = p.ImageUrl })
                .ToList();


            return Json(products);
        }

		[HttpGet]
		public IActionResult GetProductDetails(string productName)
		{
			var productDetails = _db.Products
				.Where(p => p.productName == productName)
				.Select(p => new { CostPrice = p.CostPrice, StockQuantity = p.StockQuantity })
				.FirstOrDefault();

			// Return the result as JSON
			return Json(productDetails);
		}
	}
}
