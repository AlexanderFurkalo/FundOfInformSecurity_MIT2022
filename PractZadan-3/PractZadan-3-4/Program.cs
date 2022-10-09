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
                if (response == "R" || response == "r" )
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
            Console.WriteLine("Write your password: ");
            string YourPass = Console.ReadLine();
            byte[] CoolerPass = Encoding.UTF8.GetBytes(YourPass);

            var hmacSha512Code = ComputeHmacsha512(Encoding.UTF8.GetBytes(Name), CoolerPass);
            string code = Convert.ToBase64String(hmacSha512Code);
            File.AppendAllText(@"Users.dat", code + Environment.NewLine);

            Console.WriteLine("Registration process completed successfully." + Environment.NewLine);
        }

        public static void Login()
        {
            Console.WriteLine("Welcome Back! Write your username: ");
            string Name = Console.ReadLine();
            Console.WriteLine("Now write your password: ");
            string YourPass = Console.ReadLine();
            byte[] CoolerPass = Encoding.UTF8.GetBytes(YourPass);

            var hmacSha512Code = ComputeHmacsha512(Encoding.UTF8.GetBytes(Name), CoolerPass);
            string code = Convert.ToBase64String(hmacSha512Code);

            string check = File.ReadAllText(@"Users.dat");
            if (check.Contains(code))
            {
                Console.WriteLine("Welcome back," + Name + "!" + Environment.NewLine);
            }
            else
            {
                Console.WriteLine("Wrong name or password. Try again!" + Environment.NewLine);
            }

        }

        public static byte[] ComputeHmacsha512(byte[] toBeHashed, byte[] key)
        {
            using (var hmac = new HMACSHA512(key))
            {
                return hmac.ComputeHash(toBeHashed);
            }
        }
    }
}
