using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.Services;
using Digivance.Auth.Data.Tests.Services;

namespace Digivance.Auth.Data.Tests.Commands
{
    [TestFixture]
    public class CreateUserTests
    {
        private IUserService userService;
        private CreateUserValidator validator;
        
        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            userService = new InMemoryUserService();
            validator = new CreateUserValidator(userService);

            var command = new CreateUser
            {
                DisplayName = "Existing User",
                EmailAddress = "existing@email.com",
                Password = "password",
                Username = "ExistingUsername"
            };

            await userService.CreateAsync(command, default);
        }

        [Test]
        public async Task Can_BeValid()
        {
            var command = new CreateUser
            {
                DisplayName = "New User",
                EmailAddress = "new@email.com",
                Password = "MustB3@G00dP@SS",
                Username = "new"
            };

            var res = await validator.ValidateAsync(command, default);
            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public async Task CanFail_ExistingUser()
        {
            var command = new CreateUser { EmailAddress = "existing@email.com", Username = "ExistingUsername" };
            var res = await validator.ValidateAsync(command);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == CreateUserValidator.ERR_EMAIL_UNIQUE), Is.True);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == CreateUserValidator.ERR_USERNAME_UNIQUE), Is.True);
        }

        [Test]
        public async Task CanFail_InvalidEmail()
        {
            var command = new CreateUser { EmailAddress = "not a real email address" };
            var res = await validator.ValidateAsync(command);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == CreateUserValidator.ERR_EMAIL_INVALID), Is.True);
        }

        [Test]
        public async Task CanFail_InvalidPassword()
        {
            var command = new CreateUser { Password = " " };
            var res = await validator.ValidateAsync(command);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == CreateUserValidator.ERR_PASSWORD_LOWERCASE), Is.True);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == CreateUserValidator.ERR_PASSWORD_LENGTH), Is.True);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == CreateUserValidator.ERR_PASSWORD_NUMBER), Is.True);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == CreateUserValidator.ERR_PASSWORD_SPECIAL), Is.True);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == CreateUserValidator.ERR_PASSWORD_UPPERCASE), Is.True);
        }

        [Test]
        public async Task CanFail_LongFields()
        {
            var longName = "";
            for (var i = 0; i <= 255; i++)
                longName += "Super long name - ";

            var command = new CreateUser { DisplayName = longName, Username = longName };
            var res = await validator.ValidateAsync(command);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == CreateUserValidator.ERR_DISPLAYNAME_LENGTH), Is.True);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == CreateUserValidator.ERR_USERNAME_LENGTH), Is.True);
        }

        [Test]
        public async Task CanFail_MissingFields()
        {
            var command = new CreateUser();
            var res = await validator.ValidateAsync(command);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == CreateUserValidator.ERR_EMAIL_REQUIRED), Is.True);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == CreateUserValidator.ERR_PASSWORD_REQUIRED), Is.True);
        }
    }
}
