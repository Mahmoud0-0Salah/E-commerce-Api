using Microsoft.AspNetCore.Http;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinkModels
{
    public record LinkParameters(ProductParameters ProductParameters, HttpContext Context);
}
