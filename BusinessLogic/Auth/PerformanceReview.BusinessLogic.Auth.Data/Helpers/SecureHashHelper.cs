using System;
using System.Linq;
using System.Security.Cryptography;

namespace PerformanceReview.BusinessLogic.Auth.Data.Helpers
{
    public class SecureHashHelper
    {
        public (string encodedKey, string encodedSalt) HashPassword(string password)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, 256))
            {
                byte[] saltB = deriveBytes.Salt;
                byte[] key = deriveBytes.GetBytes(256); // 256-byte key

                string encodedKey = Convert.ToBase64String(key);
                string encodedSalt = Convert.ToBase64String(saltB);

                return (encodedKey, encodedSalt);
            }
        }

        public bool IsPasswordMatch(string encodedKey, string encodedSalt, string password)
        {
            byte[] key = Convert.FromBase64String(encodedKey);
            byte[] salt = Convert.FromBase64String(encodedSalt);

            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt))
            {
                byte[] testKey = deriveBytes.GetBytes(256); // 256-byte key
                if (!testKey.SequenceEqual(key))
                    return false;

                return true;
            }
        }
    }
}
