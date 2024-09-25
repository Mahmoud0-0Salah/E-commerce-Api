using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUserRepository
    {
        Task<PagedList<User>> GetAllUsersAsync(bool TrackCahnges, UserParameters userParameters);
        Task<User> GetUserAsync(string id);
        Task DeleteUser(string id);
        Task<IdentityResult> UpdateUser(User newUser, string id,string? OldPass = null, string? NewPass = null);
    }
}
