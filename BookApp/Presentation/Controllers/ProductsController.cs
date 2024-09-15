using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.ActionFilters;
using Service.Contracts;
using Shared.DTO;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/Catgories/{CatgoryId:int}/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IServiceManager _service;

        public ProductsController(IServiceManager service) => _service = service;

        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public  async Task<IActionResult> GetProducts(int CatgoryId, [FromQuery]ProductParameters productParameters)
        {

            var linkParams = new LinkParameters(productParameters, HttpContext);

            var result = await _service.ProductService.GetProductsWithCateogriesAsync(CatgoryId, false, linkParams);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.MetaData));

            return Ok(result.products.HasLinks ? result.products.LinkedEntities : result.products.Entities);
        }

        [HttpGet("{id:int}",Name = "GetProductById")]
        public async Task<IActionResult> GetProduct(int CatgoryId,int id)
        {
            return Ok(await _service.ProductService.GetProductWithCateogriesAsync(CatgoryId,id, false));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(int CatgoryId, [FromBody] ProductForCreationDto product)
        {
            if (product is null)
                return BadRequest("ProductForCreationDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var CreatedProduct =await _service.ProductService.CreateProductAsync(CatgoryId,product,false);

            return CreatedAtRoute("GetProductById", new { CatgoryId = CatgoryId, id = CreatedProduct.Id },CreatedProduct);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int CatgoryId, int id)
        {
            await _service.ProductService.DeleteProductAsync(CatgoryId, id, TrackChanges: false);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int CatgoryId, int id,[FromBody] ProductForUpdateDto Product)
        {
            if (Product == null)
                return BadRequest("ProductForUpdateDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.ProductService.UpdateProductAsync(CatgoryId, id, Product,false);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateProduct(int CatgoryId, int id,[FromBody] JsonPatchDocument<ProductForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");
            var result = await _service.ProductService.GetProductForPatchAsync(CatgoryId, id, false);
            patchDoc.ApplyTo(result.ProductForUpdate,ModelState);

            TryValidateModel(result.ProductForUpdate);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
           
            
            await _service.ProductService.SaveChangesForPatchAsync(result.ProductForUpdate,result.Productntity);
            return NoContent();
        }
    }
}
