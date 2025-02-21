using Digivance.Auth.Data.Commands;

namespace Digivance.Auth.Data.Tests.Commands
{
    [TestFixture]
    public class UpdatePermissionTests
    {
        [Test]
        public async Task CanFail_LongDescription()
        {
            var validator = new UpdatePermissionValidator();

            var description = "";
            for (var i = 0; i <= 4001; i++)
                description += "I'm a long string, too long one might say...";

            var command = new UpdatePermission
            {
                Description = description,
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdatePermissionValidator.ERR_DESCRIPTION_TOO_LONG), Is.True);
        }
    }
}
