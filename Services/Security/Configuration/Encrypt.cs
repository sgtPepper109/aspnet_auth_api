using System.Security.Cryptography;

namespace ASPNETAuthAPI.Services.Configuration
{
    public class Encrypt
    {
        private static RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        private static readonly int SaltSize = 16;
        private static readonly int HashSize = 20;
        private static readonly int Iterations = 10000;

        public static string Hash(string text) {
            byte[] salt;
            rngCryptoServiceProvider.GetBytes(salt = new byte[SaltSize]);
            var key = new Rfc2898DeriveBytes(text, salt, Iterations);
            var hash = key.GetBytes(HashSize);
            var hashed_bytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashed_bytes, 0, SaltSize);
            Array.Copy(hash, 0, hashed_bytes, SaltSize, HashSize);
            return Convert.ToBase64String(hashed_bytes);
        }

        public static bool VerifyEncryption(string text, string base64_encoding) {
            var hashed_bytes = Convert.FromBase64String(base64_encoding);
            var salt = new byte[SaltSize];
            Array.Copy(hashed_bytes, 0, salt, 0, SaltSize);
            var key = new Rfc2898DeriveBytes(text, salt, Iterations);
            byte[] hash = key.GetBytes(HashSize);
            for (int i = 0; i < HashSize; i++)
                if (hashed_bytes[i + SaltSize] != hash[i]) return false;
            return true;
        }
    }
}
