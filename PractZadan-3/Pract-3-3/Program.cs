using System;
using System.Text;
using System.Security.Cryptography;

namespace Pract_3_3
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Write something: ");
            string Message = Console.ReadLine();
            Console.WriteLine(Message);
            var key = HMAC.GenerateKey(32);
            string ourkey = Convert.ToBase64String(key);
            Console.WriteLine(ourkey);
            var hmacSha512Message = HMAC.ComputeHmacsha512(Encoding.UTF8.GetBytes(Message), key);

            Console.WriteLine();
            Console.WriteLine("SHA 512 HMAC");
            Console.WriteLine("hash = " + Convert.ToBase64String(hmacSha512Message));

            Console.WriteLine();
            Console.WriteLine("Write the received message: ");
            string RecMes = Console.ReadLine();
            Console.WriteLine(RecMes);

            Console.WriteLine();
            Console.WriteLine("Write your password: ");
            string YourPass = Console.ReadLine();
            Console.WriteLine(YourPass);
            byte[] CoolerPass = Convert.FromBase64String(YourPass);

            Console.WriteLine();
            var hmacSha512Check1 = HMAC.ComputeHmacsha512(Encoding.UTF8.GetBytes(Message), key);
            var hmacSha512Check2 = HMAC.ComputeHmacsha512(Encoding.UTF8.GetBytes(RecMes), CoolerPass);

            Console.WriteLine();
            Console.WriteLine("SHA 512 HMAC");
            string codeOne = Convert.ToBase64String(hmacSha512Check1);
            string codeTwo = Convert.ToBase64String(hmacSha512Check2);
            Console.WriteLine("hash = " + codeOne);
            Console.WriteLine("hash = " + codeTwo);
            if (codeOne == codeTwo)
            {
                Console.WriteLine("Same message");
            }
            else
            {
                Console.WriteLine("These messages are not the same, something is wrong");
            }

        }
    }
    public class HMAC
        {
        public static byte[] GenerateKey(int length)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[length];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
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
