using ABC.DataAccess.Data;
using ABC.DataAccess.Repository.IRepository;
using ABC.Models;
using ABC.Models.ViewModels;
using ABC.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace AddSomeShopWeb.Areas.CustomerArea.Controllers
{
    [Area("CustomerArea")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDBContext _db;
        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, AppDBContext db)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public IActionResult Index()
        {
            // Retrieve the list of categories
            var categories = _unitOfWork.Category.GetAll();

            // Retrieve the list of products
            var products = _unitOfWork.Product.GetAll();

            // Retrieve best-selling products
            var bestSellingProducts = _db.OrderDetails
                .Include(detail => detail.Product)
                .GroupBy(detail => detail.Product)
                .OrderByDescending(group => group.Sum(detail => detail.Count)) // Use Quantity or any appropriate property for the product count
                .Select(group => group.Key)
                .ToList();


            // Include best-selling products in the view model
            var viewModel = new CategoryProductVM
            {
                Categories = categories,
                Products = products,
                BestSellingProducts = bestSellingProducts
            };

            return View(viewModel);
        }



        public IActionResult Shop(string searchString, string categoryFilter)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart,
                    _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count());
            }

            // Retrieve the list of categories
            var categories = _unitOfWork.Category.GetAll(); 
            ViewBag.Categories = categories;

            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Supplier,Category");

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                productList = productList.Where(p =>
                    p.productName.ToLower().Contains(searchString) || p.Brand.ToLower().Contains(searchString));
            }



            if (!string.IsNullOrEmpty(categoryFilter))
            {
                int categoryId;
                if (int.TryParse(categoryFilter, out categoryId))
                {
                    productList = productList.Where(p => p.CategoryId == categoryId);
                }
            }

            return View(productList);
        }




        //Details Button from Shop
        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Supplier"),
                Count = 1,
                ProductId = productId
            };
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            // Retrieve the product from the database
            Product productFromDb = _unitOfWork.Product.Get(u => u.Id == shoppingCart.ProductId);

            // Check if the entered count is greater than the stock quantity
            if (shoppingCart.Count > productFromDb.StockQuantity)
            {
                TempData["toastDel"] = "Cannot add more items than available in stock.";
                return RedirectToAction(nameof(Shop));
            }

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);

            if (cartFromDb != null)
            {
                // Shopping Cart exists
                cartFromDb.Count += shoppingCart.Count;

                // Check again after updating the count
                if (cartFromDb.Count > productFromDb.StockQuantity)
                {
                    TempData["toastError"] = "Cannot add more items than available in stock.";
                    return RedirectToAction(nameof(Shop));
                }

                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            else
            {
                // Add cart
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }

            _unitOfWork.Save();
            TempData["toastAdd"] = "Product Added to cart successfully";
            return RedirectToAction(nameof(Shop));
        }



        public IActionResult Privacy()
        {
            List<Content> objContentList = _unitOfWork.Content.GetAll().ToList();
            return View(objContentList);
        }

        public IActionResult Vision()
        {
            List<Content> objContentList = _unitOfWork.Content.GetAll().ToList();
            return View(objContentList);
        }

        public IActionResult About()
        {
            List<Content> objContentList = _unitOfWork.Content.GetAll().ToList();
            return View(objContentList);
        }

        public IActionResult ManageOrder()
        {
            return View();
        }

        public IActionResult ViewOrder(int orderId)
        {
            OrderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
            };
            return View(OrderVM);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            IEnumerable<OrderHeader> objOrderHeaders;

            objOrderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == userId, includeProperties: "ApplicationUser");

            switch (status)
            {
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusProcessing);
                    break;
                case "shipped":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusCompleted);
                    break;
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }

            return Json(new { data = objOrderHeaders });
        }
        #endregion
    }
}