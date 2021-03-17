using System;
using NUnit.Framework;

namespace BigInt
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
    }
}