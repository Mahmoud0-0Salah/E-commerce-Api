using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.LinkModels;
using Shared.DTO;
using Shared.RequestFeatures;
using System.ComponentModel.Design;
using WebApplication1.Models;

namespace Service
{
    internal sealed class CatgoryService : ICatgoryService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ICatgoryLinks _catgoryLinks;
        public CatgoryService(IRepositoryManager repository, IMapper mapper, ICatgoryLinks catgoryLinks)
        {
            _repository = repository;
            _mapper = mapper;
            _catgoryLinks = catgoryLinks;
        }

        private async Task<Cateogry> GetCateogryAndCheckIfItExists(int id, bool trackChanges = true)
        {
            var Cateogry = await _repository.Catgory.GetCatgoryByIdAsync(id, trackChanges);
            if (Cateogry is null)
                throw new CateogryNotFoundException(id);
            return Cateogry;
        }
        public async Task<(IEnumerable<CateogryDto> cateogries, string ids)> CreateCateogryCollectionAsync(IEnumerable<CateogryForCreationDto> CateogryCollection)
        {
            if (CateogryCollection == null)
                throw new CateogryCollectionBadRequest();

            var CateogryEntities = _mapper.Map<IEnumerable<Cateogry>>(CateogryCollection);
            _repository.Catgory.CreateCateogryCollection(CateogryEntities);

            await _repository.SaveAsync();

            var CateogryCollectionToReturn = _mapper.Map<IEnumerable<CateogryDto>>(CateogryEntities);
            var ids = string.Join(",", CateogryCollectionToReturn.Select(c => c.Id));

            return (cateogries: CateogryCollectionToReturn, ids: ids);
        }

        public async Task<CateogryDto> CreateCatgoryAsync(CateogryForCreationDto cateogry)
        {
            var NewOb = _mapper.Map<Cateogry>(cateogry);
            //NewOb.Product = _mapper.Map<ICollection<Product>>(cateogry.Products);

            _repository.Catgory.CreateCatgory(NewOb);

            await _repository.SaveAsync();

            return _mapper.Map<CateogryDto>(NewOb);
        }

        public  async Task DeleteCateogryAsync(int CateogryId, bool trackChanges)
        {
            var Cateogry = await GetCateogryAndCheckIfItExists(CateogryId);

            _repository.Catgory.DeleteCateogry(Cateogry);
            await _repository.SaveAsync();
        }

        public async Task<(LinkResponse<CateogryDto> Catgories, MetaData MetaData)> GetAllCatgoryiesAsync(bool trackChanges, LinkParameters<CateogryParameters> cateogryparameters)
        {
            var CatgoriesWithMetaData = await _repository.Catgory.GetAllCatgoryiesAsync(trackChanges, cateogryparameters.Parameters);

            var CatgoriesDto = _mapper.Map<IEnumerable<CateogryDto>>(CatgoriesWithMetaData);

            var links = _catgoryLinks.TryGenerateLinks(CatgoriesDto.ToList(), cateogryparameters.Context);

            return (Catgories: links, MetaData:CatgoriesWithMetaData.MetaData);
        }

        public async Task<IEnumerable<CateogryDto>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var CateogryEntities = await _repository.Catgory.GetByIdsAsync(ids, trackChanges);

            if (ids.Count() != CateogryEntities.Count())
                throw new CollectionByIdsBadRequestException();
            
            var CateogryToReturn = _mapper.Map<IEnumerable<CateogryDto>>(CateogryEntities);
            return CateogryToReturn;
        }

        public async Task<CateogryDto> GetCatgoryByIdAsync(int id, bool trackChanges)
        {
            var cateogry = await GetCateogryAndCheckIfItExists(id, trackChanges);

            return _mapper.Map<CateogryDto>(cateogry);
        }

        public async Task UpdateCateogryAsync(int CateogryId, CateogryForUpdateDto CatgoryForUpdate)
        {
            var CatgoryEntity = await GetCateogryAndCheckIfItExists(CateogryId);
            _mapper.Map(CatgoryForUpdate, CatgoryEntity);
            await _repository.SaveAsync();
        }
    }
}
