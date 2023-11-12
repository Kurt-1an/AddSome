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
		[BindProperty]
		public ShoppingCartVM ShoppingCartVM { get; set; }
		public POSController(AppDBContext context, IUnitOfWork unitOfWork)
		{
			_db = context;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index(int? id)
		{
			//Gets customer ID
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			ShoppingCartVM = new()
			{
				ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
				includeProperties: "Product"),
				OrderHeader = new()
			};

			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPrice(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}
			ViewBag.products = new SelectList(_db.Products, "Id", "ProductName");
			return View(ShoppingCartVM);
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
			_unitOfWork.Save();

			// Return the ShoppingCartID along with the success message
			var cartId = newShoppingCart.Id; // Assuming your ShoppingCart model has an Id property
			HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());

			return Json(new { success = true, message = "Product added to the cart successfully.", shoppingCartId = cartId });
		}



		//Delete Button
		public IActionResult Remove(int cartId)
		{
			var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId, tracked: true);

			if (cartFromDb == null)
			{
				return NotFound(); // Handle the case where the cart item is not found
			}

			HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart
			.GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);

			_unitOfWork.ShoppingCart.Remove(cartFromDb);
			_unitOfWork.Save();

			return Json(new { success = true, cartFromDb });
		}

		private double GetPrice(ShoppingCart shoppingCart)
		{
			return shoppingCart.Product.RetailPrice;
		}

		//// Place Order Button
		//[HttpPost]
		//[ActionName("Summary")]
		//public IActionResult SummaryPOST(double discount, double charge, double total)
		//{
		//	// Gets customer ID
		//	var claimsIdentity = (ClaimsIdentity)User.Identity;
		//	var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

		//	// Retrieves the shopping cart items for the user
		//	ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
		//		includeProperties: "Product");

		//	// Sets the order date and user ID
		//	ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
		//	ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

		//	// Calculates the total price for the order with discount and charge
		//	foreach (var cart in ShoppingCartVM.ShoppingCartList)
		//	{
		//		cart.Price = GetPrice(cart);
		//		ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
		//	}

		//	// Apply discount and charge
		//	ShoppingCartVM.OrderHeader.OrderTotal -= discount;
		//	ShoppingCartVM.OrderHeader.OrderTotal += charge;

		//	// Sets the payment and order status as pending
		//	ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
		//	ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusCompleted;

		//	// Adds the order header to the database
		//	_unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
		//	_unitOfWork.Save();

		//	// Adds the order details to the database and updates product quantities
		//	foreach (var cart in ShoppingCartVM.ShoppingCartList)
		//	{
		//		OrderDetail orderDetail = new()
		//		{
		//			ProductId = cart.ProductId,
		//			OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
		//			Price = cart.Price,
		//			Count = cart.Count,
		//			Charge = cart.Charge,
		//			Discount = cart.Discount
		//		};

		//		// Updates the product stock quantity
		//		var product = _unitOfWork.Product.Get(p => p.Id == cart.ProductId);
		//		product.StockQuantity -= cart.Count;
		//		_unitOfWork.Product.Update(product);

		//		_unitOfWork.OrderDetail.Add(orderDetail);
		//		_unitOfWork.Save();
		//	}

		//	// Redirects to the order confirmation page
		//	return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
		//}


		//public IActionResult OrderConfirmation(int id)
		//{
		//	OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
		//	if (orderHeader.PaymentStatus != SD.PaymentStatusApproved)
		//	{
		//		_unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusPending);
		//		_unitOfWork.Save();
		//	}

		//	HttpContext.Session.Clear();

		//	List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
		//		.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();


		//	// Remove shopping cart items and save changes
		//	_unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
		//	_unitOfWork.Save();

		//	return View(id);
		//}

		//Method 
	}
}
