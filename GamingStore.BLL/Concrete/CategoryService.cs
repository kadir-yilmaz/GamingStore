using GamingStore.BLL.Abstract;
using GamingStore.DAL.Abstract;
using GamingStore.EL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.BLL.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Category> GetAllCategories(bool trackChanges)
        {
            return _repository.GetAll(trackChanges);
        }


    }
}
