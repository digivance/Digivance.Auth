namespace Digivance.Auth.Data.EntityFramework.Tests
{
    [TestFixture]
    public class PasswordHelperTests
    {
        [TestCase("MyPassingWordz")]
        public void Can_HashAndConfirm(string password)
        {
            var hashedPassword = PasswordHelper.Hash(password);
            var canConfirm = PasswordHelper.Compare(password, hashedPassword);
            var canDeny = PasswordHelper.Compare("nopenopenizope", hashedPassword);

            Assert.That(canConfirm, Is.True);
            Assert.That(canDeny, Is.False);
        }
    }
}
