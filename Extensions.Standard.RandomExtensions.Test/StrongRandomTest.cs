using System;
using Extensions.Standard.RandomExtensions;
using NSubstitute;
using Xunit;

namespace Extensions.Standard.Randomization.Test
{
    public class StrongRandomTest
    {
        [Fact]
        public void CtorCreatesValidInstance()
        {
            var providerMock = new RandomProviderMock(0);
            var tested = new StrongRandom(providerMock);
        }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(byte.MaxValue)]
        public void NextBytesReturnsBytes(byte repeatedByte)
        {
            const int length = 10;
            var providerMock = new RandomProviderMock(repeatedByte);
            var tested = new StrongRandom(providerMock);

            var recevied = new byte[length];
            tested.NextBytes(recevied);

            Assert.Equal(length, recevied.Length);
            foreach (var el in recevied)
            {
                Assert.Equal(repeatedByte, el);
            }
        }

        [Fact]
        public void NextReturnsMaxInt()
        {
            var providerMock = new RandomProviderMock(255);
            var tested = new StrongRandom(providerMock);
            
            var received = tested.Next();
            Assert.Equal(int.MaxValue, received);
        }

        [Fact]
        public void NextReturnsZero()
        {
            var providerMock = new RandomProviderMock(0);
            var tested = new StrongRandom(providerMock);
            
            var received = tested.Next();
            Assert.Equal(0, received);
        }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(byte.MaxValue)]
        public void NextReturnsValidInt(byte repeatedByte)
        {
            //arrange
            var providerMock = new RandomProviderMock(repeatedByte);
            var tested = new StrongRandom(providerMock);

            byte[] tmpBytes = { repeatedByte, repeatedByte, repeatedByte, repeatedByte };
            var expected = BitConverter.ToInt32(tmpBytes, 0) & int.MaxValue;

            //act
            var received = tested.Next();

            //assert
            Assert.Equal(expected, received);
        }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(byte.MaxValue)]
        public void NextReturnsValidIntUnderSpecifiedArg(byte repeatedByte)
        {
            //arrange
            const int maxValue = 100;
            var providerMock = new RandomProviderMock(repeatedByte);
            var tested = new StrongRandom(providerMock);

            byte[] tmpBytes = { repeatedByte, repeatedByte, repeatedByte, repeatedByte };
            var tmpUInt32 = BitConverter.ToUInt32(tmpBytes, 0);
            var sample = tmpUInt32 / (1.0 + uint.MaxValue);
            var expected = (int)(sample * maxValue);
            //act
            var received = tested.Next(maxValue);

            //assert
            Assert.True(received < maxValue);
            Assert.Equal(expected, received);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(999)]
        [InlineData(15002900)]
        [InlineData(int.MaxValue)]
        public void NextAlwaysReturnsValidIntUnderSpecifiedArg(int myMax)
        {
            const byte byteRepeated = byte.MaxValue;
            //arrange
            var providerMock = new RandomProviderMock(byteRepeated);
            var tested = new StrongRandom(providerMock);

            //act
            var received = tested.Next(myMax);

            //assert
            if (myMax > 0)
            {
                Assert.True(received < myMax);
                Assert.Equal(myMax - 1, received);
            }
            else
            {
                Assert.True(received == myMax);
                Assert.Equal(0, received);
            }
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-15002900)]
        [InlineData(-999)]
        [InlineData(-2)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(999)]
        [InlineData(15002900)]
        public void NextReturnsValidIntInsideRangeForMaxValues(int myMin)
        {
            const byte byteRepeated = byte.MaxValue;
            const int maxValueAdditive = 100;
            int myMax = myMin + maxValueAdditive;
            //arrange
            var providerMock = new RandomProviderMock(byteRepeated);
            var tested = new StrongRandom(providerMock);

            //act
            var received = tested.Next(myMin, myMax);

            //assert
            Assert.True(received >= myMin);
            Assert.True(received < myMax);
            Assert.Equal(myMax - 1, received);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-15002900)]
        [InlineData(-999)]
        [InlineData(-2)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(999)]
        [InlineData(15002900)]
        public void NextReturnsValidIntInsideRangeForMinValues(int myMin)
        {
            const byte byteRepeated = byte.MinValue;
            const int maxValueAdditive = 100;
            int myMax = myMin + maxValueAdditive;
            //arrange
            var providerMock = new RandomProviderMock(byteRepeated);
            var tested = new StrongRandom(providerMock);

            //act
            var received = tested.Next(myMin, myMax);

            //assert
            Assert.True(received >= myMin);
            Assert.True(received < myMax);
            Assert.Equal(myMin, received);
        }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(byte.MaxValue)]
        public void NextDoubleReturnsValidDouble(byte repeatedByte)
        {
            //arrange
            var providerMock = new RandomProviderMock(repeatedByte);
            var tested = new StrongRandom(providerMock);

            byte[] tmpBytes = { repeatedByte, repeatedByte, repeatedByte, repeatedByte };
            var tmpUInt32 = BitConverter.ToUInt32(tmpBytes, 0);
            var expected = tmpUInt32 / (1.0 + uint.MaxValue);

            //act
            var received = tested.NextDouble();

            //assert
            Assert.Equal(expected, received);

        }

        [Fact]
        public void NextBytesThrowsArgumentExceptionWhenNullArrayPassedIn()
        {
            var providerMock = new RandomProviderMock(0);
            var tested = new StrongRandom(providerMock);
            byte[] nullBytesArray = null;
            Assert.Throws<ArgumentNullException>(() => tested.NextBytes(nullBytesArray));
        }
        [Fact]
        public void NextThrowsArgumentOutOfRangeExceptionWhenNegativeIntPassedIn()
        {
            var providerMock = new RandomProviderMock(0);
            var tested = new StrongRandom(providerMock);
            Assert.Throws<ArgumentOutOfRangeException>(() => tested.Next(-1));
        }
        [Fact]
        public void NextThrowsArgumentOutOfRangeExceptionWhenMinIsGreaterThanMax()
        {
            var providerMock = new RandomProviderMock(0);
            var tested = new StrongRandom(providerMock);
            Assert.Throws<ArgumentOutOfRangeException>(() => tested.Next(100, 90));
        }
    }
}
