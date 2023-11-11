using ABC.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddSomeShopWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]

	public class InventoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //ADD
        public IActionResult Brand()
        {
            return View();

        }

        public IActionResult ConfirmPurchaseOrder()
        {
            return View();
        }

        public IActionResult AddPurchaseOrder()
        {
            return View();
        }

        public IActionResult PurchaseOrder()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();

        }
    }
}
