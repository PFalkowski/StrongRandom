using System;
using System.Collections.Generic;
using NSubstitute;
using Xunit;

namespace Extensions.Standard.Randomization.Test
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
            Assert.Equal(repeats, zeroesCounter + onesCounter);
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
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(999)]
        public void NextBoolReturnsTrueOrFalse(int rand)
        {
            var randomSubstitute = Substitute.For<Random>();
            randomSubstitute.Next().Returns(rand);
            var expected = rand % 2 == 0;
            Assert.Equal(expected, randomSubstitute.NextBool());
            Assert.Equal(expected, randomSubstitute.NextBool());
            Assert.Equal(expected, randomSubstitute.NextBool());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(999)]
        public void NextCharRetrunsValidResult(int low)
        {
            var high = low + 2;
            var randomSubstitute = Substitute.For<Random>();
            randomSubstitute.Next(Arg.Is(low), Arg.Is(high)).Returns(low);
            var expected = (char)low;
            Assert.Equal(expected, randomSubstitute.NextChar((char)low, (char)high));
        }

        [Theory]
        [InlineData('a')]
        [InlineData('b')]
        [InlineData('z')]
        public void NextLowercaseLetterRetrunsValidResult(int expected)
        {
            var randomSubstitute = Substitute.For<Random>();
            randomSubstitute.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(expected);
            var received = randomSubstitute.NextLowercaseLetter();
            randomSubstitute.DidNotReceive().Next(Arg.Is<int>(x => x < 'a'), Arg.Any<int>());
            randomSubstitute.DidNotReceive().Next(Arg.Any<int>(), Arg.Is<int>(x => x > 123));
            randomSubstitute.Received(1).Next(Arg.Is<int>(97), Arg.Is<int>(123));
        }

        [Theory]
        [InlineData('A')]
        [InlineData('B')]
        [InlineData('Z')]
        public void NextUppercaseLetterLetterRetrunsValidResult(int expected)
        {
            var randomSubstitute = Substitute.For<Random>();
            randomSubstitute.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(expected);
            var received = randomSubstitute.NextUppercaseLetter();
            randomSubstitute.DidNotReceive().Next(Arg.Is<int>(x => x < 'A'), Arg.Any<int>());
            randomSubstitute.DidNotReceive().Next(Arg.Any<int>(), Arg.Is<int>(x => x > 91));
            randomSubstitute.Received(1).Next(Arg.Is<int>(65), Arg.Is<int>(91));
        }

        [Fact]
        public void NextLetterRetrunsValidResult()
        {
            var randomSubstitute = Substitute.For<Random>();
            for (int i = 'A'; i < 'Z' + 1; ++i)
            {
                randomSubstitute.Next(Arg.Any<int>()).Returns(i - 'A');
                Assert.Equal(i, randomSubstitute.NextLetter());
            }
            for (int i = 'a'; i < 123; ++i)
            {
                randomSubstitute.Next(Arg.Any<int>()).Returns(i - 'a' + 26);
                Assert.Equal(i, randomSubstitute.NextLetter());
            }
        }

        [Theory]
        [InlineData("test")]
        [InlineData(".NETStandard")]
        public void NextAlphanumericRetrunsValidResult(string toChooseFrom)
        {
            var randomSubstitute = Substitute.For<Random>();
            for (int i = '0'; i < '9' + 1; ++i)
            {
                randomSubstitute.Next(Arg.Any<int>()).Returns(i - '0');
                Assert.Equal(i, randomSubstitute.NextAlphanumeric());
            }
            for (int i = 'A'; i < 'Z' + 1; ++i)
            {
                randomSubstitute.Next(Arg.Any<int>()).Returns(i - 'A' + 10);
                Assert.Equal(i, randomSubstitute.NextAlphanumeric());
            }
            for (int i = 'a'; i < 123; ++i)
            {
                randomSubstitute.Next(Arg.Any<int>()).Returns(i - 'a' + 26 + 10);
                Assert.Equal(i, randomSubstitute.NextAlphanumeric());
            }
        }

        [Fact]
        public void NextFloatRetrunsValidResult()
        {
            var randomSubstitute = Substitute.For<Random>();
            randomSubstitute.NextDouble().Returns(.5);
            Assert.Equal(.5, randomSubstitute.NextFloat());
            Assert.IsType<float>(randomSubstitute.NextFloat());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000000)]
        public void NextFloatRangeRetrunsValidResult(float max)
        {
            var randomSubstitute = Substitute.For<Random>();
            var almostOne = 0.9999999999999f;
            randomSubstitute.NextDouble().Returns(almostOne);
            var received = randomSubstitute.NextFloat(-max, max);
            Assert.Equal(max, received);
        }

        [Fact]
        public void NextFloatEdgeCase()
        {
            var randomSubstitute = Substitute.For<Random>();
            var almostOne = 0.9999999999999f;
            randomSubstitute.NextDouble().Returns(almostOne);
            var received = randomSubstitute.NextFloat(float.MinValue, float.MaxValue);
            Assert.Equal(float.MaxValue, received);
        }
    }
}
