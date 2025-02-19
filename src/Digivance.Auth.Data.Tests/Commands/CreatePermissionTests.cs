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
                .Setup(x => x.ExistsByNameAsync(It.IsAny<Guid?>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
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
        /// Helper method to build a mock ITenantService. Will return returnsTrue when you call ExistsAsync
        /// </summary>
        /// <param name="returnsTrue">The value to return for ExistsAsync</param>
        /// <returns>A mock implementation of ITenantService</returns>
        private ITenantService GetTenantService(bool returnsTrue)
        {
            var mock = new Mock<ITenantService>();

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
        private (IPermissionService, IScopeService, ITenantService) GetPassingServices()
        {
            return (
                GetPermissionService(false),
                GetScopeService(true),
                GetTenantService(true)
            );
        }

        [Test]
        public async Task CanBeValid()
        {
            var (p,s,t) = GetPassingServices();
            var validator = new CreatePermissionValidator(p, s, t);

            var command = new CreatePermission
            {
                Description = "Valid description",
                Name = "Irrelevant to this test",
                TenantId = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public async Task CanFailLongDescription()
        {
            var (p, s, t) = GetPassingServices();
            var validator = new CreatePermissionValidator(p, s, t);

            var description = "";
            for (var i = 0; i <= 4001; i++)
                description += "I'm a long string, too long one might say...";

            var command = new CreatePermission
            {
                Description = description,
                Name = "Irrelevant to this test",
                TenantId = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreatePermissionValidator.ERR_DESCRIPTION_TOO_LONG), Is.True);
        }

        [Test]
        public async Task CanFailMissingName()
        {
            var (p, s, t) = GetPassingServices();
            var validator = new CreatePermissionValidator(p, s, t);

            var command = new CreatePermission
            {
                Name = "",
                TenantId = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == "'Name' must not be empty."), Is.True);
        }

        [Test]
        public async Task CanFailDuplicateName()
        {
            // This p (permission service) will say the permission already exists which should
            // cause our validator to fail
            var p = GetPermissionService(true);
            var s = GetScopeService(true);
            var t = GetTenantService(true);
            var validator = new CreatePermissionValidator(p, s, t);

            var command = new CreatePermission
            {
                Name = "AlreadyExists",
                TenantId = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreatePermissionValidator.ERR_PERMISSION_NAME_EXISTS), Is.True);
        }

        [Test]
        public async Task CanFailInvalidScope()
        {
            var p = GetPermissionService(false);
            // This s (scope service) will say the scope doesn't exists which should
            // cause our validator to fail
            var s = GetScopeService(false);
            var t = GetTenantService(true);
            var validator = new CreatePermissionValidator(p, s, t);

            var command = new CreatePermission
            {
                Name = "AlreadyExists",
                ScopeId = Guid.NewGuid(),
                TenantId = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreatePermissionValidator.ERR_SCOPE_MUST_EXISTS), Is.True);
        }


        [Test]
        public async Task CanFailInvalidTenant()
        {
            var p = GetPermissionService(false);
            var s = GetScopeService(true);
            // This t (tenant service) will say the tenant doesn't exists which should
            // cause our validator to fail
            var t = GetTenantService(false);
            var validator = new CreatePermissionValidator(p, s, t);

            var command = new CreatePermission
            {
                Name = "AlreadyExists",
                ScopeId = Guid.NewGuid(),
                TenantId = Guid.NewGuid() // omg the validator will actually fail this finally
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreatePermissionValidator.ERR_TENANT_MUST_EXIST), Is.True);
        }
    }
}
