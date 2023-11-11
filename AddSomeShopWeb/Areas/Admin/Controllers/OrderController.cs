using ABC.DataAccess.Repository.IRepository;
using ABC.Models;
using ABC.Models.ViewModels;
using ABC.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AddSomeShopWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}

        public IActionResult Index()
		{
			return View();
		}

        public IActionResult Details(int orderId)
        {
            OrderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
            };
            return View(OrderVM);
        }


        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult UpdateOrderDetail(int orderId)
        {
			//Retrieve OrderHeader from DB
			var orderHeaderFromb = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);

            orderHeaderFromb.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromb.StreetName = OrderVM.OrderHeader.StreetName;
            orderHeaderFromb.City = OrderVM.OrderHeader.City;
            orderHeaderFromb.Province = OrderVM.OrderHeader.Province;
            orderHeaderFromb.Barangay = OrderVM.OrderHeader.Barangay;
            orderHeaderFromb.PostalCode = OrderVM.OrderHeader.PostalCode;

            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
            {
                orderHeaderFromb.Carrier = OrderVM.OrderHeader.Carrier;
            }

            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
            {
                orderHeaderFromb.Carrier = OrderVM.OrderHeader.TrackingNumber;
            }

            _unitOfWork.OrderHeader.Update(orderHeaderFromb);
            _unitOfWork.Save();

            TempData["toastAdd"] = "Order Details Updated Succesfully";

            return RedirectToAction(nameof(Details), new {orderId = orderHeaderFromb.Id});
        }


        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing(int orderId)
        {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusProcessing);
            _unitOfWork.Save();
            TempData["toastAdd"] = "Order Details Updated Succesfully";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }


		[HttpPost]
		[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
		public IActionResult ShipOrder(int orderId)
		{
            //Retrieve OrderHeader from DB
			var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);

			orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
			orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
			orderHeader.OrderStatus = SD.StatusShipped;
			orderHeader.ShippingDate = DateTime.Now;
            orderHeader.OrderStatus = SD.StatusShipped;

			_unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
			TempData["toastAdd"] = "Order Out for Delivery!";
			return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
		}


		[HttpPost]
		[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
		public IActionResult CancelOrder(int orderId)
        {
			//Retrieve OrderHeader from DB
			var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);

			_unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);
			_unitOfWork.Save();
			TempData["toastAdd"] = "Order Cancelled Succesfully";
			return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
		}




		[ActionName("Details")]
        [HttpPost]
		public IActionResult Details_PAID(int orderId)
        {
			//Retrieve OrderHeader from DB
			var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);

			_unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCompleted, SD.PaymentStatusApproved);
			_unitOfWork.Save();
			TempData["toastAdd"] = "Order Delivered Succesfully";
			return RedirectToAction(nameof(Index), new { orderId = OrderVM.OrderHeader.Id });

		}


		#region API CALLS
		[HttpGet]
		public IActionResult GetAll(string status)
		{
            IEnumerable<OrderHeader> objOrderHeaders;

            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == userId, includeProperties: "ApplicationUser");
            }

            switch (status)
            {
                
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus== SD.StatusProcessing);
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
