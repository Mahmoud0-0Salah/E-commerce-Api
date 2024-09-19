using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Shared.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using System.Xml.Linq;

namespace Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        // private readonly RoleManager<IdentityRole> _roleManager;
        private User? _user;
        public AuthenticationService(IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            //   _roleManager = roleManager;
        }

        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var res = await _userManager.CreateAsync(user, userForRegistration.Password);
                if (!res.Succeeded)
                    return res;

                var roleRes = await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
                if (!roleRes.Succeeded)
                    return roleRes;

                transaction.Complete();
            }
            return IdentityResult.Success;
        }

        public async Task<bool> ValidateUser(UserForLoginDto userForLogin)
        {
            _user = await _userManager.FindByEmailAsync(userForLogin.Email);

            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForLogin.Password));

            return result;
        }

        private SigningCredentials GetSigningCredentials()
        {
            return new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT")["SecretKey"])), SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
             new Claim(ClaimTypes.Name, _user.UserName),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             new Claim(JwtRegisteredClaimNames.Sub, _user.Id)
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings["ValidIssuer"],
            audience: jwtSettings["ValidAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JWT");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
                ValidateLifetime = true,
                ValidIssuer = jwtSettings["ValidIssuer"],
                ValidAudience = jwtSettings["ValidAudience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public async Task<TokenDto> CreateToken(bool populateExp, bool rememberMe)
        {
            var signingCredentials = GetSigningCredentials();

            var claims = await GetClaims();

            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var refreshToken = GenerateRefreshToken();

            _user.RefreshToken = refreshToken;

            if (populateExp)
                _user.RefreshTokenExpiryTime = rememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddMinutes(30);

            await _userManager.UpdateAsync(_user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDto(accessToken, refreshToken);
        }

        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null || user.RefreshToken != tokenDto.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new RefreshTokenBadRequest();
            _user = user;
            return await CreateToken(populateExp: false,rememberMe: false);
        }
    }
}
