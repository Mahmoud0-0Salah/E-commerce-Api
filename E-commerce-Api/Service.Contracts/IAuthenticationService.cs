using Microsoft.AspNetCore.Identity;
using Shared.DTO;

namespace Service.Contracts
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
        Task<bool> ValidateUser(UserForLoginDto userForAuth);
        Task<TokenDto> CreateToken(bool populateExp, bool rememberMe);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
    }
}
