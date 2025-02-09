using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Digivance.Auth.Data.EntityFramework
{
    /// <summary>
    /// A simple password helper for hashing passwords
    /// </summary>
    public class PasswordHelper
    {
        /// <summary>
        /// Number of hashing iterations
        /// </summary>
        public const int ITERATION_COUNT = 100000;

        /// <summary>
        /// Number of bytes per segment (e.g. double this is what we store)
        /// </summary>
        public const int BYTE_SIZE = 64;

        /// <summary>
        /// Hashes the provided password, optionally using provided sale (or generates
        /// a random salt)
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <param name="salt">The salt to hash with (or generates one at randome)</param>
        /// <returns>BYTE_SIZE * 2 bytes representing this password</returns>
        public static byte[] Hash(string password, byte[]? salt = null)
        {
            if (salt == null) 
                salt = GenerateSalt();

            var passwordHash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, ITERATION_COUNT, BYTE_SIZE);
            var fullHash = new byte[BYTE_SIZE*2];

            Array.Copy(passwordHash, fullHash, BYTE_SIZE);
            Array.Copy(salt, 0, fullHash, BYTE_SIZE, BYTE_SIZE);

            return fullHash;
        }

        /// <summary>
        /// Hashes and compares the provided password to the hashedPassword value.  If true
        /// this is the correct password.
        /// </summary>
        /// <param name="password">The clear text password to compare</param>
        /// <param name="hashedPassword">The hashed password to compare</param>
        /// <returns>True if this is the correct password</returns>
        public static bool Compare(string password, byte[] hashedPassword)
        {
            var passwordHash = new byte[BYTE_SIZE];
            var salt = new byte[BYTE_SIZE];

            Array.Copy(hashedPassword, 0, passwordHash, 0, BYTE_SIZE);
            Array.Copy(hashedPassword, BYTE_SIZE, salt, 0, BYTE_SIZE);

            var confirmHash = Hash(password, salt);
            return confirmHash.SequenceEqual(hashedPassword);
        }

        /// <summary>
        /// Used internally to generate a BYTE_SIZE salt
        /// </summary>
        /// <returns>BYTE_SIZE byte array to use for flavor (salting)</returns>
        private static byte[] GenerateSalt()
        {
            var salt = new byte[BYTE_SIZE];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);
            return salt;
        }
    }
}
