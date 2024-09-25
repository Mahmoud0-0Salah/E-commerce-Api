using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.DTO;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>,IUserRepository
    {

        private readonly UserManager<User> _userManager;

        public UserRepository(RepositoryContext repositoryContext, UserManager<User> userManager)
      : base(repositoryContext)
        {
            _userManager = userManager;
        }
        public async Task DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                throw new UserNotFoundException(id);

            await _userManager.DeleteAsync(user);
        }

        public async Task<PagedList<User>> GetAllUsersAsync(bool TrackCahnges, UserParameters userParameters)
        {
            var Res =  await (TrackCahnges ? _userManager.Users.Paging(userParameters.PageNumber,userParameters.PageSize).ToListAsync() : _userManager.Users.AsNoTracking().Paging(userParameters.PageNumber, userParameters.PageSize).ToListAsync());
            var Count = await _userManager.Users.AsNoTracking().Paging(userParameters.PageNumber, userParameters.PageSize).CountAsync();

            return new PagedList<User>(Res, Count, userParameters.PageNumber, userParameters.PageSize);
        }

        public async Task<User> GetUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                throw new UserNotFoundException(id);

            return user;
        }
        
        public async Task<IdentityResult> UpdateUser(User newUser,string id, string? OldPass = null, string? NewPass =null)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    throw new UserNotFoundException(id);

                user.FirstName =newUser.FirstName;
                user.LastName =newUser.LastName;
                user.PhotoUrl = newUser.PhotoUrl;
                user.Adress = newUser.Adress;

                var res = await _userManager.UpdateAsync(user);

                if (!res.Succeeded)
                    return res;

                if (!string.IsNullOrWhiteSpace(NewPass))
                {
                    var passwordResult = await _userManager.ChangePasswordAsync(user, OldPass, NewPass);
                    if (!passwordResult.Succeeded)
                        return passwordResult;

                }

                transaction.Complete();
            }

            return IdentityResult.Success; 
        }
    }
}
