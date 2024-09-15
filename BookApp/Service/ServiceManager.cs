using AutoMapper;
using Contracts;
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

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper , IProductLinks productLinks)
        {
            _catgoryService = new Lazy<ICatgoryService>(() => new
          CatgoryService(repositoryManager,mapper));
            _productService = new Lazy<IProductService>(() => new
          ProductService(repositoryManager, mapper, productLinks));
        }

        public ICatgoryService CatgoryService => _catgoryService.Value;
        public IProductService ProductService => _productService.Value;
    }

}
