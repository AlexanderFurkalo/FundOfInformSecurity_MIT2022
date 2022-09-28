using System;
using System.IO;
using System.Text;
using System.Linq;

namespace PractZadan_2_3
{
    class Program_2_3
    {
        static void Main(string[] args)
        {
            var timeStarted = DateTime.Now;
            Console.WriteLine(timeStarted.ToString());
            string path = @"C:\Users\Asus\source\repos\AlexanderFurkalo\FundOfInformSecurity_MIT2022\PractZadan-2\encfile5.dat";
            byte[] EncData = File.ReadAllBytes(path).ToArray();

            //int count = 0;
            //while (count < 1)
            //{
            //    Console.WriteLine(count);
                BruteForce(5, EncData);
            //    count++;
            //    Console.WriteLine(count);
            //}
            Console.WriteLine("Time passed: {0}s", DateTime.Now.Subtract(timeStarted).TotalSeconds);
        }

        private static char[] OurSigns =
        {
               'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
               'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
               'u', 'v', 'w', 'x', 'y', 'z','A','B','C','D','E',
               'F','G','H','I','J','K','L','M','N','O','P','Q','R',
               'S','T','U','V','W','X','Y','Z', '0', '1','2','3','4','5',
                '6','7','8','9','!','$','#','@','-','`','~','%','^','&','*',
                '(',')','_','+','=','{','}','[',']','|','/','"', '\'', '\\',
                ':',';','\"','<','>',',','.','?'
        };

        private static void BruteForce(int keyLength, byte[] EncData)
        {
            var keyChars = ArrayCreation(keyLength, OurSigns[0]);
            var indexOfLastChar = keyLength - 1;
            NewKey(0, keyChars, keyLength, indexOfLastChar, EncData);
        }
        private static char[] ArrayCreation(int length, char defaultChar)
        {
            return (from c in new char[length] select defaultChar).ToArray();
        }

        private static byte[] EncDecArray(byte[] EncData, string password)
        {
            byte[] key = Encoding.UTF8.GetBytes(password);
            int i = 0;
            byte[] OutData = new byte[EncData.Length];
            while (i < EncData.Length)
            {
                OutData[i] = (byte)(EncData[i] ^ key[i % key.Length]);
                i++;
            }
            string s = Encoding.UTF8.GetString(OutData);
            if (s.Contains("Mit21"))
            {
                Console.WriteLine("Our case: " + s + Environment.NewLine + "Our password: " + password + Environment.NewLine);
            }
            return OutData;
        }

        private static void NewKey(int CurrentCharPosition, char[] keyChars, int keyLength, int IndexOfLastChar, byte[] EncData)
        {
            int ToTest = OurSigns.Length;
            var NextCharPosition = CurrentCharPosition + 1;
            for (int i = 0; i < ToTest; i++)
            {
                keyChars[CurrentCharPosition] = OurSigns[i];
                if (CurrentCharPosition < IndexOfLastChar)
                {
                    NewKey(NextCharPosition, keyChars, keyLength, IndexOfLastChar, EncData);
                }
                else

                new string(keyChars);
                EncDecArray(EncData, new string(keyChars));

                if (new string(keyChars) == "?????")
                {
                    return;
                }
            }
        }
    }
}
