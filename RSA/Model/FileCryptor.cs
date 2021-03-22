using System;
using System.IO;

namespace RSA.Model
{
    internal class FileCryptor
    {
        private readonly RSACryptor _rsa;

        public FileCryptor(ulong p, ulong q)
        {
            _rsa = new RSACryptor(p, q);
        }

        public void Encrypt(string filename)
        {
            var path = $"{Directory.GetCurrentDirectory()}\\{filename}";
            var input = File.ReadAllBytes(path);
            File.WriteAllBytes(path, _rsa.Encrypt(input));
            Console.WriteLine($"{path} зашифрован");
            Console.WriteLine("Публичный ключ " + GetPublicKey());
            Console.WriteLine("Приватный ключ " + GetPrivateKey());
        }

        public void Decrypt(string filename)
        {
            var path = $"{Directory.GetCurrentDirectory()}\\{filename}";
            var input = File.ReadAllBytes(path);
            File.WriteAllBytes(path, _rsa.Decrypt(input));
            Console.WriteLine($"{path} расшифрован");
        }

        public static void Decrypt(string filename, BigInt d, BigInt n)
        {
            var path = $"{Directory.GetCurrentDirectory()}\\{filename}";
            var input = File.ReadAllBytes(path);
            File.WriteAllBytes(path, RSACryptor.Decrypt(input, d, n));
            Console.WriteLine($"{path} расшифрован");
        }

        public (BigInt, BigInt) GetPublicKey()
        {
            return _rsa.GetPublicKey();
        }

        public (BigInt, BigInt) GetPrivateKey()
        {
            return _rsa.GetPrivateKey();
        }
    }
}