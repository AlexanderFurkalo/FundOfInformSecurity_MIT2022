using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace PractZadan_3_4
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Hello. If you want to register a user, type R. If you want to login, type L. If you want to quit, type Q.");
                string response = Console.ReadLine();
                if (response == "R" || response == "r")
                {
                    Console.WriteLine("Registration process will start...now");
                    Register();
                }
                if (response == "L" || response == "l")
                {
                    Console.WriteLine("You already have an account? Good!");
                    Login();
                }
                if (response == "Q" || response == "q")
                {
                    Console.WriteLine("Goodbye!");
                    return;
                }
                if (response != "R" && response != "r" && response != "L" && response != "l" && response != "Q" && response != "q")
                {
                    Console.WriteLine("To interact with this programm, you can only enter R, L or Q. Try again");
                }
            }
        }
        public static void Register()
        {
            Console.WriteLine("Write your username: ");
            string Name = Console.ReadLine();
            byte[] dataName = Encoding.Unicode.GetBytes(Name);
            var ShaForText = Sha1Hash(dataName);
            string hashName = Convert.ToBase64String(ShaForText);
            Console.WriteLine($"Hash Sha1: {hashName}");
            File.AppendAllText(@"UsersName.dat", hashName + Environment.NewLine);

            Console.WriteLine("Write your password: ");
            string YourPass = Console.ReadLine();
            int iter = 230000;
            byte[] Salt = GenerateSalt();
            string saltdata = Convert.ToBase64String(Salt);
            File.AppendAllText(@"Salt.dat", saltdata + Environment.NewLine);

            var hashedPassword = HashPassword(Encoding.UTF8.GetBytes(YourPass), Salt, iter);
            string hashPass = Convert.ToBase64String(hashedPassword);
            Console.WriteLine("Hashed Password : " + hashPass);
            File.AppendAllText(@"Pass.dat", hashPass + Environment.NewLine);

            Console.WriteLine("Registration process completed successfully." + Environment.NewLine);
        }

        public static void Login()
        {
            Console.WriteLine("Welcome Back! Write your username: ");
            string Name = Console.ReadLine();
            Console.WriteLine("Now write your password: ");
            string YourPass = Console.ReadLine();

            byte[] dataName = Encoding.Unicode.GetBytes(Name);
            var ShaForText = Sha1Hash(dataName);
            string hashName = Convert.ToBase64String(ShaForText);

            int iter = 230000;

            string check1 = File.ReadAllText(@"UsersName.dat");
            if (check1.Contains(hashName))
            {
                string[] SaltColl = File.ReadAllLines(@"Salt.dat");
                foreach (string i in SaltColl)
                {
                    byte[] salty = Convert.FromBase64String(i);
                    var hashedPassword = HashPassword(Encoding.UTF8.GetBytes(YourPass), salty, iter);
                    string hashPass = Convert.ToBase64String(hashedPassword);
                    string check2 = File.ReadAllText(@"Pass.dat");
                    if (check2.Contains(hashPass))
                    {
                        Console.WriteLine("Welcome back," + Name + "!" + Environment.NewLine);
                        return;
                    }
                }
                Console.WriteLine("Wrong name or password. Try again!" + Environment.NewLine);
            }
            else
            {
                Console.WriteLine("Wrong name or password. Try again!" + Environment.NewLine);
            }

        }
        static byte[] Sha1Hash(byte[] data)
        {
            using (var sha = SHA1.Create())
            {
                return sha.ComputeHash(data);
            }
        }

        public static byte[] GenerateSalt()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[32];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }

        public static byte[] HashPassword(byte[] toBeHashed, byte[] salt, int iter)
        {
            var HashAl = new HashAlgorithmName("SHA1");
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, iter, HashAl))
            {
                return rfc2898.GetBytes(20);
            }
        }
    }
}
