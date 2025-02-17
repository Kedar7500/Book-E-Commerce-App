using Book.DataAccess.Data;
using Book.DataAccess.Repository.IRepository;
using Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext) 
        {
            this.dbContext = dbContext;
        }
        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void Update(Company company)
        {
           dbContext.companies.Update(company);
        }
    }
}
