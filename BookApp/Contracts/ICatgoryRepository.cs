using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace Contracts
{
    public interface ICatgoryRepository
    {
        Task<PagedList<Cateogry>> GetAllCatgoryiesAsync(bool trackChanges, CateogryParameters cateogryparameters);

         Task<Cateogry> GetCatgoryByIdAsync(int id, bool trackChanges);
        void CreateCatgory(Cateogry cateogry);
        Task<IEnumerable<Cateogry>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);
        void CreateCateogryCollection(IEnumerable<Cateogry> CateogryCollection);
        void DeleteCateogry(Cateogry cateogry);
    }
}
