using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Book.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book_E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository companyRepository;

        public CompanyController(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }
        public IActionResult Index()
        {
            List<Company> companies = companyRepository.GetAll().ToList();
            return View(companies);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Company());
            }
            else
            {
                Company company = companyRepository.Get(x => x.Id == id);
                return View(company);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if(company.Id == 0)
                {
                    companyRepository.Add(company);
                }
                else
                {
                    companyRepository.Update(company);
                }

                companyRepository.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(company);
            }

        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = companyRepository.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = companyRepository.Get(u => u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            companyRepository.Remove(CompanyToBeDeleted);
            companyRepository.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion


    }
}
