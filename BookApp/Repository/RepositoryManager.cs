using Contracts;
using Microsoft.AspNetCore.Identity;
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
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IOrderRepository> _orderRepository;

        public RepositoryManager(RepositoryContext repositoryContext,UserManager<Entities.Models.User> userManager)
        {
            _repositoryContext = repositoryContext;

            _productRepository = new Lazy<IProductRepository>(() => new
          ProductRepository(repositoryContext));

            _catgoryRepository = new Lazy<ICatgoryRepository>(() => new
          CatgoryRepository(repositoryContext));

            _userRepository = new Lazy<IUserRepository>(() => new
          UserRepository(repositoryContext,userManager));

            _orderRepository = new Lazy<IOrderRepository>(() => new
          OrderRepository(repositoryContext));
        }

        public IProductRepository Product => _productRepository.Value;
        public ICatgoryRepository Catgory => _catgoryRepository.Value;
        public IUserRepository User => _userRepository.Value;
        public IOrderRepository Order => _orderRepository.Value;

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}
