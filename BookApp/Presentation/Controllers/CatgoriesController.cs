using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ModelBinders;
using Service.Contracts;
using Shared.ActionFilters;
using Shared.DTO;
using Shared.RequestFeatures;
using System.ComponentModel.Design;
using System.Text.Json;
using WebApplication1.Models;

namespace BookApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatgoriesController : ControllerBase
    {

        private readonly IServiceManager _service;

        public CatgoriesController(IServiceManager service) => _service = service;


        [HttpOptions]
        public async Task<IActionResult> GetAllCatgoryiesOptions([FromQuery] CateogryParameters cateogryparameters)
        {
            Response.Headers.Add("Allow", "Post, Get, Options");

            return Ok();
        }

        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllCatgoryies([FromQuery] CateogryParameters cateogryparameters)
        {
            var PageResult = await _service.CatgoryService.GetAllCatgoryiesAsync(false, cateogryparameters);
            Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(PageResult.MetaData));

            return Ok(PageResult.Catgories);
        }
        [HttpGet("{id:int}", Name = "GetCatgoryById")]
        public async Task<IActionResult> GetCatgoryById(int id)
        {

            return Ok(await _service.CatgoryService.GetCatgoryByIdAsync(id, false));
        }

        [HttpGet("collection/({ids})", Name = "CateogryCollection")]
        public async Task<IActionResult> GetCateogryCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            var companies = await _service.CatgoryService.GetByIdsAsync(ids, trackChanges: false);

            return Ok(companies);
        }

        [HttpPost]
        [ValidationFilterAttribute]
        public async Task<IActionResult> CreateCateogry([FromBody] CateogryForCreationDto cateogry)
        {
            var CreatedCateogry = await _service.CatgoryService.CreateCatgoryAsync(cateogry);
            return CreatedAtRoute("GetCatgoryById", new { id = CreatedCateogry.Id },
            CreatedCateogry);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CateogryForCreationDto>? cateogryCollection)
        {

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var result = await _service.CatgoryService.CreateCateogryCollectionAsync(cateogryCollection);
            return CreatedAtRoute("CateogryCollection", new { result.ids }, result.cateogries);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            await _service.CatgoryService.DeleteCateogryAsync(id, trackChanges: false);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ValidationFilterAttribute]
        public async Task<IActionResult> UpdateCateogry(int id, [FromBody] CateogryForUpdateDto Cateogry)
        {
            await _service.CatgoryService.UpdateCateogryAsync(id, Cateogry);

            return NoContent();
        }

    }
}
