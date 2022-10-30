using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;



namespace PractZadan_5_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var pan = new PassAndNumb();
            Console.WriteLine("Write your password: ");
            string YourPass = Console.ReadLine();
            byte[] password = Encoding.UTF8.GetBytes(YourPass);
            int iter = 230000;

            var des = new DesEncryption();
            var key1 = pan.HashPassword(password, iter, 8);
            var iv1 = pan.HashPassword(password, iter, 8);
            const string original1 = "Text to encrypt";
            var encrypted1 = des.Encrypt(Encoding.UTF8.GetBytes(original1), key1, iv1);
            var decrypted1 = des.Decrypt(encrypted1, key1, iv1);
            var decryptedMessage1 = Encoding.UTF8.GetString(decrypted1);
            Console.WriteLine("DES Demonstration in .NET");
            Console.WriteLine("-------------------------");
            Console.WriteLine();
            Console.WriteLine("Original Text = " + original1);
            Console.WriteLine("Encrypted Text = " + Convert.ToBase64String(encrypted1));
            Console.WriteLine("Decrypted Text = " + decryptedMessage1 + Environment.NewLine);

            var tripleDes = new TripleDesEncryption();
            var key2 = pan.HashPassword(password, iter, 16);
            var iv2 = pan.HashPassword(password, iter, 8);
            const string original2 = "Text to encrypt";
            var encrypted2 = tripleDes.Encrypt(Encoding.UTF8.GetBytes(original2), key2, iv2);
            var decrypted2 = tripleDes.Decrypt(encrypted2, key2, iv2);
            var decryptedMessage2 = Encoding.UTF8.GetString(decrypted2);
            Console.WriteLine("Triple DES Demonstration in .NET");
            Console.WriteLine("--------------------------------");
            Console.WriteLine();
            Console.WriteLine("Original Text = " + original2);
            Console.WriteLine("Encrypted Text = " + Convert.ToBase64String(encrypted2));
            Console.WriteLine("Decrypted Text = " + decryptedMessage2 + Environment.NewLine);

            var aes = new AesEncryption();
            var key3 = pan.HashPassword(password, iter, 32);
            var iv3 = pan.HashPassword(password, iter, 16);
            const string original3 = "Text to encrypt";
            var encrypted3 = aes.Encrypt(Encoding.UTF8.GetBytes(original3), key3, iv3);
            var decrypted3 = aes.Decrypt(encrypted3, key3, iv3);
            var decryptedMessage3 = Encoding.UTF8.GetString(decrypted3);
            Console.WriteLine("AES Encryption in .NET");
            Console.WriteLine("----------------------");
            Console.WriteLine();
            Console.WriteLine("Original Text = " + original3);
            Console.WriteLine("Encrypted Text = " + Convert.ToBase64String(encrypted3));
            Console.WriteLine("Decrypted Text = " + decryptedMessage3 + Environment.NewLine);
        }
    }

    class PassAndNumb
    {
        public byte[] GenerateRandomNumber(int length)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[length];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }

        public byte[] HashPassword(byte[] toBeHashed, int iter, int length)
        {
            byte[] salt = GenerateRandomNumber(32);
            var HashAl = new HashAlgorithmName("SHA1");
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, iter, HashAl))
            {
                return rfc2898.GetBytes(length);
            }
        }
    }

    class DesEncryption
    {
        public byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = key;
                des.IV = iv;
                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }

        public byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = key;
                des.IV = iv;
                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }
    }

    class TripleDesEncryption
    {
        public byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            using (var des = new TripleDESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = key;
                des.IV = iv;
                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }

        public byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var des = new TripleDESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = key;
                des.IV = iv;
                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }
    }

    class AesEncryption
    {
        public byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.IV = iv;
                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }
        public byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var des = new AesCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = key;
                des.IV = iv;
                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
