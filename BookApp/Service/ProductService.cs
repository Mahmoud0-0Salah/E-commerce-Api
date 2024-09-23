using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.LinkModels;
using Shared.DTO;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace Service
{
    internal sealed class ProductService : IProductService
    {
        private readonly IRepositoryManager _repository;

        private readonly IMapper _mapper;

        private readonly IProductLinks _productLinks;

        public ProductService(IRepositoryManager repository, IMapper mapper, IProductLinks productLinks)
        {
            _repository = repository;
            _mapper = mapper;
            this._productLinks = productLinks;
        }

        private async Task CheckIfCateogryExists(int id, bool trackChanges = true)
        {
            var Cateogry = await _repository.Catgory.GetCatgoryByIdAsync(id,trackChanges);
            if (Cateogry is null)
                 throw new CateogryNotFoundException(id);
        }

        private async Task<Product> GetProductForCateogryAndCheckIfItExists(int CateogryId, int id, bool trackChanges = true)
        {
            var Product = await _repository.Product.GetProductWithCateogriesAsync(CateogryId, id,trackChanges);
            if (Product is null)
                throw new ProductNotFoundException(id);

            return Product;
        }


        public async Task<ProductDto> CreateProductAsync(int CateogryId, ProductForCreationDto product, bool TrackChanges)
        {
            await CheckIfCateogryExists(CateogryId, TrackChanges);

            var NewOb = _mapper.Map<Product>(product);

            _repository.Product.CreateProduct(CateogryId, NewOb);

            await _repository.SaveAsync();

            return _mapper.Map<ProductDto>(NewOb);
        }

        public async Task DeleteProductAsync(int CateogryId, int ProductId, bool TrackChanges)
        {
            await CheckIfCateogryExists(CateogryId, TrackChanges);

            var product = await GetProductForCateogryAndCheckIfItExists(CateogryId, ProductId, TrackChanges);

            _repository.Product.DeleteProduct(product);
            await _repository.SaveAsync();
        }


        public async Task<IEnumerable<ProductDto>> GetProductsAsync(int CateogryId, bool TrackChanges)
        {
            var Products = await _repository.Product.GetProductsAsync(CateogryId, TrackChanges);

            return _mapper.Map<IEnumerable<ProductDto>>(Products);
        }

        public async Task<(LinkResponse<ProductDto> products, MetaData MetaData)> GetProductsWithCateogriesAsync(int CateogryId, bool TrackChanges, LinkParameters<ProductParameters> LinkParameters)
        {
            if (LinkParameters.Parameters.MaxPrice< LinkParameters.Parameters.MinPrice)
                throw new MaxRangeBadRequestException();
                
            await CheckIfCateogryExists(CateogryId, TrackChanges);

            var ProductsWithMetaData = await _repository.Product.GetProductsWithCateogriesAsync(CateogryId, TrackChanges, LinkParameters.Parameters);

            var Products = _mapper.Map<IEnumerable<ProductDto>>(ProductsWithMetaData);

            var links = _productLinks.TryGenerateLinks(Products.ToList(), CateogryId, LinkParameters.Context);

            return (links, ProductsWithMetaData.MetaData);
        }

        public async Task<ProductDto> GetProductWithCateogriesAsync(int CateogryId, int ProductId, bool TrackChanges)
        {
            await CheckIfCateogryExists(CateogryId, TrackChanges);


            var Product = await _repository.Product.GetProductWithCateogriesAsync(CateogryId, ProductId, TrackChanges);

            if (Product == null)
                throw new ProductNotFoundException(ProductId);

            return _mapper.Map<ProductDto>(Product);
        }
        public async Task<(ProductForUpdateDto ProductForUpdate, Product Productntity)> GetProductForPatchAsync(int CateogryId, int id, bool TrackChanges)
        {
            await CheckIfCateogryExists(CateogryId, TrackChanges);

            var product = await GetProductForCateogryAndCheckIfItExists(CateogryId, id, TrackChanges);

            var ProductForUpdate = _mapper.Map<ProductForUpdateDto>(product);

            return (ProductForUpdate, product);
        }

        public async Task SaveChangesForPatchAsync(ProductForUpdateDto ProductForUpdate, Product Productntity)
        {
            _mapper.Map(ProductForUpdate, Productntity);
            await _repository.SaveAsync();
        }

        public async Task UpdateProductAsync(int CateogryId, int id, ProductForUpdateDto ProductForUpdate, bool TrackChanges)
        {
            await CheckIfCateogryExists(CateogryId, TrackChanges);

            var product = await GetProductForCateogryAndCheckIfItExists(CateogryId, id, TrackChanges);

            _mapper.Map(ProductForUpdate, product);

            await _repository.SaveAsync();
        }
    }
}
