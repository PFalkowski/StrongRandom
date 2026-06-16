using System;
using System.Security.Cryptography;

namespace Extensions.Standard.Randomization
{
    /// <summary>
    /// Buffered wrapper around <see cref="RandomNumberGenerator"/> that amortises expensive CSPRNG calls
    /// by filling an internal byte buffer in one shot and dispensing bytes from it on subsequent requests.
    /// </summary>
    public class BufferedRandomProvider : IRandomProvider
    {
        private readonly Lazy<byte[]> _buffer;
        private int _currentIndex;

        /// <param name="bufferSize">Number of random bytes to pre-fetch per CSPRNG call.</param>
        public BufferedRandomProvider(int bufferSize)
        {
            _buffer = new Lazy<byte[]>(() => new byte[bufferSize], true);
        }

        private static void GetBytesInternal(byte[] buffer)
        {
            using var csprng = RandomNumberGenerator.Create();
            csprng.GetBytes(buffer);
        }

        private void RefreshBuffer()
        {
            GetBytesInternal(_buffer.Value);
            _currentIndex = 0;
        }

        /// <summary>Fills <paramref name="input"/> with cryptographically-strong random bytes.</summary>
        public void GetBytes(byte[] input)
        {
            if (!_buffer.IsValueCreated)
            {
                RefreshBuffer();
            }

            if (input.Length > _buffer.Value.Length)
            {
                GetBytesInternal(input);
                return;
            }

            if (input.Length + _currentIndex > _buffer.Value.Length)
            {
                RefreshBuffer();
            }

            for (var i = 0; i < input.Length; ++i)
            {
                input[i] = _buffer.Value[i + _currentIndex];
            }
            _currentIndex += input.Length;
        }
    }
}
