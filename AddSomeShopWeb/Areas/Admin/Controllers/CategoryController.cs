using ABC.DataAccess.Data;
using ABC.DataAccess.Repository.IRepository;
using ABC.Models;
using ABC.Utility;
using AddSomeShopWeb.Areas.CustomerArea.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using System.Security.Claims;

namespace AddSomeShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]

	public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CategoryController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }


        //Retrieve the Data from Database
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }


        //ADD
        public IActionResult Create()
        {
            return View();

        }

        //Post the Data to Database
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //Validation
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name.");
            }

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
                    EntityName = "Category",
                    EntityKey = "Create category",
                    Changes = "New category created: " + obj.Name,
                    Timestamp = DateTime.Now,
                    FormattedTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")
                };

                // Save the audit log entry to the database.
                _unitOfWork.AuditLog.Add(auditLog);
				_unitOfWork.Save();
                //LOG END

				_unitOfWork.Category.Add(obj);
				_unitOfWork.Save();

				TempData["toastAdd"] = "Category Added successfully";
                return RedirectToAction("Index", "Category");
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
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);


            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);

        }

        //Post the Data to Database
        [HttpPost]
        public IActionResult Edit(Category obj)
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
                    Action = "Edit",
                    EntityName = "Category",
                    EntityKey = "Edit category",
                    Changes = "Edited category: " + obj.Name,
                    Timestamp = DateTime.Now,
                    FormattedTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")

                };

                // Save the audit log entry to the database.
                _unitOfWork.AuditLog.Add(auditLog);
                _unitOfWork.Save();
                //LOG END

                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["toastUpd"] = "Category updated successfully";
                return RedirectToAction("Index", "Category");
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
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);

        }

        //Post the Data to Database
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);

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
                EntityName = "Category",
                EntityKey = "Remove category",
                Changes = "Deleted category: " + obj.Name,
                Timestamp = DateTime.Now,
                FormattedTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")
            };

            // Save the audit log entry to the database.
            _unitOfWork.AuditLog.Add(auditLog);
            _unitOfWork.Save();
            //LOG END

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["toastDel"] = "Category deleted successfully";
            return RedirectToAction("Index", "Category");
        }
    }
}
