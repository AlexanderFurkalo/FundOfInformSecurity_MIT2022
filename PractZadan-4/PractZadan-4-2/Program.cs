using System;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

namespace PractZadan_4_2
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Choose: MD5, SHA1, SHA256, SHA384 or SHA512");
            string choice = Console.ReadLine();
            const string passwordToHash = "VeryComplexPassword";
            int count = 0;
            int iter = 230000;
            while (count!=10)
            { 
               HashPassword(passwordToHash, iter, choice);
               iter += 50000;
               count += 1;
            }
            Console.ReadLine();
        }
        private static void HashPassword(string passwordToHash, int numberOfRounds, string choice)
        {
            var sw = new Stopwatch();
            sw.Start();
            var hashedPassword = PBKDF2.HashPassword(Encoding.UTF8.GetBytes(passwordToHash),PBKDF2.GenerateSalt(),numberOfRounds, choice);
            sw.Stop();
            Console.WriteLine();
            Console.WriteLine("Password to hash : " + passwordToHash);
            Console.WriteLine("Hashed Password : " +  Convert.ToBase64String(hashedPassword));
            Console.WriteLine("Iterations <" + numberOfRounds + ">  Elapsed Time: " +  sw.ElapsedMilliseconds + "ms");
        }
    }
    public class PBKDF2
    {
        public static byte[] GenerateSalt()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[32];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }
        public static byte[] HashPassword(byte[] toBeHashed, byte[] salt, int numberOfRounds, string choice)
        {
            if (choice == "MD5")
            {
                var HashAl = new HashAlgorithmName("MD5");
                using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds, HashAl))
                {
                    return rfc2898.GetBytes(20);
                }
            }
            if (choice == "SHA1")
            {
                var HashAl = new HashAlgorithmName("SHA1");
                using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds, HashAl))
                {
                    return rfc2898.GetBytes(20);
                }
            }
            if (choice == "SHA256")
            {
                var HashAl = new HashAlgorithmName("SHA256");
                using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds, HashAl))
                {
                    return rfc2898.GetBytes(20);
                }
            }
            if (choice == "SHA384")
            {
                var HashAl = new HashAlgorithmName("SHA384");
                using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds, HashAl))
                {
                    return rfc2898.GetBytes(20);
                }
            }
            if (choice == "SHA512")
            {
                var HashAl = new HashAlgorithmName("SHA512");
                using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds, HashAl))
                {
                    return rfc2898.GetBytes(20);
                }
            }
            else 
            {
                var HashAl = new HashAlgorithmName("SHA1");
                using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds, HashAl))
                {
                     return rfc2898.GetBytes(20);
                }
            }
        }
    }
}
