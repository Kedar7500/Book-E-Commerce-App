using Book.DataAccess.Data;
using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Book.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book_E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository category)
        {
            categoryRepository = category;
        }
        public IActionResult Index()
        {
            List<Category> categoryList = categoryRepository.GetAll().ToList();
            return View(categoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            // adding custom validation error message
            //if(category.Name == category.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("Name","Display order cannot be excatly match with name ");
            //}


            if (ModelState.IsValid)
            {
                categoryRepository.Add(category);
                categoryRepository.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? cat = categoryRepository.Get(c => c.Id == id); // use linq operation 
            // dbContext.Categories.Find(id) => find will work on primary key 
            // dbContext.Categories.Where(c => c.Id == id).FirstOrDefault(); 


            if (cat == null)
            {
                return NotFound();
            }

            return View(cat);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {

            if (ModelState.IsValid)
            {
                categoryRepository.Update(category);
                categoryRepository.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = categoryRepository.Get(u => u.Id == id);

            return View(category);

        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = categoryRepository.Get(u => u.Id == id);

            categoryRepository.Remove(category);
            categoryRepository.Save();
            TempData["success"] = "Category Deleted Successfully";

            return RedirectToAction("Index");
        }
    }
}
