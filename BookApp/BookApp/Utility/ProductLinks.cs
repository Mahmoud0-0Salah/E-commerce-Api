using Contracts;
using Entities.LinkModels;
using Microsoft.Net.Http.Headers;
using Shared.DTO;
using WebApplication1.Models;

namespace BookApp.Utility
{
    public class ProductLinks : IProductLinks
    {
        private readonly LinkGenerator _linkGenerator;

        public ProductLinks(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        public LinkResponse<ProductDto> TryGenerateLinks(List<ProductDto> Products, int CateogryId, HttpContext httpContext)
        {

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedProduct(Products, CateogryId, httpContext);

            return new LinkResponse<ProductDto>
            {
                Entities = Products,
                HasLinks = false
            };
        }

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        public LinkResponse<ProductDto> ReturnLinkdedProduct(List<ProductDto> Products, int CateogryId, HttpContext httpContext)
        {
            var ProductsDtoList = Products.ToList();

            List<LinksForObject<ProductDto>> ProductWithLinks = new List<LinksForObject<ProductDto>>();
            for (var index = 0; index < ProductsDtoList.Count(); index++)
            {
                var ProductLinks = CreateLinksForProduct(httpContext, CateogryId, ProductsDtoList[index].Id);

                ProductWithLinks.Add(new LinksForObject<ProductDto>
                {
                    Links = ProductLinks,
                    Value = ProductsDtoList[index]
                });
            }

            var ProductCollection = new LinkCollectionWrapper<LinksForObject<ProductDto>>(ProductWithLinks);

            ProductCollection.Links = CreateLinksForProducts(httpContext, ProductCollection);

            return new LinkResponse<ProductDto>
            {
                LinkedEntities = ProductCollection,
                HasLinks = true
            };
        }

        private List<Link> CreateLinksForProduct(HttpContext httpContext, int CatgoryId, int id)
        {

            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(httpContext,
                action:"GetProduct",controller:"Products", values: new { CatgoryId, id}),
                "self",
                "GET"),

                new Link(_linkGenerator.GetUriByAction(httpContext,
                "DeleteProduct", values: new { CatgoryId, id }),
                "delete_product",
                "DELETE"),

                new Link(_linkGenerator.GetUriByAction(httpContext,
                "UpdateProduct", values: new { CatgoryId, id }),
                "update_product",
                "PUT"),

                new Link(_linkGenerator.GetUriByAction(httpContext,
                "PartiallyUpdateProduct", values: new { CatgoryId, id }),
                "partially_update_product",
                "PATCH")
            };

            return links;
        }

        private List<Link> CreateLinksForProducts(HttpContext httpContext, LinkCollectionWrapper<LinksForObject<ProductDto>> employeesWrapper)
        {
            return new List<Link>
             {
              new Link(_linkGenerator.GetUriByAction(httpContext,
               "GetProducts", values: new { }),
               "self",
               "GET")
            };
                
        }


    }
}
