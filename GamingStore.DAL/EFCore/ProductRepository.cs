using GamingStore.DAL.Abstract;
using GamingStore.DAL.Extensions;
using GamingStore.EL.Models;
using GamingStore.EL.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.DAL.EFCore
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public void CreateProduct(Product product) => Create(product);

        public void DeleteProduct(Product product) => Delete(product);

        public IQueryable<Product> GetAllProducts(bool trackChanges) => GetAll(trackChanges);

        public Product? GetOneProduct(int id, bool trackChanges)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            return product;
        }

        public void UpdateProduct(Product entity) => Update(entity);

        public IQueryable<Product> GetAllProductsWithRequestParameters(ProductRequestParameters p)
        {
            return _context
                .Products
                .FilteredByCategoryId(p.CategoryId)
                .FilteredBySearchTerm(p.SearchTerm)
                .FilteredByPrice(p.MinPrice, p.MaxPrice, p.IsValidPrice)
                .ToPaginate(p.PageNumber, p.PageSize);
        }

        public IQueryable<Product> GetShowcaseProducts(bool trackChanges)
        {
            return GetAll(trackChanges)
                .Where(p => p.Showcase.Equals(true));
        }
    }
}
