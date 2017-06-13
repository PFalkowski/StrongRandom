using System;
using System.Collections.Generic;
using Xunit;
using Extensions.Standard;

namespace Extensions.Standard.RandomExtensions.Test
{
    public class UtilitiesTest
    {
        [Fact]
        public void RandByteTest1()
        {
            const int repeats = 100;
            var onesCounter = 0;
            var zeroesCounter = 0;
            var errorCounter = 0;
            var rng = new Random();
            for (var i = 0; i < repeats; ++i)
            {
                var x = rng.NextByte(2);
                if (x == 0)
                    ++zeroesCounter;
                else if (x == 1)
                    ++onesCounter;
                else
                    ++errorCounter;
            }

            Assert.Equal(0, errorCounter);
            Assert.True(repeats / 10.0 < zeroesCounter);
            Assert.True(repeats / 10.0 < onesCounter);
        }

        [Fact]
        public void RandByteTest2()
        {
            var repeats = 100;
            var randomNumbers = new HashSet<byte>();
            var rng = new Random();
            for (var i = 0; i < repeats; ++i)
            {
                var res = rng.NextByte();
                Assert.True(res.InOpenRange(byte.MinValue, byte.MaxValue));
                randomNumbers.Add(res);
            }
            if (repeats >= 1000000)
                Assert.Equal(256, randomNumbers.Count);// In randomNumbers should be all integers in range <0,255>
        }

    }
}
