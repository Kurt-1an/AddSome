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

	public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Retrieve the Data from Database
        public IActionResult Index()
        {
            List<Customer> objCustomerList = _unitOfWork.Customer.GetAll().ToList();
            return View(objCustomerList);
        }


        //ADD
        public IActionResult Create()
        {
            return View();

        }

        //Post the Data to Database
        [HttpPost]
        public IActionResult Create(Customer obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Customer.Add(obj);
                _unitOfWork.Save();
                TempData["toastAdd"] = "Customer Added successfully";
                return RedirectToAction("Index", "Customer");
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
            Customer? admCustomerFromDb = _unitOfWork.Customer.Get(u => u.Id == id);
            //Customer? admCustomerFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Customer? admCustomerFromDb3 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (admCustomerFromDb == null)
            {
                return NotFound();
            }

            return View(admCustomerFromDb);

        }

        //Post the Data to Database
        [HttpPost]
        public IActionResult Edit(Customer obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Customer.Update(obj);
                _unitOfWork.Save();
                TempData["toastUpd"] = "Customer updated successfully";
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
            Customer? admCustomerFromDb = _unitOfWork.Customer.Get(u => u.Id == id);

            if (admCustomerFromDb == null)
            {
                return NotFound();
            }
            return View(admCustomerFromDb);

        }

        //Post the Data to Database
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Customer? obj = _unitOfWork.Customer.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Customer.Remove(obj);
            _unitOfWork.Save();
            TempData["toastDel"] = "Customer deleted successfully";
            return RedirectToAction("Index", "Customer");
        }
    }
}
