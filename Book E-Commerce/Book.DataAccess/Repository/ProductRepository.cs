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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void Update(Product product)
        {
            Product product1 = dbContext.Products.SingleOrDefault(p => p.Id == product.Id);

            if (product1 != null)
            {
                product1.Title = product.Title;
                product1.Description = product.Description;
                product1.ISBN = product.ISBN;
                product1.Author = product.Author;
                product1.LeastPrice = product.LeastPrice;
                product1.Price = product.Price;
                product1.Price50 = product.Price50;
                product1.Price100 = product.Price100;
                product1.CategoryId = product.CategoryId;

                if(product.ImageUrl != null)
                {
                    product1.ImageUrl = product.ImageUrl;
                }
            }

            dbContext.Products.Update(product1);
        }
    }
}
