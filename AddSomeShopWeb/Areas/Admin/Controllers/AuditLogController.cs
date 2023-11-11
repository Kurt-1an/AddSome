using ABC.DataAccess.Repository.IRepository;
using ABC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AddSomeShopWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AuditLogController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public AuditLogController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
		{
			List<AuditLog> auditLogs = _unitOfWork.AuditLog.GetAll().ToList();
			return View(auditLogs);
		}



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<AuditLog> auditLogs = _unitOfWork.AuditLog.GetAll().ToList();
            return Json(new { data = auditLogs });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
