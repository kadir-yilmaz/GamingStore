using GamingStore.EL.Dtos;
using GamingStore.EL.Models;
using GamingStore.EL.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.BLL.Abstract
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts(bool trackChanges);
        Product? GetOneProduct(int id, bool trackChanges);
        IEnumerable<Product> GetAllProductsWithDetails(ProductRequestParameters p);
        IEnumerable<Product> GetShowcaseProducts(bool trackChanges);
        ProductDtoForUpdate GetOneProductForUpdate(int id, bool trackChanges);
        void CreateProduct(ProductDtoForInsertion productDtoForInsertion);
        void UpdateProduct(ProductDtoForUpdate productDtoForUpdate);
        void DeleteProduct(int id);
    }
}
