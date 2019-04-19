using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Extensions.Standard.Randomization.Test
{
    public class BufferedRadnomProviderTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void ConstructorCreatesValidInstance(int buffSize)
        {
            var tested = new BufferedRandomProvider(buffSize);
        }
        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        public void GetBytesReturnsFilledArray(int buffSize)
        {
            var tested = new BufferedRandomProvider(buffSize);
            var input = new byte[buffSize];
            tested.GetBytes(input);

            var uniqueBytes = new HashSet<byte>();
            foreach (var b in input) { uniqueBytes.Add(b); }
            Assert.True(uniqueBytes.Count > 1);
            Assert.Equal(buffSize, input.Length);
        }
        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        public void GetBytesReturnsFilledArrayWhenLessThanBuffer(int buffSize)
        {
            var tested = new BufferedRandomProvider(buffSize);
            var input = new byte[buffSize / 2];
            tested.GetBytes(input);
            var uniqueBytes = new HashSet<byte>();
            foreach (var b in input) { uniqueBytes.Add(b); }
            Assert.True(uniqueBytes.Count > 1);
            Assert.Equal(buffSize / 2, input.Length);
        }
        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        public void GetBytesReturnsFilledArrayWhenMoreThanBuffer(int buffSize)
        {
            var tested = new BufferedRandomProvider(buffSize);
            var input = new byte[buffSize * 2];
            tested.GetBytes(input);
            var uniqueBytes = new HashSet<byte>();
            foreach (var b in input) { uniqueBytes.Add(b); }
            Assert.True(uniqueBytes.Count > 1);
            Assert.Equal(buffSize * 2, input.Length);
        }
        [Fact]
        public void GetBytesConsecutiveCallsDoesNotReturnSameValues()
        {
            const int buffSize = 4;
            var tested = new BufferedRandomProvider(buffSize);
            var input = new byte[buffSize];
            tested.GetBytes(input);

            var uniqueBytes1 = new HashSet<byte>();
            foreach (var b in input) { uniqueBytes1.Add(b); }


            var input2 = new byte[buffSize];
            tested.GetBytes(input2);
            var uniqueBytes2 = new HashSet<byte>();
            foreach (var b in input2) { uniqueBytes2.Add(b); }

            Assert.False(input.SequenceEqual(input2));
        }
        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void GetBytesConsecutiveCallsDoesNotDegradePerformance(int numberOfCalls)
        {
            const int bufferSize = 100;
            var tested = new BufferedRandomProvider(bufferSize);
            var input = new byte[bufferSize];
            for (var i = 0; i < numberOfCalls; ++i)
            {
                tested.GetBytes(input);
                Assert.Equal(bufferSize, input.Length);
            }
        }
    }
}
