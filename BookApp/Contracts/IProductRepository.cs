using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(int CateogryId,bool TrackChanges);
        Task<PagedList<Product>> GetProductsWithCateogriesAsync(int CateogryId,bool TrackChanges, ProductParameters productParameters);
        Task<Product> GetProductWithCateogriesAsync(int CateogryId, int ProductId, bool TrackChanges);
        void CreateProduct(int CateogryId, Product product);
        void DeleteProduct(Product product);
    }
}
