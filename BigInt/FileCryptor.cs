using System;
using System.IO;

namespace BigInt
{
    internal class FileCryptor
    {
        private readonly RSACryptor _rsa;

        public FileCryptor(uint p, uint q)
        {
            _rsa = new RSACryptor(p, q);
        }

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
        }

        public void Decrypt(string filename)
        {
            var path = $"{Directory.GetCurrentDirectory()}\\{filename}";
            var input = File.ReadAllBytes(path);
            File.WriteAllBytes(path, _rsa.Decrypt(input));
            Console.WriteLine($"{path} расшифрован");
        }
    }
}