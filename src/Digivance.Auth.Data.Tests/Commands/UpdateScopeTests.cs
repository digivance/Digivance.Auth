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
    public class UpdateScopeTests
    {
        /// <summary>
        /// Helper method to build a mock IScopeService. Will return returnsTrue when you call
        /// ExistsByNameAsync
        /// </summary>
        /// <param name="returnsTrue">The value to return for ExistsBeNameAsync</param>
        /// <returns>A mock implementation of IScopeService</returns>
        private IScopeService GetScopeService(bool returnsTrue, bool returnsTrue1)
        {
            var mock = new Mock<IScopeService>();

            mock
                .Setup(x => x.ExistsAsync(It.IsAny<Guid?>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnsTrue));

            mock
               .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
               .Returns(Task.FromResult(returnsTrue1));

            return mock.Object;
        }

        [Test]
        public async Task Can_BeValid()
        {
            var service = GetScopeService(false, true);
            var validator = new UpdateScopeValidator(service);

            var command = new UpdateScope
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
            var service = GetScopeService(false, true);
            var validator = new UpdateScopeValidator(service);

            var description = "";
            for (var i = 0; i <= 4001; i++)
                description += "I'm a long string, too long one might say...";

            var command = new UpdateScope
            {
                Description = description,
                Name = "Irrelevant to this test",
                Id = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdateScopeValidator.ERR_DESCRIPTION_TOO_LONG), Is.True);
        }

        [Test]
        public async Task CanFail_LongName()
        {
            var service = GetScopeService(false, true);
            var validator = new UpdateScopeValidator(service);

            var name = "";
            for (var i = 0; i <= 101; i++)
                name += "I'm a long string, too long but not comparatively atleast...";

            var command = new UpdateScope
            {
                Description = "Irrelevant to this test",
                Name = name,
                Id = Guid.NewGuid() 
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdateScopeValidator.ERR_SCOPENAME_LENGTH), Is.True);
        }
        [Test]
        public async Task CanFail_MissingName()
        {
            var service = GetScopeService(false, true);
            var validator = new UpdateScopeValidator(service);

            var command = new UpdateScope
            {
                Name = "",
                Id = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdateScopeValidator.ERR_SCOPE_NAME_EMPTY), Is.True);
        }

        [Test]
        public async Task CanFail_DuplicateName()
        {
            var service = GetScopeService(true, true);
            var validator = new UpdateScopeValidator(service);

            var command = new UpdateScope
            {
                Name = "AlreadyExists",
                Id = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdateScopeValidator.ERR_SCOPE_NAME_EXISTS), Is.True);
        }


        [Test]
        public async Task CanFail_IDNotExists()
        {
            var service = GetScopeService(false, false);
            var validator = new UpdateScopeValidator(service);

            var command = new UpdateScope
            {
                Name = "Already Exists",
                Id = Guid.NewGuid()
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdateScopeValidator.ERR_SCOPEID_NOTFOUND), Is.True);
        }
    }
}
