using System.Security.Cryptography;
using System;

namespace PZ_1_RandomNumbers_2
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                string rndNumber = Convert.ToBase64String(Random.GeneratorRndNum(32));
                Console.WriteLine(rndNumber);
            }
            Console.ReadLine();
        }
    }
    public class Random
    {
        public static byte[] GeneratorRndNum(int length)
        {
            using (var rndNumGen = new RNGCryptoServiceProvider())
            {
                var rndNumber = new byte[length];
                rndNumGen.GetBytes(rndNumber);
                int Numb = BitConverter.ToInt32(rndNumber, 0);
                Console.WriteLine(Numb);
                return rndNumber;
            }
        }
    }
}
