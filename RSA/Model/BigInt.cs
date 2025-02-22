﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSA.Model
{
    internal class BigInt
    {
        public readonly List<byte> Bytes;

        public bool IsPositive { get; private set; } = true;

        public static BigInt Zero => new BigInt(0);

        public static BigInt One => new BigInt(1);

        public static BigInt Two => new BigInt(2);

        public bool IsZero => Bytes.Count == 0 || Bytes.All(x => x == 0) || this == Zero;

        public int Count => Bytes.Count;

        private byte this[int index]
        {
            get => index < Count ? Bytes[index] : (byte) 0;
            set
            {
                while (Bytes.Count <= index) Bytes.Add(0);
                Bytes[index] = value;
            }
        }

        public BigInt(string str)
        {
            Bytes = new List<byte>();
            if (str.StartsWith("-"))
            {
                IsPositive = false;
                str = str.Substring(1);
            }

            if (str.StartsWith("+")) str = str.Substring(1);

            if (!str.All(char.IsDigit))
                throw new ArgumentException("Входная строка не являлась числом");

            foreach (var c in str.Reverse()) Bytes.Add(Convert.ToByte(c.ToString()));

            RemoveNulls();
        }

        public BigInt(BigInt bigInt)
        {
            IsPositive = bigInt.IsPositive;
            Bytes = bigInt.Bytes.GetRange(0, bigInt.Bytes.Count);
        }

        public BigInt(IEnumerable<byte> bytes)
        {
            Bytes = new List<byte>(bytes);
        }

        private BigInt(bool isPositive, List<byte> bytes)
        {
            IsPositive = isPositive;
            Bytes = bytes;
            RemoveNulls();
        }

        public BigInt(int value)
        {
            Bytes = new List<byte>();
            if (value < 0) IsPositive = false;
            Bytes.AddRange(GetBytes((ulong) Math.Abs(value)));
        }

        public BigInt(uint value)
        {
            Bytes = new List<byte>();
            Bytes.AddRange(GetBytes(value));
        }

        public BigInt(long value)
        {
            Bytes = new List<byte>();
            if (value < 0) IsPositive = false;
            Bytes.AddRange(GetBytes((ulong) Math.Abs(value)));
        }

        public BigInt(ulong value)
        {
            Bytes = new List<byte>();
            Bytes.AddRange(GetBytes(value));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Comparison(this, (BigInt) obj) == 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Bytes != null ? Bytes.GetHashCode() : 0) * 397) ^ IsPositive.GetHashCode();
            }
        }

        public override string ToString()
        {
            if (this == Zero) return "0";
            var s = new StringBuilder(IsPositive ? "" : "-");
            for (var i = Bytes.Count - 1; i >= 0; i--) s.Append(Convert.ToString(Bytes[i]));

            return s.ToString();
        }

        public byte ConvertToByte()
        {
            return ConvertToByte(this);
        }

        private void RemoveNulls()
        {
            for (var i = Bytes.Count - 1; i > 0; i--)
                if (Bytes[i] == 0)
                    Bytes.RemoveAt(i);
                else
                    break;
        }

        private List<BigInt> ConvertToBinary()
        {
            var itemToConvert = new BigInt(this);
            var result = new List<BigInt>();
            while (!itemToConvert.IsZero)
            {
                result.Add(itemToConvert % Two);
                itemToConvert /= Two;
            }

            return result;
        }

        public byte[] GetPackedBCD(int length)
        {
            var bytes = new List<byte>();
            var counter = length > Count ? length : Count;
            for (var i = 0; i < counter; i += 2)
            {
                var first = this[i];
                var second = (byte) (this[i + 1] << 4);
                bytes.Add((byte) (first | second));
            }

            return bytes.ToArray();
        }

        public static BigInt GetUnpackedBCD(IEnumerable<byte> packed)
        {
            var bytes = new List<byte>();
            foreach (var item in packed)
            {
                bytes.Add((byte) (item & 0x0F));
                bytes.Add((byte) ((item & 0xF0) >> 4));
            }

            return new BigInt(bytes);
        }

        public static int Comparison(BigInt a, BigInt b, bool ignoreSign = false)
        {
            return CompareSign(a, b, ignoreSign);
        }

        private static int CompareSign(BigInt a, BigInt b, bool ignoreSign = false)
        {
            if (ignoreSign) return CompareSize(a, b);
            if (!a.IsPositive && b.IsPositive)
                return -1;
            if (a.IsPositive && !b.IsPositive)
                return 1;

            return CompareSize(a, b);
        }

        private static int CompareSize(BigInt a, BigInt b)
        {
            if (a.Count < b.Count)
                return -1;
            return a.Count > b.Count ? 1 : CompareDigits(a, b);
        }

        private static int CompareDigits(BigInt a, BigInt b)
        {
            for (var i = Math.Max(a.Count, b.Count); i > -1; i--)
                if (a[i] < b[i])
                    return -1;
                else if (a[i] > b[i]) return 1;

            return 0;
        }

        private static BigInt Add(BigInt a, BigInt b)
        {
            var bytes = new List<byte>();
            byte carry = 0;

            for (var i = 0; i < Math.Max(a.Count, b.Count); i++)
            {
                var current = (byte) (a[i] + b[i] + carry);
                carry = (byte) (current / 10);
                bytes.Add((byte) (current % 10));
            }

            if (carry > 0) bytes.Add(carry);

            return new BigInt(a.IsPositive, bytes);
        }

        private static BigInt Subtract(BigInt a, BigInt b)
        {
            var bytes = new List<byte>();
            var max = Zero;
            var min = Zero;

            switch (Comparison(a, b, true))
            {
                case -1:
                    min = a;
                    max = b;
                    break;
                case 0:
                    return Zero;
                case 1:
                    min = b;
                    max = a;
                    break;
            }

            var carry = 0;
            for (var i = 0; i < Math.Max(a.Count, b.Count); i++)
            {
                var current = max[i] - min[i] - carry;
                if (current < 0)
                {
                    current += 10;
                    carry = 1;
                }
                else
                {
                    carry = 0;
                }

                bytes.Add((byte) current);
            }

            return new BigInt(max.IsPositive, bytes);
        }

        private static BigInt Multiply(BigInt a, BigInt b)
        {
            try
            {
                var result = Zero;

                for (var i = 0; i < a.Count; i++)
                for (int j = 0, carry = 0; j < b.Count || carry > 0; j++)
                {
                    var current = result[i + j] + a[i] * b[j] + carry;
                    result[i + j] = (byte) (current % 10);
                    carry = current / 10;
                }

                result.IsPositive = a.IsPositive == b.IsPositive || result == Zero;
                result.RemoveNulls();
                return result;
            }
            catch (Exception)
            {
                throw new ArithmeticException("Multiplication overflow");
            }
        }

        private static BigInt Div(BigInt a, BigInt b)
        {
            if (b == Zero) throw new DivideByZeroException();

            var otherPositive = b.IsPositive ? b : new BigInt(b.Bytes);
            var result = Zero;
            var current = Zero;

            try
            {
                for (var i = a.Count - 1; i >= 0; i--)
                {
                    current += Exp(a[i], i);

                    var x = 0;
                    var l = 0;
                    var r = 10;
                    while (l <= r)
                    {
                        var m = (l + r) / 2;
                        var cur = otherPositive * Exp((byte) m, i);
                        if (cur <= current)
                        {
                            x = m;
                            l = m + 1;
                        }
                        else
                        {
                            r = m - 1;
                        }
                    }

                    result[i] = (byte) (x % 10);
                    var t = otherPositive * Exp((byte) x, i);
                    current -= t;
                }

                result.IsPositive = a.IsPositive == b.IsPositive || result == Zero;
                result.RemoveNulls();
                return result;
            }
            catch (Exception)
            {
                throw new ArithmeticException("Division overflow");
            }
        }

        private static BigInt Mod(BigInt a, BigInt b)
        {
            if (b == Zero) throw new DivideByZeroException();
            var result = Zero;
            var otherPositive = b.IsPositive ? b : new BigInt(b.Bytes);

            try
            {
                for (var i = a.Count - 1; i >= 0; i--)
                {
                    result += Exp(a[i], i);

                    var x = 0;
                    var l = 0;
                    var r = 10;

                    while (l <= r)
                    {
                        var m = (l + r) / 2;
                        var cur = otherPositive * Exp((byte) m, i);
                        if (cur <= result)
                        {
                            x = m;
                            l = m + 1;
                        }
                        else
                        {
                            r = m - 1;
                        }
                    }

                    result -= otherPositive * Exp((byte) x, i);
                }

                if (result.IsZero)
                {
                    result.IsPositive = true;
                }
                else if (a.IsPositive == b.IsPositive)
                {
                    result.IsPositive = a.IsPositive;
                }
                else if (a.IsPositive && !b.IsPositive)
                {
                    result += b;
                }
                else if (!a.IsPositive && b.IsPositive)
                {
                    result -= b;
                    result.IsPositive = true;
                }

                result.RemoveNulls();

                return result;
            }
            catch (Exception)
            {
                throw new ArithmeticException("Mod overflow");
            }
        }

        private static IEnumerable<byte> GetBytes(ulong value)
        {
            var bytes = new List<byte>();
            do
            {
                bytes.Add((byte) (value % 10));
                value /= 10;
            } while (value > 0);

            return bytes;
        }

        public static BigInt Exp(byte val, int exp)
        {
            var bigInt = Zero;
            bigInt[exp] = val;
            bigInt.RemoveNulls();
            return bigInt;
        }

        public static byte ConvertToByte(BigInt a)
        {
            if (CompareDigits(a, new BigInt(byte.MaxValue)) > 0 && a.IsPositive)
                throw new ArithmeticException("The value is greater than the int");
            byte res = 0;
            var i = 1;
            foreach (var digit in a.Bytes)
            {
                res += (byte) (digit * i);
                i *= 10;
            }

            return res;
        }

        public static BigInt Pow(BigInt number, BigInt power)
        {
            if (power < Zero)
                throw new ArgumentException("Степень не должна быть меньше нуля");
            if (number == Zero)
                return Zero;
            if (power == Zero)
                return One;
            if (power % Two == One)
                return Pow(number, power - One) * number;
            var b = Pow(number, power / Two);
            return b * b;
        }

        public static BigInt ModPow(BigInt number, BigInt power, BigInt module)
        {
            if (power > Zero)
            {
                var binaryValue = power.ConvertToBinary();

                var arr = new BigInt[binaryValue.Count];
                arr[0] = new BigInt(number);
                for (var i = 1; i < binaryValue.Count; i++)
                    arr[i] = arr[i - 1] * arr[i - 1] % module;

                var mult = One;
                for (var j = 0; j < binaryValue.Count; j++)
                    if (binaryValue[j] > Zero)
                        mult *= binaryValue[j] * arr[j];
                mult.IsPositive = number.IsPositive;
                return mult % module;
            }

            if (power < Zero) throw new ArgumentException("Степень не должна быть меньше нуля");

            return (number > Zero ? One : Zero) % module;
        }

        public static BigInt RunExtendedEuclidean(BigInt number, BigInt mod, out BigInt x,
            out BigInt y)
        {
            if (number.IsZero)
            {
                x = Zero;
                y = One;
                return mod;
            }

            var d = RunExtendedEuclidean(mod % number, number, out var x1, out var y1);
            x = y1 - mod / number * x1;
            y = x1;
            return d;
        }

        public BigInt GetReverseElement(BigInt m)
        {
            var gdc = RunExtendedEuclidean(this, m, out var x, out var _);
            if (gdc != One)
                return Zero;
            return (x % m + m) % m;
        }

        public BigInt Pow(int n)
        {
            return Pow(this, new BigInt(n));
        }

        public BigInt Pow(BigInt n)
        {
            return Pow(this, n);
        }

        public BigInt ModPow(BigInt power, BigInt module)
        {
            return ModPow(this, power, module);
        }

        public static bool operator <(BigInt a, BigInt b)
        {
            return Comparison(a, b) < 0;
        }

        public static bool operator >(BigInt a, BigInt b)
        {
            return Comparison(a, b) > 0;
        }

        public static bool operator <=(BigInt a, BigInt b)
        {
            return Comparison(a, b) <= 0;
        }

        public static bool operator >=(BigInt a, BigInt b)
        {
            return Comparison(a, b) >= 0;
        }

        public static bool operator ==(BigInt a, BigInt b)
        {
            return Comparison(a, b) == 0;
        }

        public static bool operator !=(BigInt a, BigInt b)
        {
            return Comparison(a, b) != 0;
        }

        public static BigInt operator -(BigInt a)
        {
            return new BigInt(!a.IsPositive, a.Bytes.GetRange(0, a.Count));
        }

        public static BigInt operator ++(BigInt a)
        {
            return a + One;
        }

        public static BigInt operator --(BigInt a)
        {
            return a - One;
        }

        public static BigInt operator +(BigInt a, BigInt b)
        {
            return a.IsPositive == b.IsPositive
                ? Add(a, b)
                : Subtract(a, b);
        }

        public static BigInt operator -(BigInt a, BigInt b)
        {
            return a + -b;
        }

        public static BigInt operator *(BigInt a, BigInt b)
        {
            return Multiply(a, b);
        }

        public static BigInt operator /(BigInt a, BigInt b)
        {
            return Div(a, b);
        }

        public static BigInt operator %(BigInt a, BigInt b)
        {
            return Mod(a, b);
        }
    }
}