using bulkey.DataAccess.Data;
using bulkey.DataAccess.Repository.IRepository;
using bulkey.Models;
using bulkey.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bulkeyweb.Areas.Admin.Controllers
{
    [Area("Admin")]
   // [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.name == obj.displayOder.ToString())
            {
                ModelState.AddModelError("name", "Both are not A sam value");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);//Add() is a inbuilt method of entitifream work this method through we insert data into the database
                _unitOfWork.Save();
                TempData["success"] = "Category created a Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Category categoryFromdb = _db.Categories.Find(id);
            Category? categoryFromdb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromdb == null)
            {
                return NotFound();
            }

            return View(categoryFromdb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);//Update() is a inbuilt method of entitifream work this method through we Edit data into the database
                _unitOfWork.Save();
                TempData["success"] = "Category Updated a Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Category categoryFromdb = _db.Categories.Find(id);
            Category? categoryFromdb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromdb == null)
            {
                return NotFound();
            }

            return View(categoryFromdb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted a Successfully";
            return RedirectToAction("Index");
        }

    }
}
