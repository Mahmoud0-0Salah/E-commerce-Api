using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace Contracts
{
    public interface IProductLinks
    {
        LinkResponse<ProductDto> TryGenerateLinks(List<ProductDto> Products, int CateogryId,  HttpContext httpContext);
    }
}
