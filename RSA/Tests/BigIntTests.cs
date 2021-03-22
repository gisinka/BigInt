using System;
using NUnit.Framework;
using RSA.Model;

namespace RSA.Tests
{
    [TestFixture]
    internal class BigIntTests
    {
        [TestCase("0", "0", "0")]
        [TestCase("1", "2", "3")]
        [TestCase("-123456789", "123456789", "0")]
        [TestCase("451654784513168546", "0", "451654784513168546")]
        [TestCase("451654784513168546", "4654684641324168465416546", "4654685092978952978585092")]
        [TestCase("-39392850923850928340598", "0", "-39392850923850928340598")]
        [TestCase("-1130948019840981029480184", "590238759872306598450", "-1130357781081108722881734")]
        [TestCase("-39392850923850928340598", "-759083275987213948759287359", "-759122668838137799687627957")]
        [TestCase("+451654784513168546", "+0", "+451654784513168546")]
        [TestCase("+451654784513168546", "+4654684641324168465416546", "+4654685092978952978585092")]
        [TestCase("-39392850923850928340598", "+0", "-39392850923850928340598")]
        [TestCase("-1130948019840981029480184", "+590238759872306598450", "-1130357781081108722881734")]
        public static void SumTest(string first, string second, string expected)
        {
            Assert.AreEqual(new BigInt(expected), new BigInt(first) + new BigInt(second));
        }

        [TestCase("0", "0", "0")]
        [TestCase("1", "2", "-1")]
        [TestCase("-1", "1", "-2")]
        [TestCase("759024876590290659823680298609", "1518049753180581319647360597218",
            "-759024876590290659823680298609")]
        [TestCase("759024876590290659823680298609", "-1518049753180581319647360597218",
            "2277074629770871979471040895827")]
        [TestCase("759024876590290659823680298609", "759024876590290659823680298609", "0")]
        [TestCase("-759024876590290659823680298609", "759024876590290659823680298609",
            "-1518049753180581319647360597218")]
        [TestCase("759024876590290659823680298609", "-759024876590290659823680298609",
            "1518049753180581319647360597218")]
        public static void SubtractionTest(string first, string second, string expected)
        {
            Assert.AreEqual(new BigInt(expected), new BigInt(first) - new BigInt(second));
        }

        [TestCase("0", "0", "0")]
        [TestCase("1", "2", "2")]
        [TestCase("-1", "1", "-1")]
        [TestCase("587230958712490590482150198240", "759024876590290659823680298609",
            "445722905966746240179169268044252586242510448730711746248160")]
        [TestCase("587230958712490590482150198240", "-759024876590290659823680298609",
            "-445722905966746240179169268044252586242510448730711746248160")]
        [TestCase("-587230958712490590482150198240", "-759024876590290659823680298609",
            "445722905966746240179169268044252586242510448730711746248160")]
        [TestCase("+587230958712490590482150198240", "+759024876590290659823680298609",
            "+445722905966746240179169268044252586242510448730711746248160")]
        [TestCase("-587230958712490590482150198240", "-759024876590290659823680298609",
            "+445722905966746240179169268044252586242510448730711746248160")]
        public static void MultiplicationTest(string first, string second, string expected)
        {
            Assert.AreEqual(new BigInt(expected), new BigInt(first) * new BigInt(second));
        }

        [TestCase("1", "2", "0")]
        [TestCase("-1", "1", "-1")]
        [TestCase("256", "16", "16")]
        [TestCase("-256", "16", "-16")]
        [TestCase("256", "-16", "-16")]
        [TestCase("-256", "-16", "16")]
        [TestCase("759024876590290659823680298609", "58723095871249081478939084", "12925")]
        [TestCase("-759024876590290659823680298609", "58723095871249081478939084", "-12925")]
        [TestCase("759024876590290659823680298609", "-58723095871249081478939084", "-12925")]
        [TestCase("-759024876590290659823680298609", "-58723095871249081478939084", "12925")]
        public static void DivisionTest(string first, string second, string expected)
        {
            Assert.AreEqual(new BigInt(expected), new BigInt(first) / new BigInt(second));
        }

