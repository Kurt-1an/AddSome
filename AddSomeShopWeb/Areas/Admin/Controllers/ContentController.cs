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
    public class ContentController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public ContentController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

		//Retrieve the Data from Database
		public IActionResult Index()
		{
			List<Content> objContentList = _unitOfWork.Content.GetAll().ToList();
			return View(objContentList);
		}

		public IActionResult Upsert(int? id)
        {
            Content obj;

            if (id == null || id == 0)
            {
                // Create
                obj = new Content();
            }
            else
            {
                // Update
                obj = _unitOfWork.Content.Get(u => u.Id == id);
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Upsert(Content obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    // Create
                    _unitOfWork.Content.Add(obj);

					// LOG START
					var user = _userManager.GetUserAsync(User).Result;
					var auditLog = new AuditLog
					{
						UserName = User.Identity.Name,
						Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
						Action = "Create",
						EntityName = "Content",
						EntityKey = "Create Content",
						Changes = "New content created",
						Timestamp = DateTime.Now,
						FormattedTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")
					};
					_unitOfWork.AuditLog.Add(auditLog);
					// LOG END
				}
				else
                {
                    // Update
                    _unitOfWork.Content.Update(obj);

					// LOG START
					var user = _userManager.GetUserAsync(User).Result;
					var auditLog = new AuditLog
					{
						UserName = User.Identity.Name,
						Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
						Action = "Update",
						EntityName = "Content",
						EntityKey = "Update Content",
						Changes = "Content updated",
						Timestamp = DateTime.Now,
						FormattedTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")
					};
					_unitOfWork.AuditLog.Add(auditLog);
                    // LOG END
                }


				_unitOfWork.Save();
                TempData["toastAdd"] = "Content Updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }
        }


    }
}
