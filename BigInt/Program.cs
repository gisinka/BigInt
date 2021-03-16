using System;
using System.Text;

namespace BigInt
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Команды:\r\n1.encrypt\r\n2.decrypt\r\n3.test\r\n4.exit");
                FileCryptor fc;
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("Введите простые числа P и Q");
                        fc = new FileCryptor(
                            uint.Parse(Console.ReadLine() ??
                                       throw new InvalidOperationException("Пустая строка - число")),
                            uint.Parse(Console.ReadLine() ??
                                       throw new InvalidOperationException("Пустая строка - число")));
                        Console.WriteLine("Введите имя файла");
                        fc.Encrypt(Console.ReadLine());
                        break;

                    case "2":
                        Console.WriteLine("Введите простые числа P и Q");
                        fc = new FileCryptor(
                            uint.Parse(Console.ReadLine() ??
                                       throw new InvalidOperationException("Пустая строка - число")),
                            uint.Parse(Console.ReadLine() ??
                                       throw new InvalidOperationException("Пустая строка - число")));
                        Console.WriteLine("Введите имя файла");
                        fc.Decrypt(Console.ReadLine());
                        break;

                    case "3":
                        Console.WriteLine("Введите строку для шифрования");
                        var input = Console.ReadLine();
                        Console.WriteLine("Введите простые числа P и Q");
                        var encryptor =
                            new RSACryptor(
                                uint.Parse(Console.ReadLine() ??
                                           throw new InvalidOperationException("Пустая строка - число")),
                                uint.Parse(Console.ReadLine() ??
                                           throw new InvalidOperationException("Пустая строка - число")));
                        var inputBytes = Encoding.UTF8.GetBytes(input);
                        Console.WriteLine(Encoding.UTF8.GetString(inputBytes));
                        Console.WriteLine("Input Bytes \r\n" + BitConverter.ToString(inputBytes));

                        var encrypted = encryptor.Encrypt(inputBytes);
                        Console.WriteLine(Encoding.UTF8.GetString(encrypted));
                        Console.WriteLine("Output Bytes \r\n" + BitConverter.ToString(encrypted));

                        var decrypted = encryptor.Decrypt(encrypted);
                        Console.WriteLine(Encoding.UTF8.GetString(decrypted));
                        Console.WriteLine("Decrypted Bytes \r\n" + BitConverter.ToString(decrypted));
                        Console.WriteLine(
                            $"Euler Function = {encryptor.GetEulerFunction()}, Module = {encryptor.GetModule()}, Secret Exponent = {encryptor.GetPrivateKey().Item1}, Public Exponent = {encryptor.GetPublicKey().Item1}");
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
    }
}