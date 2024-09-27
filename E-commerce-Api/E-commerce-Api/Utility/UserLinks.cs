using Contracts;
using Entities.LinkModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Shared.DTO;

namespace BookApp.Utility
{
    public class UserLinks : IUserLinks
    {
        private readonly LinkGenerator _linkGenerator;
        public UserLinks(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }
        public LinkResponse<UserDto> TryGenerateLinks(List<UserDto> Users, HttpContext httpContext)
        {
            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedUsers(Users, httpContext);

            return new LinkResponse<UserDto>
            {
                Entities = Users,
                HasLinks = false
            };
        }
        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        public LinkResponse<UserDto> ReturnLinkdedUsers(List<UserDto> Users, HttpContext httpContext)
        {
            var UsersDtoList = Users.ToList();

            List<LinksForObject<UserDto>> UsersWithLinks = new List<LinksForObject<UserDto>>();
            for (var index = 0; index < Users.Count(); index++)
            {
                var UsersLinks = CreateLinksForUser(httpContext, Users[index].Id);

                UsersWithLinks.Add(new LinksForObject<UserDto>
                {
                    Links = UsersLinks,
                    Value = UsersDtoList[index]
                });
            }

            var UsersCollection = new LinkCollectionWrapper<LinksForObject<UserDto>>(UsersWithLinks);

            UsersCollection.Links = CreateLinksForUsers(httpContext);

            return new LinkResponse<UserDto>
            {
                LinkedEntities = UsersCollection,
                HasLinks = true
            };
        }

        private List<Link> CreateLinksForUser(HttpContext httpContext, string id)
        {

            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(httpContext,
                action:"GetUser", values: new {id}),
                "self",
                "GET"),

                new Link(_linkGenerator.GetUriByAction(httpContext,
                "DeleteUser", values: new { id }),
                "Delete_User",
                "DELETE"),

                new Link(_linkGenerator.GetUriByAction(httpContext,
                "UpdateUser", values: new { id }),
                "Update_User",
                "PUT")
            };

            return links;
        }

        private List<Link> CreateLinksForUsers(HttpContext httpContext)
        {
            return new List<Link>
            {
              new Link(_linkGenerator.GetUriByAction(httpContext,
               "GetAllUsers", values: new { }),
               "self",
               "GET")
            };

        }
    }
}
