using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigInt
{
    internal class RSACryptor
    {
        private readonly BigInt _eulerFunction;
        private readonly BigInt _module;
        private readonly BigInt _publicExponent;
        private readonly BigInt _secretExponent;

        public RSACryptor(uint p, uint q)
        {
            if (!(IsPrime(p) && IsPrime(q)))
                throw new ArgumentException("P и Q должны быть простыми числами");
            _module = new BigInt(p) * new BigInt(q);
            _eulerFunction = (new BigInt(p) - BigInt.One) * (new BigInt(q) - BigInt.One);
            _secretExponent = CreateSecretExponent(_eulerFunction);
            _publicExponent = _secretExponent.GetReverseElement(_eulerFunction);
            Validate();
        }

        public byte[] Encrypt(string input)
        {
            return Encrypt(Encoding.Default.GetBytes(input), _publicExponent, _module);
        }

        public byte[] Encrypt(byte[] input)
        {
            return Encrypt(input, _publicExponent, _module);
        }

        public byte[] Decrypt(IEnumerable<byte> input)
        {
            return Decrypt(input, _secretExponent, _module);
        }

        public string DecryptToString(IEnumerable<byte> input)
        {
            return Encoding.Default.GetString(Decrypt(input, _secretExponent, _module));
        }

        public BigInt GetEulerFunction()
        {
            return _eulerFunction;
        }

        public BigInt GetModule()
        {
            return _module;
        }

        public (BigInt, BigInt) GetPublicKey()
        {
            return (_publicExponent, _module);
        }

        public (BigInt, BigInt) GetPrivateKey()
        {
            return (_secretExponent, _module);
        }

        public static byte[] Encrypt(byte[] input, BigInt e, BigInt n)
        {
            return input
                .Select(item => new BigInt(item).ModPow(e, n).ConvertToUInt())
                .Select(x => BitConverter.GetBytes(x))
                .SelectMany(x => x)
                .ToArray();
        }

        public static byte[] Decrypt(IEnumerable<byte> input, BigInt d, BigInt n)
        {
            return input
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 4)
                .Select(x => x.Select(v => v.Value).ToArray())
                .Select(x => BitConverter.ToUInt32(x, 0))
                .Select(x => new BigInt(x).ModPow(d, n))
                .Select(x => (byte) x.ConvertToUInt())
                .ToArray();
        }

        private void Validate()
        {
            if (_publicExponent.IsZero && !_secretExponent.IsZero)
                throw new ArgumentException("Публичная и приватная экспонента не могут быть равны 0");
            if (_eulerFunction < new BigInt(256) && _module < new BigInt(256))
                throw new ArgumentException("Значение функции Эйлера и модуль не должны быть меньше одного байта");
        }

        public static BigInt CreateSecretExponent(BigInt mod)
        {
            var exp = new BigInt(10);
            for (var i = BigInt.Two; i < mod; i++)
            {
                if (BigInt.GCD(exp, mod, out _, out _) == BigInt.One)
                    return exp;
                exp++;
            }

            return exp;
        }

        private static bool IsPrime(uint n)
        {
            if (n > 1)
            {
                for (var i = 2u; i < n; i++)
                {
                    if (n % i != 0) continue;
                    return false;
                }
            }
            else
                return false;

            return true;
        }
    }
}