using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BL.Security
{
    public static class PasswordHashProvider
    {
        public static string GetSalt()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); 
            return Convert.ToBase64String(salt);
        }

        public static string GetHash(string password, string b64Salt)
        {
            byte[] salt = Convert.FromBase64String(b64Salt);

            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000, 
                numBytesRequested: 256 / 8); 

            return Convert.ToBase64String(hash);
        }
    }
}
