using Entities.LinkModels;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api")]
    [ApiController]

    public class RootController : ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;
        public RootController(LinkGenerator linkGenerator)=> _linkGenerator = linkGenerator;

        [HttpGet]
        public IActionResult GetRoot()
        {

            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext,
                action:"GetRoot",controller:"Root", values: new { }),
                "self",
                "GET"),

                new Link(_linkGenerator.GetUriByAction(HttpContext,
                action:"GetAllCatgoryies",controller:"Catgories", values: new { }),
                "Get_Catgories",
                "Get"),

                new Link(_linkGenerator.GetUriByAction(HttpContext,
                action:"GetAllCatgoryiesOptions",controller:"Catgories", values: new { }),
                "Options_Catgories",
                "Options"),

                new Link(_linkGenerator.GetUriByAction(HttpContext,
                action:"GetAllCatgoryies",controller:"Catgories", values: new { }),
                "Head_Catgories",
                "Head"),
                
                new Link(_linkGenerator.GetUriByAction(HttpContext,
                action:"CreateCateogry",controller:"Catgories", values: new { }),
                "Create_Catgories",
                "Post")
            };

            return Ok(links);
        }
    }
}
