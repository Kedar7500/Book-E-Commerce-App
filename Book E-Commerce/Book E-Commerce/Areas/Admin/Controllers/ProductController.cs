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
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> products = productRepository.GetAll(includeProperties:"Category").ToList();


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
                CategoryList =  categoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                Product = new Product()
            };

            if(id == 0 || id == null)
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

                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); 
                    string productPath = Path.Combine(wwwRootPath, @"images\products");

                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\products\" + fileName;
                }

                if(productVM.Product.Id == 0)
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

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = productRepository.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = products });
        }
        #endregion
    }
}
