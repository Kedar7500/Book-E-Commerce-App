using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Book.Models.ViewModels;
using Book.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Book_E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> products = productRepository.GetAll(includeProperties: "Category").ToList();


            return View(products);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
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
                CategoryList = categoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                Product = new Product()
            };

            if (id == 0 || id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = productRepository.Get(u => u.Id == id);
                return View(productVM);
            }

        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {

            if (productVM == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\products");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\products\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    productRepository.Add(productVM.Product);
                }
                else
                {
                    productRepository.Update(productVM.Product);
                }

                TempData["success"] = "product created successfully";
                productRepository.Save();

                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = categoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                });

                return View(productVM);

            }

        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = productRepository.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Product product = productRepository.Get(x => x.Id == id);

            if (product == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            productRepository.Remove(product);
            productRepository.Save();

            return Json(new { success = true, message = "Delete Successfull" });
            
        }

        #endregion
    }
}
