using Bannerlord.ButterLib.Common.Helpers;

using NUnit.Framework;

using System;

namespace Bannerlord.ButterLib.Tests
{
    public class AlphanumComparatorFastTests
    {
        [Test]
        public void Test()
        {
            var input = new[]
            {
                "100F",
                "50F",
                "SR100",
                "SR9"
            };

            Array.Sort(input, new AlphanumComparatorFast());

            var result = new[]
            {
                "50F",
                "100F",
                "SR9",
                "SR100"
            };

            Assert.AreEqual(result, input);
        }
    }
}