using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Services;
using Digivance.Auth.Data.Services;
using Digivance.Auth.Data.Tests.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digivance.Auth.Data.Tests.Commands
{
    [TestFixture]
    public class CreateTenantTests
    {
        /// <summary>
        /// Helper method to build a mock ITenantService. Will return returnsTrue when you call
        /// ExistsByNameAsync
        /// </summary>
        /// <param name="returnsTrue">The value to return for ExistsByNameAsync</param>
        /// <returns>A mock implementation of ITenantService</returns>
        private ITenantService GetTenantService(bool returnsTrue)
        {
            var mock = new Mock<ITenantService>();

            mock
                .Setup(x => x.ExistsByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnsTrue));

            return mock.Object;
        }

        [Test]
        public async Task Can_BeValid()
        {
            var service = GetTenantService(false);
            var validator = new CreateTenantValidator(service);

            var command = new CreateTenant
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
            var service = GetTenantService(false);
            var validator = new CreateTenantValidator(service);

            var description = "";
            for (var i = 0; i <= 4001; i++)
                description += "I'm a long string, too long one might say...";

            var command = new CreateTenant
            {
                Description = description,
                Name = "Irrelevant to this test"
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreateTenantValidator.ERR_DESCRIPTION_LENGTH), Is.True);
        }

        [Test]
        public async Task CanFail_LongName()
        {
            var service = GetTenantService(false);
            var validator = new CreateTenantValidator(service);

            var name = "";
            for (var i = 0; i <= 101; i++)
                name += "I'm a long string, too long but not comparatively atleast...";

            var command = new CreateTenant
            {
                Description = "Irrelevant to this test",
                Name = name
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreateTenantValidator.ERR_TENANTNAME_LENGTH), Is.True);
        }

        [Test]
        public async Task CanFail_MissingName()
        {
            var service = GetTenantService(false);
            var validator = new CreateTenantValidator(service);

            var command = new CreateTenant
            {
                Description = "Valid description",
                Name = ""
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == "'Name' must not be empty."), Is.True);
        }

        [Test]
        public async Task CanFail_DuplicateName()
        {
            // This (tenant service) will say the tenant already exists which should
            // cause our validator to fail
            var service = GetTenantService(true);
            var validator = new CreateTenantValidator(service);

            var command = new CreateTenant
            {
                Name = "Already Exists"
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == CreateTenantValidator.ERR_TENANTNAME_UNIQUE), Is.True);
        }
    }
}
