using ABC.DataAccess.Data;
using ABC.DataAccess.Repository.IRepository;
using ABC.Models;
using ABC.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddSomeShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
	public class UserManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserManagementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Retrieve the Data from Database
        public IActionResult Index()
        {
            List<UserManagement> objUserManagementList = _unitOfWork.UserManagement.GetAll().ToList();
            return View(objUserManagementList);
        }


        //ADD
        public IActionResult Create()
        {
            return View();

        }

        //Post the Data to Database
        [HttpPost]
        public IActionResult Create(UserManagement obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.UserManagement.Add(obj);
                _unitOfWork.Save();
                TempData["toastAdd"] = "UserManagement Added successfully";
                return RedirectToAction("Index", "UserManagement");
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
            UserManagement? userManagementFromDb = _unitOfWork.UserManagement.Get(u => u.Id == id);
            //UserManagement? userManagementFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //UserManagement? userManagementFromDb3 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (userManagementFromDb == null)
            {
                return NotFound();
            }

            return View(userManagementFromDb);

        }

        //Post the Data to Database
        [HttpPost]
        public IActionResult Edit(UserManagement obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.UserManagement.Update(obj);
                _unitOfWork.Save();
                TempData["toastUpd"] = "UserManagement updated successfully";
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
            UserManagement? userManagementFromDb = _unitOfWork.UserManagement.Get(u => u.Id == id);

            if (userManagementFromDb == null)
            {
                return NotFound();
            }
            return View(userManagementFromDb);

        }

        //Post the Data to Database
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            UserManagement? obj = _unitOfWork.UserManagement.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.UserManagement.Remove(obj);
            _unitOfWork.Save();
            TempData["toastDel"] = "UserManagement deleted successfully";
            return RedirectToAction("Index", "UserManagement");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<UserManagement> objUserManagementList = _unitOfWork.UserManagement.GetAll().ToList();
            return Json(new { data = objUserManagementList });
        }
        #endregion
    }
}
