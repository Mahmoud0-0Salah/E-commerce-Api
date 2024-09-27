﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
using Repository.Extensions;
using Entities.Exceptions;
using Entities.LinkModels;

namespace Repository
{
    internal class ProductRepository : RepositoryBase<Product> , IProductRepository
    {

        public ProductRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public void CreateProduct(int CateogryId, Product product)
        {
            product.CateogryId = CateogryId;
            product.ProductState = ProductState.Pending;
            Create(product);
        }

        public void DeleteProduct(Product product) => Delete(product);

        public async Task<PagedList<Product>> GetPendingProductsAsync( bool TrackChanges, ProductParameters productParameters)
        {
            var products = await FindByCondition( p => p.ProductState == ProductState.Pending && p.Amount > 0, TrackChanges)
           .Paging(productParameters.PageNumber, productParameters.PageSize)
           .Filter(productParameters.MinPrice, productParameters.MaxPrice)
           .Search(productParameters.searchTerm)
           .Sort(productParameters.ordereby)
           .ToListAsync();

            var count = await FindByCondition((p => p.ProductState == ProductState.Pending && p.Amount > 0), TrackChanges).CountAsync();

            return new PagedList<Product>(products, count, productParameters.PageNumber, productParameters.PageSize);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(int CateogryId, bool TrackChanges)
        {
            return await FindByCondition(p => p.CateogryId == CateogryId, TrackChanges).ToListAsync();
        }

        public async Task<PagedList<Product>> GetProductsWithCateogriesAsync(int CateogryId, bool TrackChanges, ProductParameters productParameters)
        {
         
             var products = await FindByCondition(p => p.CateogryId == CateogryId && p.ProductState == ProductState.Accepted && p.Amount > 0, TrackChanges)
            .Paging(productParameters.PageNumber, productParameters.PageSize)
            .Filter(productParameters.MinPrice, productParameters.MaxPrice)
            .Search(productParameters.searchTerm)
            .Sort(productParameters.ordereby)
            .ToListAsync();


            var count = await FindByCondition((p => p.CateogryId == CateogryId), TrackChanges).CountAsync();

            return new PagedList<Product>(products, count, productParameters.PageNumber, productParameters.PageSize);
        }

        public async Task<Product> GetProductWithCateogriesAsync(int CateogryId, int ProductId, bool TrackChanges)
        {
            return await FindByCondition(p => p.CateogryId == CateogryId && p.Id == ProductId, TrackChanges).Include(p => p.Cateogry).SingleOrDefaultAsync();
        }
    }
}
