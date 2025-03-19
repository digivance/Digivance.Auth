using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.Services;
using Moq;

namespace Digivance.Auth.Data.Tests.Commands
{
    [TestFixture]
    public class UpdatePermissionTests
    {
        /// <summary>
        /// Helper method to build a mock IPermissionService. Will return returnsTrue when you call
        /// ExistsAsync
        /// </summary>
        /// <param name="returnsTrue">The value to return for ExistsBeNameAsync</param>
        /// <returns>A mock implementation of IPermissionService</returns>
        private IPermissionService GetPermissionService(bool returnsTrue)
        {
            var mock = new Mock<IPermissionService>();

            mock
               .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
               .Returns(Task.FromResult(returnsTrue));

            return mock.Object;
        }
        [Test]
        public async Task CanFail_LongDescription()
        {
            var permissionService = GetPermissionService(true);
            var validator = new UpdatePermissionValidator(permissionService);

            var description = "";
            for (var i = 0; i <= 4001; i++)
                description += "I'm a long string, too long one might say...";

            var command = new UpdatePermission
            {
                Description = description,
                Id = Guid.NewGuid()
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdatePermissionValidator.ERR_DESCRIPTION_TOO_LONG), Is.True);
        }

        [Test]
        public async Task CanFail_IDNotExists()
        {
            var permissionService = GetPermissionService(false);
            var validator = new UpdatePermissionValidator(permissionService);

            var command = new UpdatePermission
            {
                Description = "Already Exists",
                Id = Guid.NewGuid()
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdatePermissionValidator.ERR_PERMISSIONID_NOTFOUND), Is.True);
        }
    }
}
