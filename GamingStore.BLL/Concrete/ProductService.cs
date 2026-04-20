using AutoMapper;
using GamingStore.BLL.Abstract;
using GamingStore.DAL.Abstract;
using GamingStore.EL.Dtos;
using GamingStore.EL.Models;
using GamingStore.EL.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.BLL.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public void CreateProduct(ProductDtoForInsertion productDto)
        {
            Product product = _mapper.Map<Product>(productDto);
            _repository.CreateProduct(product);
            _repository.Save();
        }

        public void DeleteProduct(int id)
        {
            Product product = GetOneProduct(id, false);
            if (product is not null)
            {
                _repository.DeleteProduct(product);
                _repository.Save();
            }
        }

        public IEnumerable<Product> GetAllProducts(bool trackChanges)
        {
            return _repository.GetAllProducts(trackChanges);
        }

        public Product? GetOneProduct(int id, bool trackChanges)
        {
            var product = _repository.GetOneProduct(id, trackChanges);
            if (product is null)
                throw new Exception("Product not found!");
            return product;
        }

        public void UpdateProduct(ProductDtoForUpdate productDtoForUpdate)
        {
            var entity = _mapper.Map<Product>(productDtoForUpdate);
            _repository.UpdateProduct(entity);
            _repository.Save();
        }

        public ProductDtoForUpdate GetOneProductForUpdate(int id, bool trakcChanges)
        {
            var product = GetOneProduct(id, trakcChanges);
            var productDto = _mapper.Map<ProductDtoForUpdate>(product);
            return productDto;
        }

        public IEnumerable<Product> GetShowcaseProducts(bool trackChanges)
        {
            var products = _repository.GetShowcaseProducts(trackChanges);
            return products;
        }

        public IEnumerable<Product> GetAllProductsWithDetails(ProductRequestParameters p)
        {
            return _repository.GetAllProductsWithRequestParameters(p);
        }

    }
}
