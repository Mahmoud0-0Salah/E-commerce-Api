using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using Shared.DTO;

public interface ICatgoryLinks
{
    LinkResponse<CateogryDto> TryGenerateLinks(List<CateogryDto> Cateogriese, HttpContext httpContext);
}