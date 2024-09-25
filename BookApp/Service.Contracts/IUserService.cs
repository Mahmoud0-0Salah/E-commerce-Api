using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Shared.DTO;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IUserService
    {
        Task<(LinkResponse<UserDto> Users, MetaData MetaData)> GetAllUsersAsync(bool trackChanges, LinkParameters<UserParameters> userparameters);

        Task<IdentityResult> UpdateUser(UserDtoForUpdate newUser,string id);

        Task DeleteUser(string id);

        Task<UserDto> GetUser(string id);


    }
}
