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
            var tested = new BufferedRadnomProvider(buffSize);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void GetBytesReturnsFilledArray(int buffSize)
        {
            var tested = new BufferedRadnomProvider(buffSize);
            var input = new byte[buffSize];
            tested.GetBytes(input);
            Assert.Equal(buffSize, input.Length);
        }
        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        public void GetBytesReturnsFilledArrayWhenLessThanBuffer(int buffSize)
        {
            var tested = new BufferedRadnomProvider(buffSize);
            var input = new byte[buffSize / 2];
            tested.GetBytes(input);
            Assert.Equal(buffSize / 2, input.Length);
        }
        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        public void GetBytesReturnsFilledArrayWhenMoreThanBuffer(int buffSize)
        {
            var tested = new BufferedRadnomProvider(buffSize);
            var input = new byte[buffSize * 2];
            tested.GetBytes(input);
            Assert.Equal(buffSize * 2, input.Length);
        }
        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void GetBytesConsecutiveCallsDoNotDegradePerformance(int numberOfCalls)
        {
            const int bufferSize = 100;
            var tested = new BufferedRadnomProvider(bufferSize);
            var input = new byte[bufferSize];
            for (int i = 0; i < numberOfCalls; ++i)
            {
                tested.GetBytes(input);
                Assert.Equal(bufferSize, input.Length);
            }
        }
    }
}
