using Entities.LinkModels;
using Shared.DTO;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync(int CateogryId, bool TrackChanges);
        Task<(LinkResponse<ProductDto> products, MetaData MetaData)> GetProductsWithCateogriesAsync(int CateogryId, bool TrackChanges, LinkParameters<ProductParameters> LinkParameters);
        Task<ProductDto> GetProductWithCateogriesAsync(int CateogryId, int ProductId, bool TrackChanges);
        Task DeleteProductAsync(int CateogryId, int ProductId, bool TrackChanges);
        Task<ProductDto> CreateProductAsync(int CateogryId, ProductForCreationDto product, bool TrackChanges);
        Task UpdateProductAsync(int CateogryId, int id, ProductForUpdateDto ProductForUpdate, bool TrackChanges);
        Task<(ProductForUpdateDto ProductForUpdate, Product Productntity)> GetProductForPatchAsync(int CateogryId, int id, bool TrackChanges);
        Task SaveChangesForPatchAsync(ProductForUpdateDto ProductForUpdate, Product Productntity);

    }

}
