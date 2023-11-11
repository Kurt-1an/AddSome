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

	public class PurchaseOrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDBContext _db;

        public PurchaseOrderController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, AppDBContext db)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _db = db;
        }

        //Retrieve the Data from Database
        public IActionResult Index()
        {
            List<PurchaseOrder> objPurchaseOrderList = _unitOfWork.PurchaseOrder.GetAll().ToList();
            return View(objPurchaseOrderList);
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

        //Post the Data to Database
        [HttpPost]
        public IActionResult Create(PurchaseOrder obj)
        {
            if (ModelState.IsValid)
            {
                //LOG START
                // Retrieve the user's role
                var user = _userManager.GetUserAsync(User).Result;

                // Create an audit log entry for the "Create" action with the user's role
                var auditLog = new AuditLog
                {
                    UserName = User.Identity.Name,
                    Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(), // Get the user's role
                    Action = "Create",
                    EntityName = "Purchase order",
                    EntityKey = "Create purchase order",
                    Changes = "New purchase order: " + obj.SupplierName,
                    Timestamp = DateTime.Now,
                    FormattedTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")
                };

                // Save the audit log entry to the database.
                _unitOfWork.AuditLog.Add(auditLog);
                _unitOfWork.Save();
                //LOG END

                _unitOfWork.PurchaseOrder.Add(obj);
                _unitOfWork.Save();
                TempData["toastAdd"] = "PurchaseOrder Added successfully";
                return RedirectToAction("Index", "PurchaseOrder");
            }

            return View();
        }



        //Edit
        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            PurchaseOrder? purchaseorderFromDb = _unitOfWork.PurchaseOrder.Get(u => u.Id == id);
            //PurchaseOrder? purchaseorderFromDb1 = _db.PurchaseOrder.FirstOrDefault(u=>u.Id==id);
            //PurchaseOrder? purchaseorderFromDb3 = _db.PurchaseOrder.Where(u => u.Id == id).FirstOrDefault();

            if (purchaseorderFromDb == null)
            {
                return NotFound();
            }

            return View(purchaseorderFromDb);

        }

        ////Post the Data to Database
        //[HttpPost]
        //public IActionResult Edit(PurchaseOrder obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.PurchaseOrder.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["toastUpd"] = "PurchaseOrder updated successfully";
        //        return RedirectToAction("Index");
        //    }

        //    return View();
        //}



        //DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            PurchaseOrder? purchaseorderFromDb = _unitOfWork.PurchaseOrder.Get(u => u.Id == id);

            if (purchaseorderFromDb == null)
            {
                return NotFound();
            }
            return View(purchaseorderFromDb);

        }

        //Post the Data to Database
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            PurchaseOrder? obj = _unitOfWork.PurchaseOrder.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            //LOG START
            // Retrieve the user's role
            var user = _userManager.GetUserAsync(User).Result;

            // Create an audit log entry for the "Create" action with the user's role
            var auditLog = new AuditLog
            {
                UserName = User.Identity.Name,
                Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(), // Get the user's role
                Action = "Remove",
                EntityName = "Purchase order",
                EntityKey = "Create purchase order",
                Changes = "Deleted purchase order: " + obj.SupplierName,
                Timestamp = DateTime.Now,
                FormattedTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")
            };

            // Save the audit log entry to the database.
            _unitOfWork.AuditLog.Add(auditLog);
            _unitOfWork.Save();
            //LOG END

            _unitOfWork.PurchaseOrder.Remove(obj);
            _unitOfWork.Save();
            TempData["toastDel"] = "PurchaseOrder deleted successfully";
            return RedirectToAction("Index", "PurchaseOrder");
        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<PurchaseOrder> objPurchaseOrderList = _unitOfWork.PurchaseOrder.GetAll().ToList();
            return Json(new { data = objPurchaseOrderList });
        }
        #endregion
    }
}
