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
    public class CreateScopeTests
    {
        /// <summary>
        /// Helper method to build a mock IScopeService. Will return returnsTrue when you call
        /// ExistsByNameAsync
        /// </summary>
        /// <param name="returnsTrue">The value to return for ExistsBeNameAsync</param>
        /// <returns>A mock implementation of IScopeService</returns>
        private IScopeService GetScopeService(bool returnsTrue)
        {
            var mock = new Mock<IScopeService>();

            mock
                .Setup(x => x.ExistsByNameAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
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
        /// the service methods.  These 2 services will always "pass"
        /// </summary>
        /// <returns>The two services needed to construct the validator</returns>
        private (IScopeService, ITenantService) GetPassingServices()
        {
            return (
                GetScopeService(false),
                GetTenantService(true)
            );
        }

        [Test]
        public async Task Can_BeValid()
        {
            var (s, t) = GetPassingServices();
            var validator = new CreateScopeValidator(s, t);

            var command = new CreateScope
            {
                Description = "Valid description",
                Name = "Irrelevant to this test",
                TenantId = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public async Task CanFail_LongDescription()
        {
            var (s, t) = GetPassingServices();
            var validator = new CreateScopeValidator(s, t);

            var description = "";
            for (var i = 0; i <= 4001; i++)
                description += "I'm a long string, too long one might say...";

            var command = new CreateScope
            {
                Description = description,
                Name = "Irrelevant to this test",
                TenantId = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreateScopeValidator.ERR_DESCRIPTION_TOO_LONG), Is.True);
        }

        [Test]
        public async Task CanFail_MissingName()
        {
            var (s, t) = GetPassingServices();
            var validator = new CreateScopeValidator(s, t);

            var command = new CreateScope
            {
                Name = "",
                TenantId = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == "'Name' must not be empty."), Is.True);
        }

        [Test]
        public async Task CanFail_DuplicateName()
        {
            // This s (Scope service) will say the Scope already exists which should
            // cause our validator to fail
            var s = GetScopeService(true);
            var t = GetTenantService(true);
            var validator = new CreateScopeValidator(s, t);

            var command = new CreateScope
            {
                Name = "AlreadyExists",
                TenantId = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreateScopeValidator.ERR_SCOPE_NAME_EXISTS), Is.True);
        }


        [Test]
        public async Task CanFail_InvalidTenant()
        {
            var s = GetScopeService(false);
            // This t (tenant service) will say the tenant doesn't exists which should
            // cause our validator to fail
            var t = GetTenantService(false);
            var validator = new CreateScopeValidator(s, t);

            var command = new CreateScope
            {
                Name = "AlreadyExists",
                TenantId = Guid.NewGuid() // omg the validator will actually fail this finally
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreateScopeValidator.ERR_TENANT_MUST_EXIST), Is.True);
        }
    }
}
