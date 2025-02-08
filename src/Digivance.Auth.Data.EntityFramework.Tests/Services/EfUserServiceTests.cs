using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Services;
using Digivance.Auth.Data.Models;
using Digivance.Auth.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;


namespace Digivance.Auth.Data.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EfUserServiceTests
    {
        private AsyncServiceScope scope;
        private ServiceProvider provider;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Test.json")
                .Build();

            // This tries to fetch a connection string (which isn’t needed for in-memory DBs).
            // In reality, all you need is a simple string name, not a full connection string.
            var authDbName = configuration.GetConnectionString("AuthContext") ??
                throw new ArgumentNullException("Missing AuthContext app setting");

            services
                .AddDbContext<AuthContext>(x =>
                    x.UseInMemoryDatabase(authDbName)
                );

            //Register service
            services.AddScoped<IUserService, EfUserService>();
            services.AddScoped<ITenantService, EfTenantService>();

            provider = services.BuildServiceProvider();
            scope = provider.CreateAsyncScope();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await scope.DisposeAsync();
            await provider.DisposeAsync();
        }

        [Test]
        public async Task Can_GetById()
        {
            var service = scope.ServiceProvider.GetService<IUserService>();
            var authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();

            var createdUser = await service!.CreateAsync(new CreateUser
            {
                DisplayName = "Test",
                EmailAddress = "test@gmail.com",
                Password = "@tesT123",
                Username = "Test",
                TenantId = Guid.NewGuid()
            }, default);

            var gettedUser = await service.GetByIdAsync(createdUser.Id, default);

            var deleteMe = await authContext.UserAccounts.FirstOrDefaultAsync(x => x.Id == createdUser.Id);
            authContext.UserAccounts.Remove(deleteMe!);

            Assert.That(createdUser, Is.Not.Null);
            Assert.That(gettedUser, Is.Not.Null);
            Assert.That(gettedUser, Is.EqualTo(createdUser));
        }

        [Test]
        public async Task Can_GetUserByEmail()
        {
            var service = scope.ServiceProvider.GetService<IUserService>();
            var authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
            var tenantId = Guid.NewGuid();

            var createdUser = await service!.CreateAsync(new CreateUser
            {
                DisplayName = "Test",
                EmailAddress = "test@gmail.com",
                Password = "@tesT123",
                Username = "Test",
                TenantId = tenantId
            }, default);

            var gettedUser = await service.GetByEmailAddressAsync(createdUser.EmailAddress, default);

            var deleteMe = await authContext.UserAccounts.FirstOrDefaultAsync(x => x.Id == createdUser.Id);
            authContext.UserAccounts.Remove(deleteMe!);

            Assert.That(createdUser, Is.Not.Null);
            Assert.That(gettedUser, Is.Not.Null);
            Assert.That(gettedUser, Is.EqualTo(createdUser));
        }

        [TestCase("invalid-email@gmail.com", false)]
        [TestCase("valid@email.com", true)]
        public async Task Can_EmailAddressExists(string address, bool expectValid)
        {
            var service = scope.ServiceProvider.GetService<IUserService>();
            var authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();

            var createdUser = await service!.CreateAsync(new CreateUser
            {
                DisplayName = "Test",
                EmailAddress = "valid@email.com",
                Password = "@tesT123",
                Username = "Test",
                TenantId = Guid.NewGuid()
            }, default);

            var exists = await service.EmailAddressExistsAsync(address, default);

            var deleteMe = await authContext.UserAccounts.FirstOrDefaultAsync(x => x.Id == createdUser.Id);
            authContext.UserAccounts.Remove(deleteMe!);

            Assert.That(createdUser, Is.Not.Null);

            if (expectValid)
            {
                Assert.That(exists, Is.True);
            }
            else
            {
                Assert.That(exists, Is.False);
            }
 
        }

        [Test]
        public async Task Can_GetUserByUsername()
        {
            var service = scope.ServiceProvider.GetService<IUserService>();
            var authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
            var tenantId = Guid.NewGuid();

            var createdUser = await service!.CreateAsync(new CreateUser
            {
                DisplayName = "Test",
                EmailAddress = "test@gmail.com",
                Password = "@tesT123",
                Username = "Test",
                TenantId = tenantId
            }, default);

            var gettedUser = await service.GetByUsernameAsync(createdUser.Username, default);

            var deleteMe = await authContext.UserAccounts.FirstOrDefaultAsync(x => x.Id == createdUser.Id);
            authContext.UserAccounts.Remove(deleteMe!);

            Assert.That(createdUser, Is.Not.Null);
            Assert.That(gettedUser, Is.Not.Null);
            Assert.That(gettedUser, Is.EqualTo(createdUser));
        }

        [TestCase("Invalid", false)]
        [TestCase("Valid", true)]
        public async Task Can_UserNameExists(string username, bool expectValid)
        {
            var service = scope.ServiceProvider.GetService<IUserService>();
            var authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();

            var createdUser = await service!.CreateAsync(new CreateUser
            {
                DisplayName = "Test",
                EmailAddress = "valid@email.com",
                Password = "@tesT123",
                Username = "Valid",
                TenantId = Guid.NewGuid()
            }, default);

            var exists = await service.UsernameExistsAsync(username, default);

            var deleteMe = await authContext.UserAccounts.FirstOrDefaultAsync(x => x.Id == createdUser.Id);
            authContext.UserAccounts.Remove(deleteMe!);

            Assert.That(createdUser, Is.Not.Null);

            if (expectValid)
            {
                Assert.That(exists, Is.True);
            }
            else
            {
                Assert.That(exists, Is.False);
            }

        }
    }
}
