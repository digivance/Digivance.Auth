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
    public class UpdateTenantTests
    {
        /// <summary>
        /// Helper method to build a mock ITenantService. Will return returnsTrue when you call
        /// ExistsByNameAsync
        /// </summary>
        /// <param name="returnsTrue">The value to return for ExistsByNameAsync</param>
        /// <returns>A mock implementation of ITenantService</returns>
        private ITenantService GetTenantService(bool returnsTrue, bool returnsTrue1)
        {
            var mock = new Mock<ITenantService>();

            mock
                .Setup(x => x.ExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnsTrue));

            mock
                .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnsTrue1));

            return mock.Object;
        }

        [Test]
        public async Task Can_BeValid()
        {
            var service = GetTenantService(false, true);
            var validator = new UpdateTenantValidator(service);

            var command = new UpdateTenant
            {
                Description = "Valid description",
                Name = "Irrelevant to this test"
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public async Task CanFail_LongDescription()
        {
            var service = GetTenantService(false, true);
            var validator = new UpdateTenantValidator(service);

            var description = "";
            for (var i = 0; i <= 4001; i++)
                description += "I'm a long string, too long one might say...";

            var command = new UpdateTenant
            {
                Description = description,
                Name = "Irrelevant to this test"
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdateTenantValidator.ERR_DESCRIPTION_LENGTH), Is.True);
        }

        [Test]
        public async Task CanFail_LongName()
        {
            var service = GetTenantService(false, true);
            var validator = new UpdateTenantValidator(service);

            var name = "";
            for (var i = 0; i <= 101; i++)
                name += "I'm a long string, too long but not comparatively atleast...";

            var command = new UpdateTenant
            {
                Description = "Irrelevant to this test",
                Name = name
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdateTenantValidator.ERR_TENANTNAME_LENGTH), Is.True);
        }

        [Test]
        public async Task CanFail_MissingName()
        {
            var service = GetTenantService(false, true);
            var validator = new UpdateTenantValidator(service);

            var command = new UpdateTenant
            {
                Description = "Valid description",
                Name = ""
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdateTenantValidator.ERR_TENANT_NAME_EMPTY), Is.True);
        }

        [Test]
        public async Task CanFail_DuplicateName()
        {
            // This (tenant service) will say the tenant already exists which should
            // cause our validator to fail
            var service = GetTenantService(true, true);
            var validator = new UpdateTenantValidator(service);

            var command = new UpdateTenant
            {
                Name = "Already Exists"
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdateTenantValidator.ERR_TENANTNAME_UNIQUE), Is.True);
        }

        [Test]
        public async Task CanFail_IDNotExists()
        {
            var service = GetTenantService(false, false);
            var validator = new UpdateTenantValidator(service);

            var command = new UpdateTenant
            {
                Name = "Already Exists",
                Id = Guid.NewGuid()
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdateTenantValidator.ERR_TENANTID_NOTFOUND), Is.True);
        }
    }
}
