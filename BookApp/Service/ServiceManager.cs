using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICatgoryService> _catgoryService;
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IUserService> _useerService;
        private readonly Lazy<IAuthenticationService> _authentication;
        private readonly Lazy<IOrderService> _orderService;

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper , IProductLinks productLinks,ICatgoryLinks catgoryLinks, UserManager<User> userManager, IConfiguration configuration,IUserLinks userLinks)
        {
            _catgoryService = new Lazy<ICatgoryService>(() => new
          CatgoryService(repositoryManager,mapper,catgoryLinks));

            _productService = new Lazy<IProductService>(() => new
          ProductService(repositoryManager, mapper, productLinks));

            _useerService = new Lazy<IUserService>(() => new
          UserService(repositoryManager, mapper, userLinks));

            _orderService = new Lazy<IOrderService>(() => new
          OrderService(repositoryManager, mapper));

            _authentication = new Lazy<IAuthenticationService>(() => new
          AuthenticationService(mapper,userManager,configuration));
        }

        public ICatgoryService CatgoryService => _catgoryService.Value;
        public IProductService ProductService => _productService.Value;
        public IAuthenticationService AuthenticationService => _authentication.Value;
        public IUserService UserService => _useerService.Value;
        public IOrderService OrderService => _orderService.Value;
    }

}
