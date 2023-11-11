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
        public IActionResult GetPakyu(string term)
        {
            var products = _db.Products
                .Where(p => p.productName.Contains(term) && p.StockQuantity > 0)
                .Select(p => new { id = p.Id, text = p.productName, retailPrice = p.RetailPrice, img = p.ImageUrl })
                .ToList();

            return Json(products);
        }

        [HttpGet]
        public IActionResult GetRetailPrice(string productName)
        {
            var retailPrice = _db.Products
                .Where(p => p.productName == productName)
                .Select(p => p.RetailPrice)
                .FirstOrDefault();

            // Return the retail price as JSON
            return Json(retailPrice);
        }

        [HttpGet]
        public IActionResult CheckStock(string productName, int quantity)
        {
            var currentStock = _db.Products
                .Where(p => p.productName == productName)
                .Select(p => p.StockQuantity)
                .FirstOrDefault();

            // Check if the quantity is less than or equal to the current stock
            bool isStockAvailable = quantity <= currentStock;

            // Return the result as JSON
            return Json(isStockAvailable);
        }

        //      [HttpPost]
        //      public IActionResult SaveAndIssueReceipt ()
        //      {

        //          //get product of customer
        //	ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
        //		includeProperties: "Product");

        //	ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;

        //          //generate order id
        //	ShoppingCartVM.OrderHeader.ApplicationUserId = userId;





        //	//Capture customer payment
        //	ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
        //	ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusCompleted;

        //          //add to order header
        //	_unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
        //	_unitOfWork.Save();




        //		// Update product quantities here
        //		var product = _unitOfWork.Product.Get(p => p.Id == cart.ProductId);
        //		product.StockQuantity -= cart.Count;
        //		_unitOfWork.Product.Update(product);

        //              //Push to DB
        //		_unitOfWork.OrderDetail.Add(orderDetail);
        //		_unitOfWork.Save();


        //	return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
        //}

        //[HttpPost]
        //[ActionName("Summary")]
        //public IActionResult SummaryPOST()
        //{

        //    //Gets customer ID
        //    var claimsIdentity = (ClaimsIdentity)User.Identity;
        //    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        //    ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
        //        includeProperties: "Product");

        //    ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
        //    ShoppingCartVM.OrderHeader.ApplicationUserId = userId;


        //    ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

        //    foreach (var cart in ShoppingCartVM.ShoppingCartList)
        //    {
        //        cart.Price = GetPrice(cart);
        //        ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        //    }

        //    //Capture customer payment
        //    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
        //    ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;

        //    _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
        //    _unitOfWork.Save();

        //    foreach (var cart in ShoppingCartVM.ShoppingCartList)
        //    {
        //        OrderDetail orderDetail = new()
        //        {
        //            ProductId = cart.ProductId,
        //            OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
        //            Price = cart.Price,
        //            Count = cart.Count
        //        };

        //        // Update product quantities here
        //        var product = _unitOfWork.Product.Get(p => p.Id == cart.ProductId);
        //        product.StockQuantity -= cart.Count;
        //        _unitOfWork.Product.Update(product);


        //        _unitOfWork.OrderDetail.Add(orderDetail);
        //        _unitOfWork.Save();

        //    }

        //    return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
        //}

        //public IActionResult OrderConfirmation(int id)
        //{
        //    OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
        //    if (orderHeader.PaymentStatus != SD.PaymentStatusApproved)
        //    {
        //        _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusPending);
        //        _unitOfWork.Save();
        //    }

        //    HttpContext.Session.Clear();

        //    List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
        //        .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();


        //    // Remove shopping cart items and save changes
        //    _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
        //    _unitOfWork.Save();




        //    return View(id);
        //}
    }
}
