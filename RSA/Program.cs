using System;
using System.Text;
using RSA.Model;

namespace RSA
{
    internal class Program
    {
        private static void Main()
        {
            while (true)
            {
                Console.WriteLine("Команды:\r\n1.encrypt\r\n2.decrypt\r\n3.test\r\n4.exit");

                switch (Console.ReadLine())
                {
                    case "1":
                        FileEncrypt();
                        break;

                    case "2":
                        FileDecrypt();
                        break;

                    case "3":
                        ConsoleTest();
                        break;

                    case "4":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Команда не распознана");
                        break;
                }
            }
        }

        private static void FileDecrypt()
        {
            Console.WriteLine("Введите простые числа P и Q");
            var p = ulong.Parse(Console.ReadLine() ??
                                throw new InvalidOperationException("Пустая строка - число"));
            var q = ulong.Parse(Console.ReadLine() ??
                                throw new InvalidOperationException("Пустая строка - число"));
            var fc = new FileCryptor(p, q);
            Console.WriteLine("Введите имя файла");
            fc.Decrypt(Console.ReadLine());
        }

        private static void FileEncrypt()
        {
            Console.WriteLine("Введите простые числа P и Q");
            var p = ulong.Parse(Console.ReadLine() ??
                                throw new InvalidOperationException("Пустая строка - число"));
            var q = ulong.Parse(Console.ReadLine() ??
                                throw new InvalidOperationException("Пустая строка - число"));
            var fc = new FileCryptor(p, q);
            Console.WriteLine("Введите имя файла");
            fc.Encrypt(Console.ReadLine());
        }

        private static void ConsoleTest()
        {
            Console.WriteLine("Введите простые числа P и Q");
            var p = ulong.Parse(Console.ReadLine() ??
                                throw new InvalidOperationException("Пустая строка - число"));
            var q = ulong.Parse(Console.ReadLine() ??
                                throw new InvalidOperationException("Пустая строка - число"));

            var encryptor = new RSACryptor(p, q);

            Console.WriteLine("Введите строку для шифрования");
            var input = Console.ReadLine();

            var inputBytes = Encoding.UTF8.GetBytes(input ?? string.Empty);
            Console.WriteLine(input);
            Console.WriteLine("Input Bytes \r\n" + BitConverter.ToString(inputBytes));

            var encrypted = encryptor.Encrypt(inputBytes);
            Console.WriteLine(Encoding.UTF8.GetString(encrypted));
            Console.WriteLine("Encrypted Bytes \r\n" + BitConverter.ToString(encrypted));

            var decrypted = encryptor.Decrypt(encrypted);
            Console.WriteLine(Encoding.UTF8.GetString(decrypted));
            Console.WriteLine("Decrypted Bytes \r\n" + BitConverter.ToString(decrypted));

            Console.WriteLine(
                $"Euler Function = {encryptor.GetEulerFunction()}, Module = {encryptor.GetModule()}, Secret Exponent = {encryptor.GetPrivateKey().Item1}, Public Exponent = {encryptor.GetPublicKey().Item1}");
        }
    }
}