using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace PractZadan_2
{
    class Program-2
    {
        public static void Main(string[] args)
        {
            string path = @"C:\Users\Asus\source\repos\AlexanderFurkalo\FundOfInformSecurity_MIT2022\PractZadan-2\mit2022.txt";
            byte[] origData = File.ReadAllBytes(path);
            File.WriteAllBytes(@"mit2022.dat", origData);
            string text1 = Encoding.UTF8.GetString(origData);
            Console.WriteLine(text1);
            Console.WriteLine("End of original file");
            Console.WriteLine("  ");

            Console.WriteLine("Write password");
            string keyword = Console.ReadLine();
            Console.Write(keyword);
            byte[] secret = Encoding.UTF8.GetBytes(keyword);
            /*Console.WriteLine(Environment.NewLine + "Our int is: " + origData.Length % secret.Length + Environment.NewLine);
            byte key = Convert.ToByte(origData.Length % secret.Length);
            Console.WriteLine(" ");
            StreamWriter encfile = new StreamWriter(@"mit2022Encrypted.dat");
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
            Console.WriteLine("End of encrypted text" + Environment.NewLine);
            encfile.Close(); */

            int i = 0;
            byte[] encrData = new byte[origData.Length];
            while (i < origData.Length)
            {
                encrData[i] = (byte)(origData[i] ^ secret[i % secret.Length]);
                i++;
            }
            string s = Encoding.UTF8.GetString(encrData);
            Console.WriteLine(s);
            File.WriteAllText(@"mit2022Encrypted.dat", s);
            Console.WriteLine("End of encrypted file" + Environment.NewLine);

            byte[] EncData = File.ReadAllBytes(@"mit2022Encrypted.dat");
            int x = 0;
            byte[] decrData = new byte[EncData.Length];
            while (x < EncData.Length)
            {
                decrData[x] = (byte)(EncData[x] ^ secret[x % secret.Length]);
                x++;
            }
            string st = Encoding.UTF8.GetString(decrData);
            Console.WriteLine(st);
            File.WriteAllText(@"mit2022Decrypted.dat", st); 
            Console.WriteLine("End of decrypted file" + Environment.NewLine);

            /*StreamWriter decfile = new StreamWriter(@"mit2022Decrypted.dat");
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
            Console.WriteLine("End of decrypted file" + Environment.NewLine);
            decfile.Close();*/
        }
    }
}
