using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Entities;
using Digivance.Auth.Data.Models;
using Digivance.Auth.Data.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace Digivance.Auth.Data.EntityFramework.Services
{
    /// <summary>
    /// Configurable options for our EfAuthService, you should bind these
    /// to your builder.Configuration and store in appsettings / environment variables
    /// </summary>
    public record EfAuthServiceOptions
    {
        /// <summary>
        /// Timespan representing how long a generated bearer token is valid for
        /// </summary>
        public TimeSpan BearerExpiry { get; set; }

        /// <summary>
        /// The audience to encode to JWT tokens
        /// </summary>
        public string JwtAudience { get; set; }

        /// <summary>
        /// The issuer to encode to JWT tokens
        /// </summary>
        public string JwtIssuer { get; set; }

        /// <summary>
        /// Base64 encoded signing key to use when generating bearer tokens
        /// !! DO NOT STORE THIS IN YOUR REPO !!
        /// </summary>
        public string JwtSigningKey { get; set; }

        /// <summary>
        /// Timespan representing how long a refresh code is valid for
        /// </summary>
        public TimeSpan RefreshExpiry { get; set; }
    }

    /// <summary>
    /// Entity framework implementation of our AuthService
    /// </summary>
    /// <param name="authContext">The AuthContext to use</param>
    /// <param name="options">The EfAuthServiceOptions configuration</param>
    public class EfAuthService(AuthContext authContext, IOptions<EfAuthServiceOptions> options) : IAuthService
    {
        private readonly AuthContext authContext = authContext;
        private EfAuthServiceOptions options = options.Value;

        /// <inheritdoc />
        public async Task<AuthenticationResult> AuthenticateAsync(AuthenticateUserCredentials command, CancellationToken cancellationToken)
        {
            // We don't want to hint anything, throw these generic field error messages
            var failures = new ValidationFailure[]
            {
                new ValidationFailure("emailAddress", "Incorrect email address or password"),
                new ValidationFailure("password", "Incorrect email address or password")
            };

            var user = await authContext.UserAccounts
                .Where(x => x.EmailAddress == command.EmailAddress)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null || !PasswordHelper.Compare(command.Password, user.Password))
                throw new ValidationException("Authentication failed", failures);

            return new AuthenticationResult
            {
                BearerExpires = DateTime.UtcNow.Add(options.BearerExpiry),
                BearerToken = await GenerateJwtAsync(user, cancellationToken),
                RefreshCode = await GenerateRefreshCode(user, cancellationToken),
                RefreshExpires = DateTime.UtcNow.Add(options.RefreshExpiry)
            };
        }

        /// <inheritdoc />
        public async Task<AuthenticationResult> RefreshTokenAsync(string expiredJwt, string refreshCode, CancellationToken cancellationToken)
        {
            // We don't want to hint anything, throw these generic field error messages
            var failures = new ValidationFailure[]
            {
                new ValidationFailure("bearer", "Incorrect bearer token or refresh code"),
                new ValidationFailure("refreshCode", "Incorrect bearer token or refresh code")
            };

            var signingBytes = Convert.FromBase64String(options.JwtSigningKey);
            var key = new SymmetricSecurityKey(signingBytes);

            var validateJwt = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,

                ValidAudience = options.JwtAudience,
                ValidIssuer = options.JwtIssuer,
                IssuerSigningKey = key
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(expiredJwt, validateJwt, out SecurityToken token);

                var userIdValue = ((JwtSecurityToken)token).Claims.FirstOrDefault(x => x.Type == "userId")?.Value;
                var userId = Guid.Parse(userIdValue!); // Let it blow if this doesn't work

                // This enforces it...  throws if not found
                var existingCode = await authContext.UserRefreshCodes
                    .Include(x => x.User)
                    .Where(x => x.UserId == userId)
                    .Where(x => x.Code == refreshCode)
                    .FirstAsync(cancellationToken);

                authContext.UserRefreshCodes.Remove(existingCode);
                await authContext.SaveChangesAsync(cancellationToken);

                return new AuthenticationResult
                {
                    BearerExpires = DateTime.UtcNow.Add(options.BearerExpiry),
                    BearerToken = await GenerateJwtAsync(existingCode.User, cancellationToken),
                    RefreshCode = await GenerateRefreshCode(existingCode.User, cancellationToken),
                    RefreshExpires = DateTime.UtcNow.Add(options.RefreshExpiry)
                };
            }
            catch (Exception ex)
            {
                // Maybe should log it?
                throw new ValidationException("Failed to refresh authentication token", failures);
            }
        }

        /// <summary>
        /// Helper method that will generate the actual JWT bearer token
        /// </summary>
        /// <param name="user">The user entity to generate the token for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>JWT bearer token</returns>
        private async Task<string> GenerateJwtAsync(UserEntity user, CancellationToken cancellationToken)
        {
            var claims = new Dictionary<string, object?>
            {
                { "displayName", user.DisplayName },
                { "email", user.EmailAddress },
                { "username", user.Username ?? user.EmailAddress },
                { "userId", user.Id.ToString() }
            };

            var roleIds = await authContext.Roles
                .Where(x => x.Users.Any(u => u.UserId == user.Id))
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            var permissionsIds = await authContext.Permissions
                .Where(x =>
                    x.Users.Any(u => u.UserId == user.Id) ||
                    x.Roles.Any(r => roleIds.Contains(r.RoleId))
                )
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            if (roleIds.Any())
                claims.Add("roles", roleIds);

            if (permissionsIds.Any())
                claims.Add("permissions", permissionsIds);

            var signingBytes = Convert.FromBase64String(options.JwtSigningKey);
            var key = new SymmetricSecurityKey(signingBytes);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var descriptor = new SecurityTokenDescriptor
            {
                Audience = options.JwtAudience,
                Claims = claims,
                Expires = DateTime.UtcNow.Add(options.BearerExpiry),
                Issuer = options.JwtIssuer,
                SigningCredentials = credentials
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(descriptor);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        /// <summary>
        /// Helper method that will generate and persist a UserRefreshCode
        /// </summary>
        /// <param name="user">The user entity to generate the refresh code for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The refresh code itself (the string)</returns>
        private async Task<string> GenerateRefreshCode(UserEntity user, CancellationToken cancellationToken)
        {
            var expiredCodes = authContext.UserRefreshCodes
                .Where(x => x.Expires < DateTime.UtcNow);

            authContext.UserRefreshCodes.RemoveRange(expiredCodes);
            await authContext.SaveChangesAsync(cancellationToken);

            var code = new UserRefreshCodeEntity
            {
                Code = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.Add(options.RefreshExpiry),
                UserId = user.Id
            };

            authContext.UserRefreshCodes.Add(code);
            await authContext.SaveChangesAsync(cancellationToken);

            return code.Code;
        }
    }
}
