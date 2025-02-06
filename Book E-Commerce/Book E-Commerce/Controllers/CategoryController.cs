using Book_E_Commerce.Data;
using Book_E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace Book_E_Commerce.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            List<Category> categoryList = dbContext.Categories.ToList();
            return View(categoryList);
        }
    }
}
