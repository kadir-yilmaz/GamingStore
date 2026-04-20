using GamingStore.EL.Models;
using GamingStore.EL.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.DAL.Abstract
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IQueryable<Product> GetAllProducts(bool trackChanges);
        Product? GetOneProduct(int id, bool trackChanges);
        IQueryable<Product> GetAllProductsWithRequestParameters(ProductRequestParameters p);
        IQueryable<Product> GetShowcaseProducts(bool trackChanges);
        void CreateProduct(Product product);
        void UpdateProduct(Product entity);
        void DeleteProduct(Product product);
    }
}
