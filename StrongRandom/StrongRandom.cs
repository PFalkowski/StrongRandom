using System;
using System.Security.Cryptography;

namespace Extensions.Standard.RandomExtensions
{
    /// <summary>
    ///     Implements interface left by Random using RNGCryptoServiceProvider underneath
    /// </summary>
    public class StrongRandom : Random
    {
        private RandomNumberGenerator Provider { get; } = RandomNumberGenerator.Create();

        private int InternalSample()
        {
            var buffer = new byte[4];
            Provider.GetBytes(buffer);
            return BitConverter.ToInt32(buffer, 0) & int.MaxValue;
        }

        private double GetSampleForLargeRange()
        {
            var num = InternalSample();
            if (InternalSample() % 2 == 0)
            {
                num = -num;
            }
            return ((double)num + int.MaxValue - 1.0) / (int.MaxValue * 2.0 - 1.0);
        }

        protected override double Sample()
        {
            var buffer = new byte[4];
            Provider.GetBytes(buffer);
            var temp = BitConverter.ToUInt32(buffer, 0);
            return temp / (1.0 + uint.MaxValue); //perfectly in range of <0..1)
        }

        public override void NextBytes(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            Provider.GetBytes(buffer);
        }

        public override int Next()
        {
            return InternalSample();
        }

        public override int Next(int maxValue)
        {
            if (maxValue < 0) throw new ArgumentOutOfRangeException(nameof(maxValue));
            return (int)(Sample() * maxValue);
        }

        public override double NextDouble()
        {
            return Sample();
        }

        public override int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue));
            if (minValue == maxValue) return minValue;
            var range = maxValue - (long)minValue;
            if (range <= int.MaxValue)
            {
                return (int)(Sample() * range) + minValue;
            }
            return (int)((long)(GetSampleForLargeRange() * range) + minValue);
        }
    }
}