using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.Services;
using Digivance.Auth.Data.Tests.Services;

namespace Digivance.Auth.Data.Tests.Commands
{
    [TestFixture]
    public class UpdateUserTests
    {
        private Guid userId;
        private IUserService userService;
        private UpdateUserValidator validator;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            userService = new InMemoryUserService();
            validator = new UpdateUserValidator(userService);

            var command = new CreateUser
            {
                DisplayName = "Existing User",
                EmailAddress = "existing@email.com",
                Password = "password",
                Username = "ExistingUsername"
            };

            var user = await userService.CreateAsync(command, default);
            userId = user.Id;
        }

        [Test]
        public async Task Can_BeValid()
        {
            var command = new UpdateUser
            {
                DisplayName = "New User",
                Id = userId,
                Username = "new"
            };

            var res = await validator.ValidateAsync(command, default);
            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public async Task CanFail_ExistingUser()
        {
            var command = new UpdateUser { Username = "ExistingUsername" };
            var res = await validator.ValidateAsync(command);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == CreateUserValidator.ERR_USERNAME_UNIQUE), Is.True);
        }

        [Test]
        public async Task CanFail_LongFields()
        {
            var longName = "";
            for (var i = 0; i <= 255; i++)
                longName += "Super long name - ";

            var command = new UpdateUser { DisplayName = longName, Username = longName };
            var res = await validator.ValidateAsync(command);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == CreateUserValidator.ERR_DISPLAYNAME_LENGTH), Is.True);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == CreateUserValidator.ERR_USERNAME_LENGTH), Is.True);
        }
    }
}