using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Entities;
using Digivance.Auth.Data.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Digivance.Auth.Data.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EfAuthServiceTests
    {
        private EfAuthService authService;
        private AuthContext context;
        private UserEntity user;

        [OneTimeSetUp]
        public async Task OnetimeSetup()
        {
            var contextOptions = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase("EfAuthServiceTests")
                .Options;

            var serviceOptions = new EfAuthServiceOptions
            {
                BearerExpiry = TimeSpan.FromMinutes(15),
                JwtAudience = "AuthTestAudience",
                JwtIssuer = "AuthTestIssuer",
                JwtSigningKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
                RefreshExpiry = TimeSpan.FromHours(4)
            };

            context = new AuthContext(contextOptions);
            authService = new EfAuthService(context, Options.Create(serviceOptions));
            user = new UserEntity
            {
                DisplayName = "Display Name",
                EmailAddress = "user@site.com",
                Password = PasswordHelper.Hash("RandoPass"),
                Permissions = [
                    new UserPermissionEntity
                    {
                        Permission = new PermissionEntity
                        {
                            Description = "Explicit permission",
                            Name = "Explicit"
                        }
                    }
                ],
                Roles = [
                    new UserRoleEntity
                    {
                        Role = new RoleEntity
                        {
                            Description = "Test Role",
                            Name = "TestRole",
                            Permissions = [
                                new RolePermissionEntity
                                {
                                    Permission = new PermissionEntity
                                    {
                                        Description = "Role assigned permission",
                                        Name = "Assigned"
                                    }
                                }
                            ]
                        }
                    }
                ],
                Username = "TestuserName"
            };

            context.UserAccounts.Add(user);
            await context.SaveChangesAsync(default);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            context.Dispose();
        }

        [Test]
        public async Task Can_AuthenticateUserCredentials()
        {
            var command = new AuthenticateUserCredentials
            {
                EmailAddress = user.EmailAddress,
                Password = "RandoPass"
            };

            var res = await authService.AuthenticateAsync(command, default);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(res.BearerToken);

            Assert.Multiple(() =>
            {
                // Basic expectations
                Assert.That(token.Claims.Any(x => x.Type == "displayName" && x.Value == user.DisplayName), Is.True);
                Assert.That(token.Claims.Any(x => x.Type == "email" && x.Value == user.EmailAddress), Is.True);
                Assert.That(token.Claims.Any(x => x.Type == "userId" && x.Value == user.Id.ToString()), Is.True);
                Assert.That(token.Claims.Any(x => x.Type == "username" && x.Value == user.Username), Is.True);

                // Permissions we expect in the token.Claims
                var assignedPermissionId = user.Roles.First().Role.Permissions.First().PermissionId.ToString();
                var explicitPermissionId = user.Permissions.First().PermissionId.ToString();

                Assert.That(token.Claims.Any(x => x.Type == "permissions" && x.Value == assignedPermissionId), Is.True);
                Assert.That(token.Claims.Any(x => x.Type == "permissions" && x.Value == explicitPermissionId), Is.True);

                // Roles we expect in the token.Claims
                var roleId = user.Roles.First().RoleId.ToString();

                Assert.That(token.Claims.Any(x => x.Type == "roles" && x.Value == roleId), Is.True);
            });
        }

        [Test]
        public async Task Can_RefreshToken()
        {
            var signInCommand = new AuthenticateUserCredentials
            {
                EmailAddress = user.EmailAddress,
                Password = "RandoPass"
            };

            var signInRes = await authService.AuthenticateAsync(signInCommand, default);

            var refresh = await context.UserRefreshCodes
                .FirstOrDefaultAsync(x => x.UserId == user.Id, default);

            var res = await authService.RefreshTokenAsync(signInRes.BearerToken, refresh!.Code, default);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(res.BearerToken);

            Assert.Multiple(() =>
            {
                // Basic expectations
                Assert.That(token.Claims.Any(x => x.Type == "displayName" && x.Value == user.DisplayName), Is.True);
                Assert.That(token.Claims.Any(x => x.Type == "email" && x.Value == user.EmailAddress), Is.True);
                Assert.That(token.Claims.Any(x => x.Type == "userId" && x.Value == user.Id.ToString()), Is.True);
                Assert.That(token.Claims.Any(x => x.Type == "username" && x.Value == user.Username), Is.True);

                // Permissions we expect in the token.Claims
                var assignedPermissionId = user.Roles.First().Role.Permissions.First().PermissionId.ToString();
                var explicitPermissionId = user.Permissions.First().PermissionId.ToString();

                Assert.That(token.Claims.Any(x => x.Type == "permissions" && x.Value == assignedPermissionId), Is.True);
                Assert.That(token.Claims.Any(x => x.Type == "permissions" && x.Value == explicitPermissionId), Is.True);

                // Roles we expect in the token.Claims
                var roleId = user.Roles.First().RoleId.ToString();

                Assert.That(token.Claims.Any(x => x.Type == "roles" && x.Value == roleId), Is.True);
            });
        }
    }
}
