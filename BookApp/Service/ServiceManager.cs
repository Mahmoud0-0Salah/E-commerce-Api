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
        private readonly Lazy<IAuthenticationService> _authentication;

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper , IProductLinks productLinks, UserManager<User> userManager, IConfiguration configuration)
        {
            _catgoryService = new Lazy<ICatgoryService>(() => new
          CatgoryService(repositoryManager,mapper));
            _productService = new Lazy<IProductService>(() => new
          ProductService(repositoryManager, mapper, productLinks));
            _authentication = new Lazy<IAuthenticationService>(() => new
          AuthenticationService(mapper,userManager,configuration));
        }

        public ICatgoryService CatgoryService => _catgoryService.Value;
        public IProductService ProductService => _productService.Value;
        public IAuthenticationService AuthenticationService => _authentication.Value;
    }

}
