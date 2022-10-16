using System;
using System.Text;
using System.Security.Cryptography;

namespace PractZadan_3
{
    class Program
    {
        static void Main(string[] args)
        {
            string text1 = "Text about something";
            byte[] data1 = Encoding.Unicode.GetBytes(text1);

            var md5ForText = MD5Hash(data1);
            Guid guidForText = new Guid(md5ForText);
            Console.WriteLine($"Our text is: {text1} ");
            Console.WriteLine($"Hash MD5: {Convert.ToBase64String(md5ForText)}");
            Console.WriteLine($"GUID:{guidForText}");

            var ShaForText = Sha1Hash(data1);
            Console.WriteLine($"Our text is: {text1} ");
            Console.WriteLine($"Hash Sha5: {Convert.ToBase64String(ShaForText)}");

            var Sha256ForText = Sha256Hash(data1);
            Console.WriteLine($"Hash Sha256: {Convert.ToBase64String(Sha256ForText)}");
            var Sha384ForText = Sha384Hash(data1);
            Console.WriteLine($"Hash Sha384: {Convert.ToBase64String(Sha384ForText)}");
            var Sha512ForText = Sha512Hash(data1);
            Console.WriteLine($"Hash Sha512: {Convert.ToBase64String(Sha512ForText)}" + Environment.NewLine);

            string text2 = "Another Text About Something New";
            byte[] data2 = Encoding.Unicode.GetBytes(text2);

            var md5ForText2 = MD5Hash(data2);
            Guid guidForText2 = new Guid(md5ForText);
            Console.WriteLine($"Our text is: {text2} ");
            Console.WriteLine($"Hash MD5: {Convert.ToBase64String(md5ForText2)}");
            Console.WriteLine($"GUID:{guidForText2}");

            var ShaForText2 = Sha1Hash(data2);
            Console.WriteLine($"Hash Sha5: {Convert.ToBase64String(ShaForText2)}");
            var Sha256ForText2 = Sha256Hash(data2);
            Console.WriteLine($"Hash Sha256: {Convert.ToBase64String(Sha256ForText2)}");
            var Sha384ForText2 = Sha384Hash(data2);
            Console.WriteLine($"Hash Sha384: {Convert.ToBase64String(Sha384ForText2)}");
            var Sha512ForText2 = Sha512Hash(data2);
            Console.WriteLine($"Hash Sha512: {Convert.ToBase64String(Sha512ForText2)}" + Environment.NewLine);

            string text3 = "Another Text About Something new";
            byte[] data3 = Encoding.Unicode.GetBytes(text3);

            var md5ForText3 = MD5Hash(data3);
            Guid guidForText3 = new Guid(md5ForText);
            Console.WriteLine($"Our text is: {text3} ");
            Console.WriteLine($"Hash MD5: {Convert.ToBase64String(md5ForText3)}");
            Console.WriteLine($"GUID:{guidForText3}");

            var ShaForText3 = Sha1Hash(data3);
            Console.WriteLine($"Hash Sha5: {Convert.ToBase64String(ShaForText3)}");
            var Sha256ForText3 = Sha256Hash(data3);
            Console.WriteLine($"Hash Sha256: {Convert.ToBase64String(Sha256ForText3)}");
            var Sha384ForText3 = Sha384Hash(data3);
            Console.WriteLine($"Hash Sha384: {Convert.ToBase64String(Sha384ForText3)}");
            var Sha512ForText3 = Sha512Hash(data3);
            Console.WriteLine($"Hash Sha512: {Convert.ToBase64String(Sha512ForText3)}" + Environment.NewLine);
        }

        static byte[] MD5Hash(byte[] data)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(data);
            }
        }
        static byte[] Sha1Hash(byte[] data)
        {
            using (var sha = SHA1.Create())
            {
                return sha.ComputeHash(data);
            }
        }

        static byte[] Sha256Hash(byte[] data)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(data);
            }
        }
        static byte[] Sha384Hash(byte[] data)
        {
            using (var sha384 = SHA384.Create())
            {
                return sha384.ComputeHash(data);
            }
        }
        static byte[] Sha512Hash(byte[] data)
        {
            using (var sha512 = SHA512.Create())
            {
                return sha512.ComputeHash(data);
            }
        }

    }
}
