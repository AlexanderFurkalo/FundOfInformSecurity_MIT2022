using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace PractZadan_6_3
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {


                Console.WriteLine("Hello! You have the following options: " + Environment.NewLine +
                    "1. Generate new keys;" + Environment.NewLine +
                    "2. Delete your keys;" + Environment.NewLine +
                    "3. Encrypt;" + Environment.NewLine +
                    "4. Decrypt;" + Environment.NewLine +
                    "5. Quit." + Environment.NewLine +
                    "So choose a number and proceed!");
                string response = Console.ReadLine();
                if (response == "1")
                {
                    Console.WriteLine("Write a name for your pair of keys");
                    string keys = Console.ReadLine()+".xml";
                    RSAW.DeleteKeyInCsp(keys);
                    RSAW.AssignNewKeys(keys);
                    Console.WriteLine("Done." + Environment.NewLine);
                }
                if (response == "2")
                {
                    Console.WriteLine("Write the name of the keys to be deleted");
                    string delkey = Console.ReadLine() + ".xml";
                    RSAW.DeleteKeyInCsp(delkey);
                    Console.WriteLine("Done." + Environment.NewLine);
                }
                if (response == "3")
                {
                    Console.WriteLine("Write a message");
                    string message = Console.ReadLine();
                    byte[] toencrypt = Encoding.UTF8.GetBytes(message);
                    Console.WriteLine("Write a name of the key");
                    string keyname = Console.ReadLine() + ".xml";
                    Console.WriteLine("Write a name for your message");
                    string namemess = Console.ReadLine() + ".txt";
                    byte[] encrypted = RSAW.EncryptData(toencrypt, keyname, namemess);
                    Console.WriteLine("Encrypted Text = " + Convert.ToBase64String(encrypted));
                    Console.WriteLine("Done." + Environment.NewLine);
                }
                if (response == "4")
                {
                     Console.WriteLine("Write a name of the file to decrypt");
                     string name = Console.ReadLine();
                     byte[] messtodecr = File.ReadAllBytes(name + ".txt");
                     string newname = "Decr" + name + ".txt";
                     byte[] decrypted = RSAW.DecryptData(messtodecr, newname);
                     Console.WriteLine("Decrypted Text = " + Encoding.Default.GetString(decrypted));
                     Console.WriteLine("Done." + Environment.NewLine);
                }
                if (response == "5")
                {
                    Console.WriteLine("Goodbye!");
                    return;
                }
            }
        }
    }

    class RSAW
    {
        const string CspContainerName = "MyContainer";

        public static void AssignNewKeys(string publicKeyPath)
        {
            CspParameters cspParams = new CspParameters(1)
            {
                KeyContainerName = CspContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore,
                ProviderName = "Microsoft Strong Cryptographic Provider"
            };
            using (var rsa = new RSACryptoServiceProvider(2048, cspParams))
            {
                rsa.PersistKeyInCsp = true;
                File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
            };
        }

        public static void DeleteKeyInCsp(string publicKeyPath)
        {
            CspParameters cspParams = new CspParameters
            {
                KeyContainerName = CspContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            var rsa = new RSACryptoServiceProvider(cspParams)
            {
                PersistKeyInCsp = false
            };
            File.Delete(publicKeyPath);
            rsa.Clear();
        }

        public static byte[] EncryptData(byte[] dataToEncrypt, string publicKeyPath, string path)
        {
            byte[] cipherbytes;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(publicKeyPath));
                cipherbytes = rsa.Encrypt(dataToEncrypt, true);
            }
            File.WriteAllBytes(path, cipherbytes);
            return cipherbytes;
        }

        public static byte[] DecryptData(byte[] dataToDecrypt, string newname)
        {
            byte[] plain;
            var cspParams = new CspParameters
            {
                KeyContainerName = CspContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            using (var rsa = new RSACryptoServiceProvider(2048, cspParams))
            {
                rsa.PersistKeyInCsp = true;
                plain = rsa.Decrypt(dataToDecrypt, true);
                File.WriteAllText(newname, rsa.ToXmlString(true));
            }
            return plain;
        }

    }
}