        [TestCase("0", "0")]
        [TestCase("1", "0")]
        [TestCase("1526348609843026981034", "0")]
        [TestCase("+1526348609843026981034", "0")]
        [TestCase("-1526348609843026981034", "0")]
        public static void ThrowsDivideByZeroExceptionWhenDivisionTest(string first, string second)
        {
            Assert.That(() => new BigInt(first) / new BigInt(second), Throws.Exception.TypeOf<DivideByZeroException>());
        }


        [TestCase("1", "2", "1")]
        [TestCase("256", "16", "0")]
        [TestCase("1284", "17", "9")]
        [TestCase("-1284", "17", "8")]
        [TestCase("1284", "-17", "-8")]
        [TestCase("-1284", "-17", "-9")]
        [TestCase("1518049753180581319647360597218", "759024876590290659823680298609", "0")]
        [TestCase("759024876590290659823680298609", "58723095871249081478939084", "28862454396281708392637909")]
        [TestCase("-759024876590290659823680298609", "58723095871249081478939084", "29860641474967373086301175")]
        [TestCase("759024876590290659823680298609", "-58723095871249081478939084", "-29860641474967373086301175")]
        [TestCase("-759024876590290659823680298609", "-58723095871249081478939084", "-28862454396281708392637909")]
        public static void ModTest(string first, string second, string expected)
        {
            Assert.AreEqual(new BigInt(expected), new BigInt(first) % new BigInt(second));
        }

        [TestCase("0", "0")]
        [TestCase("1", "0")]
        [TestCase("1526348609843026981034", "0")]
        [TestCase("+1526348609843026981034", "0")]
        [TestCase("-1526348609843026981034", "0")]
        public static void ThrowsDivideByZeroExceptionWhenModTest(string first, string second)
        {
            Assert.That(() => new BigInt(first) % new BigInt(second), Throws.Exception.TypeOf<DivideByZeroException>());
        }

        [TestCase("0", "0", "0")]
        [TestCase("0", "1000", "0")]
        [TestCase("2", "9", "512")]
        [TestCase("-2", "9", "-512")]
        [TestCase("2", "10", "1024")]
        [TestCase("-2", "10", "1024")]
        [TestCase("4213524356245", "43",
            "724030521258007518583560483303916922332632960364092362998402917361677474014395383304344311140015998484627707482809914648605592580001778626038345295135449803908464144784647463444043764189178366186161599268931630969481540171809224743926918241221259678316874010727004567082165113812672323132373997378856731548163291480837726447019318125084527456276973381275675250015808027035359153720526122706537052703601524747390737967931777244260220707647992235194505120729012480482125273734749176426306432753136135312396534858690984037821181118488311767578125")]
        [TestCase("-4213524356245", "43",
            "-724030521258007518583560483303916922332632960364092362998402917361677474014395383304344311140015998484627707482809914648605592580001778626038345295135449803908464144784647463444043764189178366186161599268931630969481540171809224743926918241221259678316874010727004567082165113812672323132373997378856731548163291480837726447019318125084527456276973381275675250015808027035359153720526122706537052703601524747390737967931777244260220707647992235194505120729012480482125273734749176426306432753136135312396534858690984037821181118488311767578125")]
        public static void PowTest(string first, string second, string expected)
        {
            Assert.AreEqual(new BigInt(expected), new BigInt(first).Pow(new BigInt(second)));
        }

        [TestCase("0", "-1")]
        [TestCase("1", "-1000")]
        [TestCase("1526348609843026981034", "-43")]
        [TestCase("+1526348609843026981034", "-43")]
        [TestCase("-1526348609843026981034", "-43")]
        public static void ThrowsArgumentExceptionWhenPowerLessZeroTest(string first, string second)
        {
            Assert.That(() => new BigInt(first).Pow(new BigInt(second)), Throws.Exception.TypeOf<ArgumentException>());
        }

