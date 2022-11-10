using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace PractZadan_6_2
{
    class Program
    {
        static void Main(string[] args)
        {
            string ppath = @"publ.txt";
            string privpath = @"priv.txt";
            var rsaParams = new RSAWithRSAParameterKey();
            const string original = "Text to encrypt";
            rsaParams.AssignNewKey(ppath, privpath);
            Console.WriteLine("You can encrypt a message with another user's key. Will you do it? Type Y, if yes; Type N, if no.");
            string response = Console.ReadLine();
            if (response=="Y")
                {
                Console.WriteLine("Write a file name");
                string another = (Console.ReadLine() + ".txt");
                var encrypted = rsaParams.EncryptData(another, Encoding.UTF8.GetBytes(original));
                Console.WriteLine("RSA Encryption Demonstration in .NET");
                Console.WriteLine("------------------------------------");
                Console.WriteLine();
                Console.WriteLine("In Memory Key");
                Console.WriteLine();
                Console.WriteLine(" Original Text = " + original);
                Console.WriteLine(" Encrypted Text = " + Convert.ToBase64String(encrypted));
            }
            else
                {
                var encrypted = rsaParams.EncryptData(ppath, Encoding.UTF8.GetBytes(original));
                var decrypted = rsaParams.DecryptData(privpath, encrypted);
                Console.WriteLine("RSA Encryption Demonstration in .NET");
                Console.WriteLine("------------------------------------");
                Console.WriteLine();
                Console.WriteLine("In Memory Key");
                Console.WriteLine();
                Console.WriteLine(" Original Text = " + original);
                Console.WriteLine(" Encrypted Text = " + Convert.ToBase64String(encrypted));
                Console.WriteLine(" Decrypted Text = " + Encoding.Default.GetString(decrypted));
            }
        }
    }

    class RSAWithRSAParameterKey
    {
        public void AssignNewKey(string publicKeyPath, string privateKeyPath)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
                File.WriteAllText(privateKeyPath, rsa.ToXmlString(true));
            }
        }

        public byte[] EncryptData(string publicKeyPath, byte[] dataToEncrypt)
        {
            byte[] cipherbytes;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(publicKeyPath));
                cipherbytes = rsa.Encrypt(dataToEncrypt, false);
            }
            return cipherbytes;
        }

        public byte[] DecryptData(string privateKeyPath, byte[] dataToEncrypt)
        {
            byte[] plain;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(privateKeyPath));
                plain = rsa.Decrypt(dataToEncrypt, false);
            }
            return plain;
        }

    }
}
