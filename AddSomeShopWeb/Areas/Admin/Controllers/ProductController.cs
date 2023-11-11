
using ABC.DataAccess.Data;
using ABC.DataAccess.Repository.IRepository;
using ABC.Models;
using ABC.Models.ViewModels;
using ABC.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AddSomeShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]

	public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        //Retrieve the Data from Database
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Supplier").ToList();
            return View(objProductList);
        }


        //ADD
        public IActionResult Upsert(int? id)
        {
            //Chooses the supplier company name column from DB
            ProductVM productVM = new()
            {
                SuppllierList = _unitOfWork.Supplier.GetAll().Select(u => new SelectListItem
                {
                    Text = u.supplierCompanyName,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                //Create
                return View(productVM);
            }
            else
            {
                //Update
                productVM.Product = _unitOfWork.Product.Get(u=>u.Id == id);
                return View(productVM);
            }


        }


        //Post the Data to Database
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"image\product");

                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl)) 
                    {
                        //delete old Image
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);    
                        }
                    }
                    //Upload Image
                    using (var fileStream = new FileStream (Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    //Ipload Image URl
                    productVM.Product.ImageUrl = @"\image\product\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);

                    //-- LOG START --//
                    var user = _userManager.GetUserAsync(User).Result;
                    var auditLog = new AuditLog
                    {
                        UserName = User.Identity.Name,
                        Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                        Action = "Create",
                        EntityName = "Product",
                        EntityKey = "Create Product",
                        Changes = "New Product created: " + productVM.Product.productName,
                        Timestamp = DateTime.Now,
                        FormattedTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")
                    };

                    _unitOfWork.AuditLog.Add(auditLog);
                    //-- LOG END --//
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    //-- LOG START --//
                    var user = _userManager.GetUserAsync(User).Result;
                    var auditLog = new AuditLog
                    {
                        UserName = User.Identity.Name,
                        Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                        Action = "Update",
                        EntityName = "Product",
                        EntityKey = "Update Product",
                        Changes = "Product updated: " + productVM.Product.productName,
                        Timestamp = DateTime.Now,
                        FormattedTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")
                    };

                    _unitOfWork.AuditLog.Add(auditLog);
                    //-- LOG END --//
                }


                _unitOfWork.Save();
                TempData["toastAdd"] = "Product Updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.SuppllierList = _unitOfWork.Supplier.GetAll().Select(u => new SelectListItem
                {
                    Text = u.supplierCompanyName,
                    Value = u.Id.ToString()
                });
                    return View(productVM);
            }
        }



        




        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Supplier").ToList();
            return Json(new {data = objProductList});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);

            if(productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting"});
            }

            // LOG START: Log the Delete action
            var user = _userManager.GetUserAsync(User).Result;
            var auditLogDelete = new AuditLog
            {
                UserName = User.Identity.Name,
                Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                Action = "Delete",
                EntityName = "Product",
                EntityKey = "Delete Product",
                Changes = "Product deleted: " + productToBeDeleted.productName, 
                Timestamp = DateTime.Now,
                FormattedTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")
            };

            _unitOfWork.AuditLog.Add(auditLogDelete);
            // LOG END

            var oldImagePath = 
            
            //delete old Image
            Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
