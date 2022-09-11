using System;

namespace PZ_1_RandomNumbers
{
    class Program
    {
        public static void Main(string[] args)
        {
            NewRndNumbers(1234);
            NewRndNumbers(5678);
            NewRndNumbers(12345);
        }
        static void NewRndNumbers(int seed)
        {
            var rnd = new Random(seed);
            for (int i = 5; i < 10; i++)
            {
                var nrnd = rnd.Next(0, 10);
                Console.WriteLine(nrnd);
            }
            Console.WriteLine("End of sequence.");
        }
    }
}
