using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using StuartsBookstores.Services;

namespace StuartsBookstores.Services
{
    public class CryptographyService
    {
        public CryptographyService()
        {

        }

        private byte[] GenerateSalt()
        {
            int length = 16;
            var salt = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public Tuple<byte[], byte[]> HashPassword(string password)
        {

            byte[] salt = GenerateSalt();
            return Tuple.Create(ComputeSHA256(password, salt), salt);
        }

        private byte[] ComputeSHA256(string password, byte[] salt)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            string pepper = configuration["Pepper"];

            byte[] pepperBytes = Encoding.UTF8.GetBytes(pepper);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length + pepperBytes.Length];

            Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);
            Buffer.BlockCopy(pepperBytes, 0, saltedPassword, passwordBytes.Length + salt.Length, pepperBytes.Length);

            using (var sha256 = SHA256.Create())
            {
                byte[] hashedPassword = sha256.ComputeHash(saltedPassword);
                return hashedPassword;
            }

        }

        public bool CheckHashValid(string password, byte[] salt, byte[] TrueHashed)
        {
            byte[] candidateHash = ComputeSHA256(password, salt);

            if (candidateHash.Length != TrueHashed.Length) return false;

            for (int i = 0; i < candidateHash.Length; i++)
            {
                if (candidateHash[i] != candidateHash[i]) return false;
            }

            return true;
        }

    }
}
