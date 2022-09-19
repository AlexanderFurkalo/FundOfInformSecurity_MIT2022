﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace PractZadan_2
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\Asus\source\repos\AlexanderFurkalo\FundOfInformSecurity_MIT2022\PractZadan-2\mit2022.txt";
            byte[] origData = File.ReadAllBytes(path);
            File.WriteAllBytes(@"mit2022.dat", origData);
            string text1 = Encoding.UTF8.GetString(origData);
            Console.WriteLine(text1);
            Console.WriteLine("End of original file");

            StreamWriter encfile = new StreamWriter(@"mit2022Encrypted.dat");
            byte key = 2;
            List<byte> list1 = new List<byte>();
            foreach (byte i in origData)
            {
                byte message = i;
                byte encryptedMessage = (byte)(message ^ key);
                list1.Add(encryptedMessage);

            }
            byte[] array1 = list1.ToArray();
            string TextEncr = Encoding.UTF8.GetString(array1);
            Console.WriteLine(TextEncr);
            encfile.Write(TextEncr);
            Console.WriteLine("End of encrypted text");
            encfile.Close();

            StreamWriter decfile = new StreamWriter(@"mit2022Decrypted.dat");
            byte[] EncData = File.ReadAllBytes(@"mit2022Encrypted.dat");
            List<byte> list2 = new List<byte>();
            foreach (byte i in EncData)
            {
                byte encryptedMessage = i;
                byte decryptedMessage = (byte)(encryptedMessage ^ key);
                list2.Add(decryptedMessage);
            }
            byte[] array2 = list2.ToArray();
            string TextDecr = Encoding.UTF8.GetString(array2);
            Console.WriteLine(TextDecr);
            decfile.Write(TextDecr);
            Console.WriteLine("End of decrypted file");
            decfile.Close();
        }
    }
}