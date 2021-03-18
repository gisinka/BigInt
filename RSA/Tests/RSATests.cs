using System.Text;
using NUnit.Framework;
using RSA.Model;

namespace RSA.Tests
{
    [TestFixture]
    internal class RSATests
    {
        [TestCase("17", "19", "Test")]
        [TestCase("101", "103", "Test")]
        [TestCase("5009", "7013", "Test")]
        [TestCase("100043", "100049", "Test")]
        [TestCase("408689", "580529", "Test")]
        [TestCase("2375963", "2379989", "Test")]
        [TestCase("17", "19", "Мой дядя самых честных правил")]
        [TestCase("101", "103", "Мой дядя самых честных правил")]
        [TestCase("5009", "7013", "Мой дядя самых честных правил")]
        [TestCase("100043", "100049", "Мой дядя самых честных правил")]
        [TestCase("408689", "580529", "Мой дядя самых честных правил")]
        [TestCase("2375963", "2379989", "Мой дядя самых честных правил")]
        public static void EncryptionDecryptionTest(string p, string q, string input)
        {
            var rsa = new RSACryptor(ulong.Parse(p), ulong.Parse(q));
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var decryptedBytes = rsa.Decrypt(rsa.Encrypt(inputBytes));
            Assert.AreEqual(inputBytes, decryptedBytes);
        }
    }
}