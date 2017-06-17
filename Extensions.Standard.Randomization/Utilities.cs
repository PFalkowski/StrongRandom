using System;

namespace Extensions.Standard.Randomization
{
    public static partial class Utilities
    {
        /// <summary>
        ///     Get random number from 0 to 255 or specified upper limit.
        ///     Number is less than specified maximum.
        /// </summary>
        /// <param name="rng"></param>
        /// <param name="upperLimit"></param>
        /// <returns></returns>
        public static byte NextByte(this Random rng, short upperLimit = byte.MaxValue + 1)
        {
            return (byte)rng.Next(upperLimit);
        }

        public static bool NextBool(this Random rng)
        {
            return rng.Next() % 2 == 0; // or rng.NextDouble() < .5;
        }

        public static char NextChar(this Random rng, char lowerInclusive = (char)32, char upperExclusive = (char)127)
        {
            return Convert.ToChar(rng.Next(lowerInclusive, upperExclusive));
        }

        public static char NextLowercaseLetter(this Random rng)
        {
            return rng.NextChar((char)97, (char)123);
        }

        public static char NextUppercaseLetter(this Random rng)
        {
            return rng.NextChar((char)65, (char)91);
        }
        /// <summary>
        /// Returns a random float that is equal or greater than zero and less than 1.0
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static float NextFloat(this Random random)
        {
            var res = (float) random.NextDouble();
            return res;
        }

        public static float NextFloat(this Random random, float min, float max)
        {
            double tmp = min + random.NextDouble() * ((double)max - min);
            return (float) tmp;
        }

        public static char NextChar(this Random rng, string chooseFrom)
        {
            if (string.IsNullOrEmpty(chooseFrom)) return (char)0;
            return chooseFrom[rng.Next(chooseFrom.Length)];
        }

        public static char NextLetter(this Random rng)
        {
            return rng.NextChar("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
        }

        public static char NextAlphanumeric(this Random rng)
        {
            return rng.NextChar("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
        }

        //public static double NextDouble(this Random rng, double min, double max) // unsafe. Will overflow for larger min / max values. Needs fixing - mantisa & exponent randomization will be best
        //{
        //    return min + rng.NextDouble() * (max - min);
        //}

        /// <summary>
        ///     Box-Muller transform applied in order to get <b>normally-looking</b> double for provided mean and SD.
        ///     Inspired by http://stackoverflow.com/a/218600
        /// </summary>
        /// <param name="rng"></param>
        /// <param name="mean"></param>
        /// <param name="sd"></param>
        /// <returns></returns>
        public static double NextNormal(this Random rng, double mean, double sd = 1)
        {
            var rand = Math.Sqrt(-2.0 * Math.Log(rng.NextDouble())) * Math.Sin(2.0 * Math.PI * rng.NextDouble());
            return mean + sd * rand;
        }
    }
}
