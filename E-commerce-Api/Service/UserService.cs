using AutoMapper;
using Contracts;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Shared.DTO;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserLinks _userLinks;
        private readonly IRepositoryManager _repositoryManager;
        public UserService(IRepositoryManager repository, IMapper mapper, IUserLinks userLinks)
        {
            _repositoryManager = repository;
            _mapper = mapper;
            _userLinks = userLinks;
        }

        public async Task DeleteUser(string id)
        {
             await _repositoryManager.User.DeleteUser(id);
        }

        public async Task<UserDto> GetUser(string id)
        {
          return  _mapper.Map<UserDto>(await _repositoryManager.User.GetUserAsync(id));
        }

        public async Task<(LinkResponse<UserDto> Users, MetaData MetaData)> GetAllUsersAsync(bool trackChanges, LinkParameters<UserParameters> userparameters)
        {
           var usersWithMetaData = await  _repositoryManager.User.GetAllUsersAsync(trackChanges, userparameters.Parameters);

            var users = _mapper.Map<List<UserDto>>(usersWithMetaData);


            var usersWithLinks = _userLinks.TryGenerateLinks(users, userparameters.Context);

            return (usersWithLinks, usersWithMetaData.MetaData);
        }

        public async Task<IdentityResult> UpdateUser(UserDtoForUpdate newUser, string id)
        {
            var user = _mapper.Map<User>(newUser);

            return await _repositoryManager.User.UpdateUser(user, id,newUser.OldPassword,newUser.NewPassword);
        }

    }
}
