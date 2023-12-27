using bulkey.DataAccess.Repository.IRepository;
using bulkey.Models;
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
    public class CompanyController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
                .GetAll().Select(u=> new SelectListItem
            {
                    Text = u.name,
                    Value = u.Id.ToString()
            });
            return View(objCompanyList);
        }
        public IActionResult Upsert(int? id)
        {
          
            if(id==null || id == 0)
            {
                //Create View 
                return View(new Company());
            }
            else
            {
                //Update View
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {
                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }
                _unitOfWork.Save();
                TempData["success"] = "Company Added a Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                {

                    return View(CompanyObj);
                }

            }
        }
            //public IActionResult Delete(int? id)
            //{
            //    if(id==null || id==0)
            //    {
            //        return NotFound();
            //    }
            //    Company Company1 = _unitOfWork.Company.Get(u=>u.Id==id);
            //    if(Company1 == null)
            //    {
            //        return NotFound() ;
            //    }
            //    return View(Company1);
            //}
            //[HttpPost]
            //public IActionResult Delete(Company obj)
            //{
            //    if(ModelState.IsValid)
            //    {
            //        _unitOfWork.Company.Remove(obj);
            //        _unitOfWork.Save();
            //        TempData["success"] = "Company Added a Successfully";
            //        return RedirectToAction("Index");
            //    }
            //    return View();
            //}

            #region API CALLS

            [HttpGet]
            public IActionResult GetAll()
            {
                List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
                return Json(new { data = objCompanyList });
            }

            [HttpDelete]
            public IActionResult Delete(int? id)
            {
                var CompanyTobeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
                if (CompanyTobeDeleted == null)
                {
                    return Json(new { success = false, message = "Error while Deleteing" });
                }

                _unitOfWork.Company.Remove(CompanyTobeDeleted);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Deleted Successfully" });
            }
            #endregion
        

    }
}
