using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace PractZadan_7
{
    class Program
    {
        static void Main(string[] args)
        {
            string ppath = @"publ.xml";
            var document = Encoding.UTF8.GetBytes("Document to Sign");
            byte[] hashedDocument;
            using (var sha256 = SHA256.Create())
            {
                hashedDocument = sha256.ComputeHash(document);
            }
            var wrongdocument = Encoding.UTF8.GetBytes("Wrong Document");
            byte[] testdocument;
            using (var sha256 = SHA256.Create())
            {
                testdocument = sha256.ComputeHash(wrongdocument);
            }
            var digitalSignature = new DigitalSignature();
            digitalSignature.AssignNewKeys(ppath);
            var signature = digitalSignature.SignData(hashedDocument);
            var verified = digitalSignature.VerifySignature(ppath,hashedDocument, signature);
            var notverified = digitalSignature.VerifySignature(ppath, testdocument, signature);
            Console.WriteLine("Digital Signature Demonstration in .NET");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine(" Original Text = " + Encoding.Default.GetString(document));
            Console.WriteLine();
            Console.WriteLine(" Digital Signature = " + Convert.ToBase64String(signature));
            Console.WriteLine(verified
            ? "The digital signature has been correctly verified."
            : "The digital signature has NOT been correctly verified.");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine(" Wrong original text = " + Encoding.Default.GetString(wrongdocument));
            Console.WriteLine(notverified
            ? "The digital signature has been correctly verified."
            : "The digital signature has NOT been correctly verified.");
        }
    }

    class DigitalSignature
    {
        const string CspContainerName = "MyContainer";
        public void AssignNewKeys(string publicKeyPath)
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
        public byte[] SignData(byte[] data)
        {
            var cspParams = new CspParameters
            {
                KeyContainerName = CspContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            using (var rsa = new RSACryptoServiceProvider(2048, cspParams))
            {
                rsa.PersistKeyInCsp = true;
                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm(nameof(SHA512));
                byte[] hashOfData;
                using (var sha512 = SHA512.Create())
                {
                    hashOfData = sha512.ComputeHash(data);
                }
                return rsaFormatter.CreateSignature(hashOfData);
            }
        }
        public bool VerifySignature(string publicKeyPath, byte[] data, byte[] signature)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(publicKeyPath));
                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm(nameof(SHA512));
                byte[] hashOfData;
                using (var sha512 = SHA512.Create())
                {
                    hashOfData = sha512.ComputeHash(data);
                }
                return rsaDeformatter.VerifySignature(hashOfData, signature);
            }
        }
    }
}
