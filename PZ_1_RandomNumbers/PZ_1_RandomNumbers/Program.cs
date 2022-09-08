using System;

namespace PZ_1_RandomNumbers
{
    class Program
    {
        static void Main()
        {
            var rnd = new Random(1234);
            for (int i = 5; i < 10; i++)
            {
                var nrnd = rnd.Next(0, 10);
                Console.WriteLine(nrnd);
            }
        }
    }
}
