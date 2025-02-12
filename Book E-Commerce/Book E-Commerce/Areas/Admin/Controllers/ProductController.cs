using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Book.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Book_E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            List<Product> products = productRepository.GetAll().ToList();


            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {

            // retrieve category list here using projections in EF Core
            //IEnumerable<SelectListItem> categoryList = categoryRepository.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.Name,
            //    Value = u.Id.ToString(),
            //});

            //ViewBag.CategoryList = categoryList;

            ProductVM productVM = new ProductVM
            {
                CategoryList =  categoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                Product = new Product()
            };

            return View(productVM);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (product == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                productRepository.Add(product);
                TempData["success"] = "product created successfully";
                productRepository.Save();

                return RedirectToAction("Index");
            }

            return View(product);
         
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product? product = productRepository.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if(product == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                productRepository.Update(product);
                TempData["success"] = "product updated successfully";
                productRepository.Save();

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            Product? product = productRepository.Get(u => u.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product? product = productRepository.Get(u => u.Id == id);

            productRepository.Remove(product);
            TempData["success"] = "product deleted successfully";
            productRepository.Save();

            return RedirectToAction("Index");
        }
    }
}
