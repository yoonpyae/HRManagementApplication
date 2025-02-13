using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HRManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HRManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public UsersController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration config,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _context = context;
        }



        [HttpPost("register")]
        [AllowAnonymous]
        [EndpointSummary("Register User")]
        [EndpointDescription("Register User")]
        public async Task<IActionResult> Register([FromBody] LoginModel model)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == model.Username))
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Username already exists"
                });
            }

            IdentityUser user = new()
            {
                UserName = model.Username
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            return !result.Succeeded
                ? BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Failed to create user"
                })
                : Ok(new DefaultResponseModel()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "User created successfully"
                });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [EndpointSummary("User Login")]
        [EndpointDescription("User Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginModel model)
        {
            IdentityUser? user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized(new DefaultResponseModel()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Invalid username or password"
                });
            }

            (string token, string refreshToken, DateTime refreshTokenExpiry) = await GetTokens(user);
            return Ok(new DefaultResponseModel()
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiry = refreshTokenExpiry
                },
                Message = "Login successful"
            });
        }

        private async Task<(string, string, DateTime)> GetTokens(IdentityUser user)
        {
            IConfigurationSection jwtSettings = _config.GetSection("Jwt");
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            JwtSecurityToken token = new(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: credentials
            );

            string refreshToken = Guid.NewGuid().ToString();
            DateTime refreshTokenExpiry = DateTime.Now.AddDays(Convert.ToDouble(jwtSettings["RefreshTokenExpiryInDays"]));

            _ = await _userManager.SetAuthenticationTokenAsync(user, "MyApp", "RefreshToken", refreshToken);
            _ = await _userManager.SetAuthenticationTokenAsync(user, "MyApp", "RefreshTokenExpiry", refreshTokenExpiry.ToString());

            return (new JwtSecurityTokenHandler().WriteToken(token), refreshToken, refreshTokenExpiry);
        }

        private (string, DateTime) GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            string refreshToken = Convert.ToBase64String(randomNumber);
            DateTime refreshTokenExpiry = DateTime.UtcNow.AddMinutes(3); // Set refresh token to expire in 3 minutes
            return (refreshToken, refreshTokenExpiry);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [EndpointSummary("Refresh Token")]
        [EndpointDescription("Refresh Token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel tokenRequest)
        {
            if (tokenRequest is null)
            {
                return BadRequest("Invalid client request");
            }

            ClaimsPrincipal? principal = GetPrincipalFromExpiredToken(tokenRequest.Access_Token);
            if (principal is null)
            {
                return BadRequest("Invalid client request");
            }

            string? userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return BadRequest("Invalid client request");
            }

            IdentityUser? user = await _userManager.FindByIdAsync(userId);
            if (user is null || !await ValidateRefreshToken(user, tokenRequest.Refresh_Token))
            {
                return BadRequest("Invalid client request");
            }


            (string newAccessToken, string newRefreshToken, DateTime newRefreshTokenExpiry) = await GetTokens(user);
            return Ok(new
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiry = newRefreshTokenExpiry
            });
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                TokenValidationParameters tokenValidationParameters = new()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found"))),
                    ValidateLifetime = false // Allow expired tokens
                };

                JwtSecurityTokenHandler tokenHandler = new();
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

                if (securityToken is not JwtSecurityToken jwtToken || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null; // Invalid token
                }

                // ✅ Check if the token is expired
                Claim? expiryClaim = principal.FindFirst(JwtRegisteredClaimNames.Exp);
                if (expiryClaim != null && long.TryParse(expiryClaim.Value, out long expiry))
                {
                    DateTime expiryDate = DateTimeOffset.FromUnixTimeSeconds(expiry).UtcDateTime;
                    if (expiryDate > DateTime.UtcNow)
                    {
                        return null; // Token is still valid, no need to refresh
                    }
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private async Task<bool> ValidateRefreshToken(IdentityUser user, string refreshToken)
        {
            string? storedRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, "MyApp", "RefreshToken");
            string? storedRefreshTokenExpiry = await _userManager.GetAuthenticationTokenAsync(user, "MyApp", "RefreshTokenExpiry");

            if (string.IsNullOrEmpty(storedRefreshToken) || string.IsNullOrEmpty(storedRefreshTokenExpiry))
            {
                return false;
            }

            return DateTime.TryParse(storedRefreshTokenExpiry, out DateTime expiryDate) && expiryDate >= DateTime.UtcNow
&& storedRefreshToken == refreshToken;
        }
    }
}
