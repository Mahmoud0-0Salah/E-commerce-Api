using Entities.LinkModels;
using Microsoft.Net.Http.Headers;
using Shared.DTO;

namespace BookApp.Utility
{
    public class CateogriesLinks : ICatgoryLinks
    {

        private readonly LinkGenerator _linkGenerator;

        public CateogriesLinks(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        public LinkResponse<CateogryDto> TryGenerateLinks(List<CateogryDto> Cateogries, HttpContext httpContext)
        {

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedCateogries(Cateogries, httpContext);

            return new LinkResponse<CateogryDto>
            {
                Entities = Cateogries,
                HasLinks = false
            };
        }

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        public LinkResponse<CateogryDto> ReturnLinkdedCateogries(List<CateogryDto> Cateogries,  HttpContext httpContext)
        {
            var CateogriesDtoList = Cateogries.ToList();

            List<LinksForObject<CateogryDto>> CateogriesWithLinks = new List<LinksForObject<CateogryDto>>();
            for (var index = 0; index < CateogriesDtoList.Count(); index++)
            {
                var CateogriesLinks = CreateLinksForCateogry(httpContext, CateogriesDtoList[index].Id);

                CateogriesWithLinks.Add(new LinksForObject<CateogryDto>
                {
                    Links = CateogriesLinks,
                    Value = CateogriesDtoList[index]
                });
            }

            var CateogriesCollection = new LinkCollectionWrapper<LinksForObject<CateogryDto>>(CateogriesWithLinks);

            CateogriesCollection.Links = CreateLinksForCateogries(httpContext);

            return new LinkResponse<CateogryDto>
            {
                LinkedEntities = CateogriesCollection,
                HasLinks = true
            };
        }

        private List<Link> CreateLinksForCateogry(HttpContext httpContext, int id)
        {

            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(httpContext,
                action:"GetCatgoryById",controller:"Catgories", values: new {id}),
                "self",
                "GET"),

                new Link(_linkGenerator.GetUriByAction(httpContext,
                "DeleteCatgory", values: new { id }),
                "Delete_Catgory",
                "DELETE"),

                new Link(_linkGenerator.GetUriByAction(httpContext,
                "UpdateCateogry", values: new { id }),
                "Update_Cateogry",
                "PUT")
            };

            return links;
        }

        private List<Link> CreateLinksForCateogries(HttpContext httpContext)
        {
            return new List<Link>
             {
              new Link(_linkGenerator.GetUriByAction(httpContext,
               "GetAllCatgoryies", values: new { }),
               "self",
               "GET")
            };

        }

    }
}
