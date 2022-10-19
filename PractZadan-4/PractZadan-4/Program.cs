using System;
using System.Text;
using System.Security.Cryptography;

namespace PractZadan_4
{
    class Program
    {
        static void Main()
        {
            const string password = "V3ryC0mpl3xP455w0rd";
            byte[] salt = Hash.GenerateSalt(32);
            Console.WriteLine("Password : " + password);
            Console.WriteLine("Salt = " + Convert.ToBase64String(salt));
            Console.WriteLine();
            var Sha256text = Hash.Sha256Hash(Encoding.UTF8.GetBytes(password));
            Console.WriteLine("Hashed Password = " + Convert.ToBase64String(Sha256text));
            var hashedPassword1 = Hash.HashPasswordWithSalt(Encoding.UTF8.GetBytes(password),salt);
            Console.WriteLine("Hashed Password With Salt = " + Convert.ToBase64String(hashedPassword1));
            Console.ReadLine();
        }
    }

    public class Hash
    {
        public static byte[] GenerateSalt(int length)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[length];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }
        private static byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        public static byte[] Sha256Hash(byte[] toBeHashed)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(toBeHashed);
            }
        }
        public static byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Combine(toBeHashed, salt));
            }
        }
    }
}
