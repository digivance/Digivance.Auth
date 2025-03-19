using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.Services;
using Moq;

namespace Digivance.Auth.Data.Tests.Commands
{
    [TestFixture]
    public class CreatePermissionTests
    {
        /// <summary>
        /// Helper method to build a mock IPermissionService. Will return returnsTrue when you call
        /// ExistsByNameAsync
        /// </summary>
        /// <param name="returnsTrue">The value to return for ExistsBeNameAsync</param>
        /// <returns>A mock implementation of IPermissionService</returns>
        private IPermissionService GetPermissionService(bool returnsTrue)
        {
            var mock = new Mock<IPermissionService>();

            mock
                .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnsTrue));

            return mock.Object;
        }

        /// <summary>
        /// Helper method to build a mock IScopeService. Will return returnsTrue when you call ExistsAsync
        /// </summary>
        /// <param name="returnsTrue">The value to return for ExistsAsync</param>
        /// <returns>A mock implementation of IScopeService</returns>
        private IScopeService GetScopeService(bool returnsTrue)
        {
            var mock = new Mock<IScopeService>();

            mock
                .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnsTrue));

            return mock.Object;
        }

        /// <summary>
        /// Helper because multiple tests want the validator to just assume passing conditions for
        /// the service methods.  These 3 services will always "pass"
        /// </summary>
        /// <returns>The three services needed to construct the validator</returns>
        private (IPermissionService, IScopeService) GetPassingServices()
        {
            return (
                GetPermissionService(false),
                GetScopeService(true)
            );
        }

        [Test]
        public async Task Can_BeValid()
        {
            var (p,s) = GetPassingServices();
            var validator = new CreatePermissionValidator(p, s);

            var command = new CreatePermission
            {
                Description = "Valid description",
                EntityAccess = "Read",
                EntityType = "Blog",
                ScopeId = Guid.NewGuid()
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public async Task CanFail_LongDescription()
        {
            var (p, s) = GetPassingServices();
            var validator = new CreatePermissionValidator(p, s);

            var description = "";
            for (var i = 0; i <= 4001; i++)
                description += "I'm a long string, too long one might say...";

            var command = new CreatePermission
            {
                Description = description,
                EntityAccess = "Read",
                EntityType = "Blog",
                ScopeId = Guid.NewGuid()
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreatePermissionValidator.ERR_DESCRIPTION_TOO_LONG), Is.True);
        }

        [Test]
        public async Task CanFail_InvalidScope()
        {
            var p = GetPermissionService(false);
            // This s (scope service) will say the scope doesn't exists which should
            // cause our validator to fail
            var s = GetScopeService(false);
            var validator = new CreatePermissionValidator(p, s);

            var command = new CreatePermission
            {
                EntityAccess = "Read",
                EntityType = "Blog",
                ScopeId = Guid.NewGuid()
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreatePermissionValidator.ERR_SCOPE_MUST_EXISTS), Is.True);
        }
    }
}
