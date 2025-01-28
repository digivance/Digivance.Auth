using Digivance.Auth.Data.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digivance.Auth.Data.Tests.Commands
{
    [TestFixture]
    public class CreateUserValidationTests
    {
        public CreateUserValidator _validator;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _validator = new CreateUserValidator();
        }

        [TestCase("invalid-email", false)]
        [TestCase("valid@email.com", true)]
        public void CanValidateEmail(string address, bool expectValid)
        {
            var newUser = new CreateUser
            {
                DisplayName = "TestUser",
                EmailAddress = address,
                Password = "abcdefgH1!",
                TenantId = Guid.NewGuid(),
                Username = "TestUser"
            };
            
            if(expectValid)
            {
                Assert.DoesNotThrow(() => _validator.ValidateAndThrow(newUser));   
            }
            else
            {
                Assert.Throws<FluentValidation.ValidationException>(() => _validator.ValidateAndThrow(newUser));
            }
        }

        [TestCase("Short1!", false)]
        [TestCase("12345678", false)]
        [TestCase("abcdefgh", false)]
        [TestCase("nosymb1s", false)]
        [TestCase("abC123!@", true)]
        public void CanValidatePassword(string password, bool expectValid)
        {
            var newUser = new CreateUser
            {
                DisplayName = "TestUser",
                EmailAddress = "test123@gmail",
                Password = password,
                TenantId = Guid.NewGuid(),
                Username = "TestUser"
            };

            if(expectValid)
            {
                Assert.DoesNotThrow(() => _validator.ValidateAndThrow(newUser));   
            }
            else
            {
                Assert.Throws<FluentValidation.ValidationException>(() => _validator.ValidateAndThrow(newUser));
            }
        }
    }
}
