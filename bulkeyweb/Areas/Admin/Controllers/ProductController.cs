using bulkey.DataAccess.Repository.IRepository;
using bulkey.Models;
using bulkey.Models.ViewModels;
using bulkey.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bulkeyweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objproductList = _unitOfWork.Product.GetAll(includePropreties:"Category").ToList();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
                .GetAll().Select(u=> new SelectListItem
            {
                    Text = u.name,
                    Value = u.Id.ToString()
            });
            return View(objproductList);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList =  _unitOfWork.Category
               .GetAll().Select(u => new SelectListItem
               {
                   Text = u.name,
                   Value = u.Id.ToString()
               }),
                Product = new Product()
            };
            if(id==null || id == 0)
            {
                //Create View 
                return View(productVM);
            }
            else
            {
                //Update View
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM , IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;//Define path of 'wwwroot' folder
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        // Delete Old Image
                        var oldImgPath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }
                    using(var fileStrem = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStrem);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Product Added a Successfully";
                return RedirectToAction("Index");
            }
            else
            {

                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                  {
                      Text = u.name,
                      Value = u.Id.ToString()
                  });

                return View(productVM);
            }
          
        }
       
        //public IActionResult Delete(int? id)
        //{
        //    if(id==null || id==0)
        //    {
        //        return NotFound();
        //    }
        //    Product product1 = _unitOfWork.Product.Get(u=>u.Id==id);
        //    if(product1 == null)
        //    {
        //        return NotFound() ;
        //    }
        //    return View(product1);
        //}
        //[HttpPost]
        //public IActionResult Delete(Product obj)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Remove(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product Added a Successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objproductList = _unitOfWork.Product.GetAll(includePropreties: "Category").ToList();
            return Json(new { data = objproductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productTobeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productTobeDeleted == null)
            {
                return Json(new { success = false, message = "Error while Deleteing" });
            }
            var oldImgPath =
                Path.Combine(_webHostEnvironment.WebRootPath, productTobeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImgPath))
            {
                System.IO.File.Delete(oldImgPath);
            }

            _unitOfWork.Product.Remove(productTobeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully" });
        }
        #endregion


    }
}
