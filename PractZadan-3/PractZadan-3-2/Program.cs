using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Security.Cryptography;

namespace PractZadan_2_3
{
    class Program_2_3
    {
        static void Main(string[] args)
        {
            var timeStarted = DateTime.Now;
            Console.WriteLine(timeStarted.ToString());

            //int count = 0;
            //while (count < 1)
            //{
            //    Console.WriteLine(count);
            BruteForce(8);
            //    count++;
            //    Console.WriteLine(count);
            //}
            Console.WriteLine("Time passed: {0}s", DateTime.Now.Subtract(timeStarted).TotalSeconds);
        }

        private static char[] OurSigns =
        {
               '0', '1','2','3','4','5',
                '6','7','8','9'
        };

        private static void BruteForce(int keyLength)
        {
            var keyChars = ArrayCreation(keyLength, OurSigns[0]);
            var indexOfLastChar = keyLength - 1;
            NewKey(0, keyChars, keyLength, indexOfLastChar);
        }
        private static char[] ArrayCreation(int length, char defaultChar)
        {
            return (from c in new char[length] select defaultChar).ToArray();
        }

        private static void MD5Check(string password)
        {
            byte[] Data = Encoding.Unicode.GetBytes(password);
            var md5ForText = MD5Hash(Data);
            string s = Convert.ToBase64String(md5ForText);
            if (s.Contains("{564c8da6-0440-88ec-d453-0bbad57c6036}"))
            {
                Console.WriteLine("Our case: " + s + Environment.NewLine + "And our password: " + password + Environment.NewLine);
            }
            else if (s.Contains("po1MVkAE7IjUUwu61XxgNg=="))
            {
                Console.WriteLine("Our case: " + s + Environment.NewLine + "And our password: " + password + Environment.NewLine);
            }
        }
        static byte[] MD5Hash(byte[] data)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(data);
            }
        }

        private static void NewKey(int CurrentCharPosition, char[] keyChars, int keyLength, int IndexOfLastChar)
        {
            int ToTest = OurSigns.Length;
            var NextCharPosition = CurrentCharPosition + 1;
            for (int i = 0; i < ToTest; i++)
            {
                keyChars[CurrentCharPosition] = OurSigns[i];
                if (CurrentCharPosition < IndexOfLastChar)
                {
                    NewKey(NextCharPosition, keyChars, keyLength, IndexOfLastChar);
                }
                else

                    new string(keyChars);
                MD5Check(new string(keyChars));

                if (new string(keyChars) == "99999999")
                {
                    return;
                }
            }
        }
    }
}
