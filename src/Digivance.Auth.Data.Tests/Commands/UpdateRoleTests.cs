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
    public class UpdateRoleTests
    {
        /// <summary>
        /// Helper method to build a mock IRoleService. Will return returnsTrue when you call
        /// ExistsByNameAsync
        /// </summary>
        /// <param name="returnsTrue">The value to return for ExistsBeNameAsync</param>
        /// <returns>A mock implementation of IRoleService</returns>
        private IRoleService GetRoleService(bool returnsTrue, bool returnsTrue1)
        {
            var mock = new Mock<IRoleService>();

            mock
               .Setup(x => x.GetScopeId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(Guid.NewGuid()); 

            mock
               .Setup(x => x.ExistsByNameAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
               .Returns(Task.FromResult(returnsTrue));

            mock
               .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
               .Returns(Task.FromResult(returnsTrue1));

            return mock.Object;
        }

        [Test]
        public async Task Can_BeValid()
        {
            var roleService = GetRoleService(false, true);
            var validator = new UpdateRoleValidator(roleService);

            var command = new UpdateRole
            {
                Description = "Valid description",
                Name = "Irrelevant to this test",
                Id = Guid.NewGuid()
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public async Task CanFail_LongDescription()
        {
            var roleService = GetRoleService(false, true);
            var validator = new UpdateRoleValidator(roleService);

            var description = "";
            for (var i = 0; i <= 4001; i++)
                description += "I'm a long string, too long one might say...";

            var command = new UpdateRole
            {
                Description = description,
                Name = "Irrelevant to this test",
                Id = Guid.NewGuid() // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdateRoleValidator.ERR_DESCRIPTION_TOO_LONG), Is.True);
        }

        [Test]
        public async Task CanFail_MissingName()
        {
            var roleService = GetRoleService(false, true);
            var validator = new UpdateRoleValidator(roleService);

            var command = new UpdateRole
            {
                Name = "",
                Id = Guid.NewGuid()
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == "'Name' must not be empty."), Is.True);
        }

        [Test]
        public async Task CanFail_LongName()
        {
            var roleService = GetRoleService(false, true);
            var validator = new UpdateRoleValidator(roleService);

            var name = "";
            for (var i = 0; i <= 101; i++)
                name += "I'm a long string, too long but not comparatively atleast...";

            var command = new UpdateRole
            {
                Description = "Irrelevant to this test",
                Name = name,
                Id = Guid.NewGuid()
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdateRoleValidator.ERR_ROLENAME_LENGTH), Is.True);
        }

        [Test]
        public async Task CanFail_DuplicateName()
        {
            var roleService = GetRoleService(true, true);
            var validator = new UpdateRoleValidator(roleService);

            var command = new UpdateRole
            {
                Name = "AlreadyExists",
                Id = Guid.NewGuid(), // doesn't matter mocked service will pass it
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdateRoleValidator.ERR_ROLE_NAME_EXISTS), Is.True);
        }

        [Test]
        public async Task CanFail_IDNotExists()
        {
            var roleService = GetRoleService(false, false);
            var validator = new UpdateRoleValidator(roleService);

            var command = new UpdateRole
            {
                Name = "Already Exists",
                Id = Guid.NewGuid()
            };

            var res = await validator.ValidateAsync(command, default);

            Assert.That(res.IsValid, Is.False);
            Assert.That(res.Errors.All(x => x.ErrorMessage == UpdateRoleValidator.ERR_ROLEID_NOTFOUND), Is.True);
        }
    }
}
