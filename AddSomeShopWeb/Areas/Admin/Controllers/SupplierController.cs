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
	[Authorize(Roles = SD.Role_Admin)]
	public class SupplierController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public SupplierController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        //Retrieve the Data from Database
        public IActionResult Index()
        {
            List<Supplier> objSupplierList = _unitOfWork.Supplier.GetAll().ToList();
            return View(objSupplierList);
        }


        //ADD
        public IActionResult Create()
        {
            return View();

        }

        //Post the Data to Database
        [HttpPost]
        public IActionResult Create(Supplier obj)
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
                    EntityName = "Supplier",
                    EntityKey = "Create supplier",
                    Changes = "New supplier created: " + obj.supplierCompanyName,
                    Timestamp = DateTime.Now,
                    FormattedTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")
                };

                // Save the audit log entry to the database.
                _unitOfWork.AuditLog.Add(auditLog);
                _unitOfWork.Save();
                //LOG END

                _unitOfWork.Supplier.Add(obj);
                _unitOfWork.Save();
                TempData["toastAdd"] = "Supplier Added successfully";
                return RedirectToAction("Index", "Supplier");
            }

            return View();
        }




        //Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Supplier? supplierFromDb = _unitOfWork.Supplier.Get(u => u.Id == id);
            //Supplier? supplierFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Supplier? supplierFromDb3 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (supplierFromDb == null)
            {
                return NotFound();
            }

            return View(supplierFromDb);

        }

        //Post the Data to Database
        [HttpPost]
        public IActionResult Edit(Supplier obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Supplier.Update(obj);
                _unitOfWork.Save();
                TempData["toastUpd"] = "Supplier updated successfully";
                return RedirectToAction("Index");
            }

            return View();
        }



        //DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Supplier? productFromDb = _unitOfWork.Supplier.Get(u => u.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);

        }

        //Post the Data to Database
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Supplier? obj = _unitOfWork.Supplier.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Supplier.Remove(obj);
            _unitOfWork.Save();
            TempData["toastDel"] = "Supplier deleted successfully";
            return RedirectToAction("Index", "Supplier");
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Supplier> objSupplierList = _unitOfWork.Supplier.GetAll().ToList();
            return Json(new { data = objSupplierList });
        }
        #endregion
    }
}
