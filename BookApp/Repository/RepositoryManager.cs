using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IProductRepository> _productRepository;
        private readonly Lazy<ICatgoryRepository> _catgoryRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;

            _productRepository = new Lazy<IProductRepository>(() => new
          ProductRepository(repositoryContext));

            _catgoryRepository = new Lazy<ICatgoryRepository>(() => new
          CatgoryRepository(repositoryContext));
        }

        public IProductRepository Product => _productRepository.Value;
        public ICatgoryRepository Catgory => _catgoryRepository.Value;

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}
