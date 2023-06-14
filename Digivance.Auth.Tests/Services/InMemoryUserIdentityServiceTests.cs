using Digivance.Auth.Commands;
using Digivance.Auth.Services;
using NUnit.Framework;

namespace Digivance.Auth.Tests.Services
{
    [TestFixture]
    public class InMemoryUserIdentityServiceTests
    {
        [Test]
        public async Task Can_Create()
        {
            var service = new InMemoryUserIdentityService();

            var command = new CreateUserIdentity
            {
                EmailAddress = "test@site.com",
                FirstName = "Test",
                LastName = "User"
            };

            var userIdentity = await service.CreateUserIdentityAsync(command);
            await service.DeleteUserIdentityAsync(userIdentity.Id);

            Assert.That(userIdentity.EmailAddress, Is.EqualTo(command.EmailAddress));
            Assert.That(userIdentity.FirstName, Is.EqualTo(command.FirstName));
            Assert.That(userIdentity.LastName, Is.EqualTo(command.LastName));
        }

        [Test]
        public async Task Can_Delete()
        {
            var service = new InMemoryUserIdentityService();

            var command = new CreateUserIdentity
            {
                EmailAddress = "test@site.com",
                FirstName = "Test",
                LastName = "User"
            };

            var userIdentity = await service.CreateUserIdentityAsync(command);
            await service.DeleteUserIdentityAsync(userIdentity.Id);

            Assert.ThrowsAsync<Exception>(async () => await service.GetUserIdentityAsync(userIdentity.Id));
        }

        [Test]
        public async Task Can_Get()
        {
            var service = new InMemoryUserIdentityService();

            var command = new CreateUserIdentity
            {
                EmailAddress = "test@site.com",
                FirstName = "Test",
                LastName = "User"
            };

            var userIdentity = await service.CreateUserIdentityAsync(command);
            var readIdentity = await service.GetUserIdentityAsync(userIdentity.Id);
            await service.DeleteUserIdentityAsync(userIdentity.Id);

            Assert.That(userIdentity.EmailAddress, Is.EqualTo(readIdentity.EmailAddress));
            Assert.That(userIdentity.FirstName, Is.EqualTo(readIdentity.FirstName));
            Assert.That(userIdentity.LastName, Is.EqualTo(readIdentity.LastName));
        }


        [Test]
        public async Task Can_Update()
        {
            var service = new InMemoryUserIdentityService();

            var createCommand = new CreateUserIdentity
            {
                EmailAddress = "test@site.com",
                FirstName = "Test",
                LastName = "User"
            };

            var userIdentity = await service.CreateUserIdentityAsync(createCommand);

            var updateCommand = new UpdateUserIdentity
            {
                EmailAddress = userIdentity.EmailAddress,
                FirstName = "User",
                LastName = "Test"
            };

            var readIdentity = await service.UpdateAsync(updateCommand);
            await service.DeleteUserIdentityAsync(userIdentity.Id);

            Assert.That(readIdentity.EmailAddress, Is.EqualTo(updateCommand.EmailAddress));
            Assert.That(readIdentity.FirstName, Is.EqualTo(updateCommand.FirstName));
            Assert.That(readIdentity.LastName, Is.EqualTo(updateCommand.LastName));
        }
    }
}
