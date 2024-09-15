using Shared.DTO;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICatgoryService
    {
        Task<(IEnumerable<CateogryDto> Catgories, MetaData MetaData)> GetAllCatgoryiesAsync(bool trackChanges, CateogryParameters productparameters);
        Task<CateogryDto> GetCatgoryByIdAsync(int id,bool trackChanges);
        Task<CateogryDto> CreateCatgoryAsync(CateogryForCreationDto cateogry);
        Task<IEnumerable<CateogryDto>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);
        Task<(IEnumerable<CateogryDto> cateogries, string ids)> CreateCateogryCollectionAsync(IEnumerable<CateogryForCreationDto> CateogryCollection);
        Task DeleteCateogryAsync(int CateogryId, bool trackChanges);

        Task UpdateCateogryAsync(int CateogryId, CateogryForUpdateDto CatgoryForUpdate);
    }
}
