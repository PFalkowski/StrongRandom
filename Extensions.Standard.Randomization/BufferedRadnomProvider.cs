﻿using System;
using System.Security.Cryptography;

namespace Extensions.Standard.Randomization
{
    /// <summary>
    /// Internally uses RNGCryptoServiceProvider. Buffered call to RNGCryptoServiceProvider up to bufferSize
    /// </summary>
    public class BufferedRandomProvider : IRandomProvider
    {
        public BufferedRandomProvider(int bufferSize)
        {
            _bufer = new Lazy<byte[]>(() => new byte[bufferSize], true);
        }

        private readonly Lazy<byte[]> _bufer;
        private int _currentIndex;

        private void GetBytesInternal(byte[] buffer)
        {
            using (var csprng = RandomNumberGenerator.Create())
            {
                csprng.GetBytes(buffer);
            }
        }

        private void RefreshBuffer()
        {
            GetBytesInternal(_bufer.Value);
            _currentIndex = 0;
        }

        public void GetBytes(byte[] input)
        {
            if (!_bufer.IsValueCreated)
            {
                RefreshBuffer();
            }
            if (input.Length > _bufer.Value.Length)
            {
                GetBytesInternal(input);
                return;
            }
            else if (input.Length + _currentIndex > _bufer.Value.Length)
            {
                RefreshBuffer();
            }

            for (var i = 0; i < input.Length; ++i)
            {
                input[i] = _bufer.Value[i + _currentIndex];
            }
            _currentIndex += input.Length;
        }
    }
}
