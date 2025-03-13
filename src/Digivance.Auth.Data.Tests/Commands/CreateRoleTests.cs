using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digivance.Auth.Data.Tests.Commands
{
    [TestFixture]
    public class CreateRoleTests
    {
        /// <summary>
        /// Helper method to build a mock IRoleService. Will return returnsTrue when you call
        /// ExistsByNameAsync
        /// </summary>
        /// <param name="returnsTrue">The value to return for ExistsBeNameAsync</param>
        /// <returns>A mock implementation of IRoleService</returns>
        private IRoleService GetRoleService(bool returnsTrue)
        {
            var mock = new Mock<IRoleService>();

            mock
                .Setup(x => x.ExistsByNameAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
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
                .Setup(x => x.ExistsAsync(It.IsAny<Guid?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnsTrue));

            return mock.Object;
        }

        /// <summary>
        /// Helper because multiple tests want the validator to just assume passing conditions for
        /// the service methods.  These 3 services will always "pass"
        /// </summary>
        /// <returns>The three services needed to construct the validator</returns>
        private (IRoleService, IScopeService, ITenantService) GetPassingServices()
        {
            return (
                GetRoleService(false),
                GetScopeService(true),
                GetTenantService(true)
            );
        }

        [Test]
        public async Task Can_BeValid()
        {
            var (r, s, t) = GetPassingServices();
            var validator = new CreateRoleValidator(r, s, t);

            var command = new CreateRole
            {
                Description = "Valid description",
                Name = "Irrelevant to this test",
                ScopeId = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public async Task CanFail_LongDescription()
        {
            var (r, s, t) = GetPassingServices();
            var validator = new CreateRoleValidator(r, s, t);

            var description = "";
            for (var i = 0; i <= 4001; i++)
                description += "I'm a long string, too long one might say...";

            var command = new CreateRole
            {
                Description = description,
                Name = "Irrelevant to this test",
                ScopeId = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreateRoleValidator.ERR_DESCRIPTION_TOO_LONG), Is.True);
        }

        [Test]
        public async Task CanFail_MissingName()
        {
            var (r, s, t) = GetPassingServices();
            var validator = new CreateRoleValidator(r, s, t);

            var command = new CreateRole
            {
                Name = "",
                ScopeId = Guid.NewGuid(), // doesn't matter mocked service will pass it
                TenantId = Guid.NewGuid()
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreateRoleValidator.ERR_ROLE_NAME_EMPTY), Is.True);
        }

        [Test]
        public async Task CanFail_LongName()
        {
            var (r, s, t) = GetPassingServices();
            var validator = new CreateRoleValidator(r, s, t);

            var name = "";
            for (var i = 0; i <= 101; i++)
                name += "I'm a long string, too long but not comparatively atleast...";

            var command = new CreateRole
            {
                Description = "Irrelevant to this test",
                Name = name,
                ScopeId = Guid.NewGuid()
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreateRoleValidator.ERR_ROLENAME_LENGTH), Is.True);
        }

        [Test]
        public async Task CanFail_DuplicateName()
        {
            // This r (Role service) will say the Role already exists which should
            // cause our validator to fail
            var r = GetRoleService(true);
            var s = GetScopeService(true);
            var t = GetTenantService(true);
            var validator = new CreateRoleValidator(r, s, t);

            var command = new CreateRole
            {
                Name = "AlreadyExists",
                ScopeId = Guid.NewGuid(), // doesn't matter mocked service will pass it
                TenantId = Guid.NewGuid()
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreateRoleValidator.ERR_ROLE_NAME_EXISTS), Is.True);
        }

        [Test]
        public async Task CanFail_InvalidScope()
        {
            var r = GetRoleService(false);
            // This s (scope service) will say the scope doesn't exists which should
            // cause our validator to fail
            var s = GetScopeService(false);
            var t = GetTenantService(true);
            var validator = new CreateRoleValidator(r, s, t);

            var command = new CreateRole
            {
                Name = "AlreadyExists",
                ScopeId = Guid.NewGuid(),
                TenantId = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreateRoleValidator.ERR_SCOPE_MUST_EXISTS), Is.True);
        }


        [Test]
        public async Task CanFail_InvalidTenant()
        {
            var r = GetRoleService(false);
            var s = GetScopeService(true);
            // This t (tenant service) will say the tenant doesn't exists which should
            // cause our validator to fail
            var t = GetTenantService(false);
            var validator = new CreateRoleValidator(r, s, t);

            var command = new CreateRole
            {
                Name = "AlreadyExists",
                ScopeId = Guid.NewGuid(),
                TenantId = Guid.NewGuid() // omg the validator will actually fail this finally
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreateRoleValidator.ERR_TENANT_MUST_EXIST), Is.True);
        }
    }
}
