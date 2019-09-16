using Slovar.Models;
using Slovar;
using NUnit.Framework;

namespace Slovar.Tests
{
    public class LemmaNormalizerTestCase
    {
        [Test]
        public void NormalizeTestWithSimpleLemma()
        {
            var simple = "абв";
            var normalizer = new LemmaNormalizer(simple);
            Assert.AreEqual(simple, normalizer.Normalize());
        }

        [Test]
        public void NormalizeTestWithLetter_ё()
        {
            var complex = "ёлка";
            var normalizer = new LemmaNormalizer(complex);
            Assert.AreEqual("елка", normalizer.Normalize());
        }
    }
}
