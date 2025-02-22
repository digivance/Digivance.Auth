using Digivance.Auth.Data.Commands;

namespace Digivance.Auth.Data.Tests.Commands
{
    [TestFixture]
    public class AuthenticateUserCredentialsTests
    {
        [Test]
        public async Task CanFail_InvalidEmail()
        {
            var validator = new AuthenticateUserCredentialsValidator();
            var command = new AuthenticateUserCredentials { EmailAddress = "not a real email address" };
            var res = await validator.ValidateAsync(command);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.Any(x => x.ErrorMessage == AuthenticateUserCredentialsValidator.ERR_EMAIL_INVALID), Is.True);
        }
    }
}
