using Contracts;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace Repository
{
    internal class CatgoryRepository : RepositoryBase<Cateogry>, ICatgoryRepository
    {
        public CatgoryRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public void CreateCateogryCollection(IEnumerable<Cateogry> CateogryCollection)
        {
            CreateeRange(CateogryCollection);
        }

        public void CreateCatgory(Cateogry cateogry) => Create(cateogry);


        public void DeleteCateogry(Cateogry cateogry) => Delete(cateogry);

        public async Task<PagedList<Cateogry>> GetAllCatgoryiesAsync(bool trackChanges, CateogryParameters cateogryparameters)
        {
            var Cateogries= await FindAll(trackChanges)
                .Paging(cateogryparameters.PageNumber, cateogryparameters.PageSize)
                .Search(cateogryparameters.searchTerm)
                .Sort(cateogryparameters.ordereby)
                .ToListAsync();

            var count = await FindAll(trackChanges).CountAsync();

            return new PagedList<Cateogry>(Cateogries, count,cateogryparameters.PageNumber, cateogryparameters.PageSize);
        }

        public async Task<IEnumerable<Cateogry>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges)
        {
           return await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
        }

        public async Task<Cateogry> GetCatgoryByIdAsync(int id, bool trackChanges)
        {
            return await FindByCondition(c => c.Id == id, trackChanges).SingleOrDefaultAsync();
        }
    }
}
