using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.Standard.RandomExtensions
{
    public sealed class RandomProviderMock : IRandomProvider
    {
        public readonly byte ByteToRepeat;

        public RandomProviderMock(byte byteToRepeat)
        {
            ByteToRepeat = byteToRepeat;
        }
        public void GetBytes(byte[] input)
        {
            for (int i = 0; i < input.Length; ++i)
            {
                input[i] = ByteToRepeat;
            }
        }
    }
}
