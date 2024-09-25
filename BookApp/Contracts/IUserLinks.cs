using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUserLinks
    {
        LinkResponse<UserDto> TryGenerateLinks(List<UserDto> Users, HttpContext httpContext);

    }
}