        [TestCase("1", "1", "1", "0")]
        [TestCase("0", "0", "16", "0")]
        [TestCase("0", "1", "16", "0")]
        [TestCase("0", "256", "16", "0")]
        [TestCase("1", "0", "16", "1")]
        [TestCase("256", "0", "16", "1")]
        [TestCase("1", "7", "16", "1")]
        [TestCase("256", "10", "43", "35")]
        [TestCase("532098759872395", "12", "5897423759", "1663887615")]
        [TestCase("-532098759872395", "12", "5897423759", "4233536144")]
        [TestCase("532098759872395", "12", "-5897423759", "-4233536144")]
        [TestCase("-532098759872395", "12", "-5897423759", "-1663887615")]
        public static void ModPowTest(string number, string power, string module, string expected)
        {
            Assert.AreEqual(new BigInt(expected), new BigInt(number).ModPow(new BigInt(power), new BigInt(module)));
        }

        [TestCase("0", "-1", "16")]
        [TestCase("532098759872395", "-12", "5897423759")]
        [TestCase("-532098759872395", "-12", "5897423759")]
        [TestCase("+532098759872395", "-12", "5897423759")]
        public static void ThrowsArgumentExceptionWhenModPowerLessZeroTest(string number, string power, string module)
        {
            Assert.That(() => new BigInt(number).ModPow(new BigInt(power), new BigInt(module)),
                Throws.Exception.TypeOf<ArgumentException>());
        }

        [TestCase("0", "0", 0)]
        [TestCase("1", "1", 0)]
        [TestCase("-1", "-1", 0)]
        [TestCase("+1", "1", 0)]
        [TestCase("+1", "+1", 0)]
        [TestCase("123456789123456789", "123456789123456789", 0)]
        [TestCase("+123456789123456789", "123456789123456789", 0)]
        [TestCase("+123456789123456789", "+123456789123456789", 0)]
        [TestCase("-123456789123456789", "-123456789123456789", 0)]
        [TestCase("1", "0", 1)]
        [TestCase("1", "-1", 1)]
        [TestCase("+1", "-1", 1)]
        [TestCase("123456789123456789", "0", 1)]
        [TestCase("+123456789123456789", "0", 1)]
        [TestCase("123456789123456789", "123456789", 1)]
        [TestCase("+123456789123456789", "123456789", 1)]
        [TestCase("123456789123456789", "-123456789123456789", 1)]
        [TestCase("0", "1", -1)]
        [TestCase("-1", "1", -1)]
        [TestCase("-1", "+1", -1)]
        [TestCase("-123456789123456789", "0", -1)]
        [TestCase("-123456789123456789", "123456789", -1)]
        [TestCase("0", "123456789123456789", -1)]
        [TestCase("0", "+123456789123456789", -1)]
        [TestCase("123456789", "123456789123456789", -1)]
        [TestCase("123456789", "+123456789123456789", -1)]
        [TestCase("-123456789123456789", "123456789123456789", -1)]
        public static void ComparisonTest(string first, string second, int expected)
        {
            Assert.AreEqual(expected, BigInt.Comparison(new BigInt(first), new BigInt(second)));
        }

        [TestCase("0", "-0", 0)]
        [TestCase("-0", "0", 0)]
        [TestCase("-1", "1", 0)]
        [TestCase("1", "-1", 0)]
        [TestCase("-1", "+1", 0)]
        [TestCase("+1", "-1", 0)]
        [TestCase("-123456789123456789", "123456789123456789", 0)]
        [TestCase("123456789123456789", "-123456789123456789", 0)]
        [TestCase("-123456789123456789", "+123456789123456789", 0)]
        [TestCase("+123456789123456789", "-123456789123456789", 0)]
        public static void EqualityIgnoreSignTest(string first, string second, int expected)
        {
            Assert.AreEqual(expected, BigInt.Comparison(new BigInt(first), new BigInt(second), true));
        }

        [TestCase("0", "1", "0")]
        [TestCase("3", "26", "9")]
        [TestCase("3", "7", "5")]
        [TestCase("7", "40832", "34999")]
        public static void ReverseElementTest(string number, string module, string expected)
        {
            Assert.AreEqual(new BigInt(expected), new BigInt(number).GetReverseElement(new BigInt(module)));
        }
    }
}