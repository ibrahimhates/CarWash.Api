using CarWash.Core.Settings;
using CarWash.Core.Validation;
using CarWash.Entity.Entities;
using CarWash.Entity.Enums;
using CarWash.Repository.Repositories.Customers;
using CarWash.Repository.Repositories.Employees;
using CarWash.Repository.Repositories.Roles;
using CarWash.Repository.Repositories.Token;
using CarWash.Repository.Repositories.Users;
using CarWash.Repository.UnitOfWork;
using CarWash.Service.ServiceExtensions;
using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace CarWash.Service.Providers
{
    public class JwtGenerator
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<JwtGenerator> _logger;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        public JwtGenerator(IOptions<JwtSettings> jwtSettings, ILogger<JwtGenerator> logger, ICustomerRepository customerRepository, IRoleRepository roleRepository, IEmployeeRepository employeeRepository, IUserRepository userRepository, ITokenRepository tokenRepository, IUnitOfWork unitOfWork)
        {
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
            _customerRepository = customerRepository;
            _roleRepository = roleRepository;
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<ValidateTokenResult> ValidateToken(string token)
        {

            if (token == null)
                return new ValidateTokenResult(false, "Please provide valid token!");
            //var dbToken = await _userManager.GetAuthenticationTokenAsync(await _userManager.FindByIdAsync(GetClaim(token,"UserId")), "MyApp", "AccessToken");
            var user = await _userRepository.GetByIdAsync(int.Parse(GetClaim(token, "userid")));
            var result = await _tokenRepository.FindByCondition(t=> t.UserId == user.Id && t.TokenType == TokenTypes.AccessToken,false).FirstOrDefaultAsync();

            if (result == null)
            {
                return new ValidateTokenResult(false, "Token not found!");
            }
            if (!token.Equals(result.Token))
            {
                return new ValidateTokenResult(false, "Token not valid! Please login to get new token!");
            }

            //token = token.Split(' ')[1];
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);

                return new ValidateTokenResult(true, string.Empty, GetClaim(token, "userid"));
            }
            catch (SecurityTokenExpiredException)
            {
                // Token'ın süresi dolmuş
                var expiredUserId = int.Parse(GetClaim(token, "userid"));
                var expiredToken = await _tokenRepository
                    .FindByCondition(t => t.UserId == expiredUserId && t.TokenType == TokenTypes.AccessToken, false)
                    .FirstOrDefaultAsync();

                // Token'ı veritabanından sil
                _tokenRepository.Delete(expiredToken);
                await _unitOfWork.SaveChangesAsync(); // Değişiklikleri kaydet
                return new ValidateTokenResult(false, "Token has expired! Please login to get a new token!");
            }
            catch (Exception ex)
            {
                _logger.SendError(ex, "JwtGenerator");
                return new ValidateTokenResult(false, ex.Message);
            }
        }

        public async Task<string> GenerateJwt(User user,UserTypes userType,TokenTypes tokenType)
        {
            string token = "";

            try
            {
                var whoseToken = userType == UserTypes.Employee ? WhosToken.Employee : WhosToken.Customer;

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _jwtSettings.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                    new Claim("userid", user.Id.ToString()),
                    new Claim("whoseToken" , whoseToken.GetStringValue())
                };
                if (whoseToken == WhosToken.Employee)
                {
                    var employeRole = await _employeeRepository.GetEmployeeRole(user.Id);
                    var rolelaim = new Claim("role", employeRole);
                    claims.Add(rolelaim);
                }
                if (tokenType == TokenTypes.AccessToken)
                    claims.Add(
                        new Claim(JwtRegisteredClaimNames.Name, user.UserName)
                    );

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                var rawToken = new JwtSecurityToken(
                    _jwtSettings.Issuer,
                    _jwtSettings.Audience,
                    claims.ToArray(),
                    expires: DateTime.UtcNow.AddSeconds(_jwtSettings.Ttl),
                    signingCredentials: signIn);

                token = new JwtSecurityTokenHandler().WriteToken(rawToken);


                var updatedToken = await _tokenRepository.FindByCondition(t => t.UserId == user.Id && t.TokenType == TokenTypes.AccessToken && t.WhosToken == whoseToken, true).FirstOrDefaultAsync();

                if (updatedToken != null)
                {
                    updatedToken.Token = token;
                    _tokenRepository.Update(updatedToken);
                }
                else
                    await _tokenRepository.CreateAsync(new UserToken()
                    {
                        Token = token,
                        TokenType = TokenTypes.AccessToken,
                        UserId = user.Id,
                        WhosToken = whoseToken,
                    });

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.SendError(ex, "GenerateJwt");
            }
            return token;
        }
        

        public async Task<string> GenerateRefreshToken(User user, UserTypes userType)
        {
            string token = "";
            try
            {
                var whoseToken = userType == UserTypes.Employee ? WhosToken.Employee : WhosToken.Customer;
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var userRole = await _employeeRepository.GetEmployeeRole(user.Id);

                var rawToken = new JwtSecurityToken(
                    null,
                    null,
                    expires: DateTime.UtcNow.AddSeconds(_jwtSettings.Ttl),
                    signingCredentials: signIn,
                    claims: new Claim[] { new Claim("userid", user.Id.ToString()) 
                    });
                token = new JwtSecurityTokenHandler().WriteToken(rawToken);

                var updatedToken = await _tokenRepository.FindByCondition(t => t.UserId == user.Id && t.TokenType == TokenTypes.RefreshToken && t.WhosToken == whoseToken, true).FirstOrDefaultAsync();

                if (updatedToken != null)
                {
                    updatedToken.Token = token;
                    _tokenRepository.Update(updatedToken);
                }
                else
                    await _tokenRepository.CreateAsync(new UserToken()
                    {
                        Token = token,
                        TokenType = TokenTypes.RefreshToken,
                        UserId = user.Id,
                        WhosToken = whoseToken,
                    });

                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.SendError(ex, "GenerateRefreshToken");
            }

            return token;

        }


        public string GetClaim(string token, string claimType)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
                return stringClaimValue;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task Logout(User user)
        {
            var deletedAccessToken = await _tokenRepository.FindByCondition(t => t.UserId == user.Id && t.TokenType == TokenTypes.AccessToken,true).FirstOrDefaultAsync();
            var refreshToken = await _tokenRepository.FindByCondition(t => t.UserId == user.Id && t.TokenType == TokenTypes.RefreshToken, true).FirstOrDefaultAsync();
            if (deletedAccessToken != null) 
                _tokenRepository.Delete(deletedAccessToken);
            if (refreshToken != null)
                _tokenRepository.Delete(refreshToken);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
