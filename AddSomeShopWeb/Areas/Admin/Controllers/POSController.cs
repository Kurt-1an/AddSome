using ABC.DataAccess.Data;
using ABC.DataAccess.Repository.IRepository;
using ABC.Models;
using ABC.Models.ViewModels;
using ABC.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace AddSomeShopWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]

	public class POSController : Controller
	{

		private static List<OrderHeader> inProcessOrders = new List<OrderHeader>();
		private readonly AppDBContext _db;
		private readonly IUnitOfWork _unitOfWork;

		[BindProperty]
		public OrderVM OrderVM { get; set; }

		public POSController(AppDBContext context, IUnitOfWork unitOfWork)
		{
			_db = context;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index(int? id)
		{
			ViewBag.products = new SelectList(_db.Products, "Id", "ProductName");
			return View();
		}

		// Add a new action to fetch product data as JSON for Select2
		[HttpGet]
		public IActionResult GetProduct(string term)
		{
			var products = _db.Products
				.Where(p => p.productName.Contains(term) && p.StockQuantity > 0)
				.Select(p => new { id = p.Id, text = p.productName, retailPrice = p.RetailPrice, img = p.ImageUrl, qty = p.StockQuantity })
				.ToList();

			return Json(products);
		}

		[HttpGet]
		public IActionResult AddProduct(string productName, int quantity)
		{
			if (quantity < 0)
			{
				return Json(new { success = false, message = "Quantity cannot be negative." });
			}

			var product = _db.Products
				.Where(p => p.productName == productName && p.StockQuantity >= quantity)
				.Select(p => new { id = p.Id, text = p.productName, retailPrice = p.RetailPrice, img = p.ImageUrl })
				.FirstOrDefault();

			if (product != null)
			{
				return Json(new { success = true, product });
			}
			else
			{
				return Json(new { success = false, message = "Insufficient stock or product not found." });
			}
		}




		[HttpPost]
		[Authorize]
		public IActionResult AddToCart(int productId, int quantity)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			// Retrieve the product from the database
			Product productFromDb = _unitOfWork.Product.Get(u => u.Id == productId);

			// Check if the entered count is greater than the stock quantity
			if (quantity > productFromDb.StockQuantity)
			{
				return Json(new { success = false, message = "Cannot add more items than available in stock." });
			}

			ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == productId);

			// Create a new cart entry
			var newShoppingCart = new ShoppingCart
			{
				ProductId = productId,
				Count = quantity,
				ApplicationUserId = userId
			};

			// Add the new cart entry
			_unitOfWork.ShoppingCart.Add(newShoppingCart);
			HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());

			_unitOfWork.Save();

			return Json(new { success = true, message = "Product added to the cart successfully." });
		}


	}
}
